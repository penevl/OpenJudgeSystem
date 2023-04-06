import React from 'react';

import { IUserSearchType } from '../../../common/search-types';
import concatClassNames from '../../../utils/class-names';

import styles from './SearchUser.module.scss';

interface ISearchUser {
    user: IUserSearchType;
}

const SearchUser = ({ user }: ISearchUser) => {
    const searchUserText = 'search-user-text';
    const searchUserClassName = concatClassNames(styles.userText, searchUserText);
    const searchUserElement = 'search-user-element';
    const searchUserElementClassName = concatClassNames(styles.userElement, searchUserElement);
    return (
        <div className={searchUserElementClassName}>
            <span
              className={searchUserClassName}
            >
                {user.name}
            </span>
        </div>
    );
};

export default SearchUser;