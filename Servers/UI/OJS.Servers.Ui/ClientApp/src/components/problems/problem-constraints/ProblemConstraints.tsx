import React, { useCallback, useMemo } from 'react';
import isEmpty from 'lodash/isEmpty';
import isNil from 'lodash/isNil';

import { useProblems } from '../../../hooks/use-problems';
import IconSize from '../../guidelines/icons/common/icon-sizes';
import QuestionIcon from '../../guidelines/icons/QuestionIcon';
import List from '../../guidelines/lists/List';

import styles from './ProblemConstraints.module.scss';

enum ProblemConstraintsTitles {
    TimeLimit = 'Allowed working time',
    MemoryLimit = 'Allowed memory',
    FileSizeLimit = 'Size limit',
    Checker = 'Checker'
}

const ProblemConstraints = () => {
    const { state: { currentProblem } } = useProblems();

    const constraints = useMemo(() => {
        if (isNil(currentProblem)) {
            return [];
        }

        const { memoryLimit, fileSizeLimit, timeLimit, checkerName } = currentProblem;

        return [
            !isNil(timeLimit)
                ? `${ProblemConstraintsTitles.TimeLimit}: ${timeLimit.toFixed(3)} sec.`
                : '',
            !isNil(memoryLimit)
                ? `${ProblemConstraintsTitles.MemoryLimit}: ${memoryLimit.toFixed(2)} MB`
                : '',
            !isNil(fileSizeLimit)
                ? `${ProblemConstraintsTitles.FileSizeLimit}: ${fileSizeLimit.toFixed(2)} KB`
                : '',
            !isNil(checkerName)
                ? `${ProblemConstraintsTitles.Checker}: ${checkerName}`
                : '',
        ].filter((item) => !isEmpty(item));
    }, [ currentProblem ]);

    const renderConstraint = useCallback((constraint: string) => {
        const { checkerDescription = '' } = currentProblem || {};

        const constraintElement = <span>{constraint}</span>;

        return constraint.includes(ProblemConstraintsTitles.Checker)
            ? (
                <>
                    {constraintElement}
                    <QuestionIcon
                      className={styles.questionIcon}
                      size={IconSize.Medium}
                      helperText={checkerDescription}
                    />
                </>
            )
            : constraintElement;
    }, [ currentProblem ]);

    return (
        <div className={styles.constraints}>
            <List
              values={constraints}
              itemFunc={renderConstraint}
              className={styles.constraintsList}
            />
        </div>
    );
};

export default ProblemConstraints;
