import * as React from 'react';
import { useCallback, useEffect, useMemo } from 'react';
import { useNavigate } from 'react-router';
import { isNil } from 'lodash';
import { useSubmissionsDetails } from '../../../hooks/submissions/use-submissions-details';
import Heading, { HeadingType } from '../../guidelines/headings/Heading';
import List, { ListType, Orientation } from '../../guidelines/lists/List';
import { ISubmissionDetails } from '../../../hooks/submissions/types';
import { Button, ButtonType } from '../../guidelines/buttons/Button';
import concatClassNames from '../../../utils/class-names';
import SubmissionResultPointsLabel from '../submission-result-points-label/SubmissionResultPointsLabel';
import CodeEditor from '../../code-editor/CodeEditor';
import SubmissionResults from '../submission-results/SubmissionResults';
import styles from './SubmissionDetails.module.scss';
import { formatDate } from '../../../utils/dates';

const SubmissionDetails = () => {
    const {
        currentSubmission,
        setCurrentSubmissionId,
        currentProblemSubmissionResults,
        getSubmissionResults,
    } = useSubmissionsDetails();

    const problemNameHeadingText = useMemo(
        () => `${currentSubmission?.problem.name} - ${currentSubmission?.problem.id}`,
        [ currentSubmission?.problem.id, currentSubmission?.problem.name ],
    );
    const detailsHeadingText = useMemo(() => `Details #${currentSubmission?.id}`, [ currentSubmission?.id ]);
    const navigate = useNavigate();

    const submissionsNavigationClass = 'submissionsNavigation';
    const submissionsNavigationClassName = concatClassNames(styles.submissionsNavigation, submissionsNavigationClass);
    const submissionDetailsClass = 'submissionDetails';
    const submissionDetailsClassName = concatClassNames(styles.submissionDetails, submissionDetailsClass);
    const submissionsNavigationListClass = 'submissionsSideNavigationList';
    const submissionsNavigationListClassName = concatClassNames(styles.sideNavigation, submissionsNavigationListClass);
    const submissionListItemClass = 'submissionListItem';
    const submissionListItemClassName = concatClassNames(styles.submissionListItem, submissionListItemClass);

    useEffect(() => {
        if (isNil(currentSubmission)) {
            return;
        }

        (async () => {
            await getSubmissionResults(currentSubmission.problem.id);
        })();
    }, [ currentSubmission, getSubmissionResults ]);

    const handleSubmissionClick = useCallback((submissionId: number) => {
        setCurrentSubmissionId(submissionId);
        navigate(`/submissions/${submissionId}/details`);
    }, [ navigate, setCurrentSubmissionId ]);

    const renderSubmissionListItem = useCallback((submissionDetails: ISubmissionDetails) => {
        const selectedClassName = submissionDetails.id === currentSubmission?.id
            ? styles.selected
            : '';

        const className = concatClassNames(
            styles.submissionsNavigationItem,
            selectedClassName,
            'submissionBtn',
        );
        return (
            <>
                <Button
                  type={ButtonType.plain}
                  className={className}
                  onClick={() => {
                      handleSubmissionClick(submissionDetails.id);
                  }}
                >
                    {submissionDetails.submissionType}
                </Button>
                <SubmissionResultPointsLabel
                  points={submissionDetails.points}
                  maximumPoints={submissionDetails.maximumPoints}
                  isProcessed={submissionDetails.isProcessed}
                />
                <p className={styles.submissionCreatedOnParagraph}>{formatDate(new Date(submissionDetails.createdOn))}</p>
            </>
        );
    }, [ currentSubmission, handleSubmissionClick ]);

    const renderSubmissionsForProblem =
        useCallback(
            () => (
                <List
                  values={currentProblemSubmissionResults}
                  keyFunc={(v) => v.id.toString()}
                  className={submissionsNavigationListClassName}
                  itemFunc={renderSubmissionListItem}
                  itemClassName={submissionListItemClassName}
                  type={ListType.normal}
                  orientation={Orientation.vertical}
                />
            ),
            [
                currentProblemSubmissionResults,
                renderSubmissionListItem,
                submissionListItemClassName,
                submissionsNavigationListClassName,
            ],
        );

    if (isNil(currentSubmission)) {
        return <div>No details fetched.</div>;
    }

    return (
        <div className={styles.detailsWrapper}>
            <div className={submissionsNavigationClassName}>
                <Heading type={HeadingType.secondary}>Submissions</Heading>
                {renderSubmissionsForProblem()}
            </div>
            <div className={styles.code}>
                <Heading
                  type={HeadingType.secondary}
                  className={styles.taskHeading}
                >
                    {problemNameHeadingText}
                </Heading>
                <CodeEditor
                  readOnly
                  code={currentSubmission?.content}
                />
            </div>
            <div className={submissionDetailsClassName}>
                <Heading type={HeadingType.secondary}>{detailsHeadingText}</Heading>
                <SubmissionResults testRuns={currentSubmission.testRuns} />
            </div>
        </div>
    );
};

export default SubmissionDetails;
