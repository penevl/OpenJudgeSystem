import React, { useEffect, useMemo, useState } from 'react';

import concatClassNames from '../../../utils/class-names';
import { IHaveChildrenProps } from '../../common/Props';

import styles from './Collapsible.module.scss';

interface ICollapsibleComponentProps extends IHaveChildrenProps {
    collapsed: boolean;
    className?: string | undefined;
}

const Collapsible = ({
    collapsed,
    children,
    className,
}: ICollapsibleComponentProps) => {
    const [ internalCollapsed, setInternalCollapsed ] = useState(collapsed);

    const internalContainerClassName = useMemo(() => concatClassNames(
        styles.collapsibleContainer,
        internalCollapsed
            ? ''
            : 'collapsibleContainerHidden',
        internalCollapsed
            ? styles.visible
            : styles.hidden,
        className || '',
    ), [ internalCollapsed, className ]);

    useEffect(() => {
        setInternalCollapsed(collapsed);
    }, [ collapsed ]);

    return (
        <div className={internalContainerClassName}>
            {children}
        </div>
    );
};

export default Collapsible;
