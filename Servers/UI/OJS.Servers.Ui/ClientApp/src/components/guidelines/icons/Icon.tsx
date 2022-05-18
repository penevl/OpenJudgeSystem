import React, { useCallback } from 'react';
import { IconBaseProps } from 'react-icons/lib/cjs/iconBase';
import { IHaveOptionalClassName } from '../../common/Props';
import concatClassNames from '../../../utils/class-names';
import styles from './Icon.module.scss';
import IconSize from './icon-sizes';

interface IIconProps extends IHaveOptionalClassName {
    size?: IconSize;
    helperText?: string;
}

interface IIconInternalProps extends IIconProps {
    Component: React.FC<IconBaseProps>;
}

const InProgressIcon = ({
    Component,
    className = '',
    size = IconSize.Medium,
    helperText = '',
}: IIconInternalProps) => {
    const sizeClassName =
        size === IconSize.Small
            ? styles.small
            : size === IconSize.Medium
                ? styles.medium
                : styles.large;

    const iconClassName = concatClassNames(
        styles.icon,
        className,
    );

    const iconContainerClassName = concatClassNames(
        styles.iconContainer,
        sizeClassName,
    );

    const renderHelperText = useCallback(
        () => {
            if (helperText === '') {
                return null;
            }
            return <span className={styles.helperText}>{helperText}</span>;
        },
        [ helperText ],
    );

    return (
        <div className={iconContainerClassName}>
            <Component className={iconClassName} />
            {renderHelperText()}
        </div>
    );
};

export default InProgressIcon;

export type {
    IIconProps,
};