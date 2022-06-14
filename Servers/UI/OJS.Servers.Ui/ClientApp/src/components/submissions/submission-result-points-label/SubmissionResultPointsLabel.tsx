import * as React from 'react';
import { useMemo } from 'react';
import Label, { LabelType } from '../../guidelines/labels/Label';
import styles from './SubmissionResultPointsLabel.module.scss';

interface ISubmissionResultPointsLabelProps {
    points: number;
    maximumPoints: number;
    isProcessed: boolean
}

const SubmissionResultPointsLabel = ({
    points,
    maximumPoints,
    isProcessed,
}: ISubmissionResultPointsLabelProps) => {
    const labelType = points === 0
        ? LabelType.warning
        : points === 100
            ? LabelType.success
            : LabelType.info;

    const currentPoints = isProcessed
        ? points
        : '?';

    const text = useMemo(() => `${currentPoints}/${maximumPoints}`, [ currentPoints, maximumPoints ]);

    return (
        <Label
          className={styles.resultLabel}
          type={labelType}
        >
            {text}
        </Label>
    );
};

export default SubmissionResultPointsLabel;