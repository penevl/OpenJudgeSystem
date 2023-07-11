import React, { useCallback, useMemo } from 'react';
import isNil from 'lodash/isNil';

import { useProblems } from '../../../hooks/use-problems';
import { format } from '../../../utils/number-utils';
import IconSize from '../../guidelines/icons/common/icon-sizes';
import QuestionIcon from '../../guidelines/icons/QuestionIcon';
import List from '../../guidelines/lists/List';

import styles from './ProblemConstraints.module.scss';

const ProblemConstraints = () => {
    const { state: { currentProblem } } = useProblems();

    const constraints = useMemo(() => {
        if (isNil(currentProblem)) {
            return [];
        }

        const { memoryLimit, fileSizeLimit, timeLimit, checkerName } = currentProblem;

        return [ `Allowed working time: ${format(timeLimit).toString()} sec.`,
            `Allowed memory: ${format(memoryLimit).toString()} MB`,
            `Size limit: ${format(fileSizeLimit).toString()} MB`,
            `Checker: ${checkerName}` ];
    }, [ currentProblem ]);

    const renderConstraint = useCallback(
        (constraint: string) => <span>{constraint}</span>,
        [],
    );

    const description = useMemo(() => {
        const { checkerDescription = '' } = currentProblem || {};
        return checkerDescription;
    }, [ currentProblem ]);

    return (
        <div className={styles.constraints}>
            <List
              values={constraints}
              itemFunc={renderConstraint}
              className={styles.constraintsList}
            />
            <QuestionIcon size={IconSize.Medium} className={styles.questionIcon} helperText={description} />
        </div>
    );
};

export default ProblemConstraints;