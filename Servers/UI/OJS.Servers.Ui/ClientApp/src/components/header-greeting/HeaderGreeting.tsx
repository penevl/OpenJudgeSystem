import * as React from 'react';

import styles from './HeaderGreeting.module.scss';
import { useAuth } from '../../hooks/use-auth';

const HeaderGreeting = () => {
    const { getUser } = useAuth();
    const user = getUser();

    const className = user.isLoggedIn
        ? ''
        : styles.hidden;

    const text = user.isLoggedIn
        ? `Hello, ${user.username}`
        : '';

    return (
        <p className={className}>{text}</p>
    );
};

export default HeaderGreeting;