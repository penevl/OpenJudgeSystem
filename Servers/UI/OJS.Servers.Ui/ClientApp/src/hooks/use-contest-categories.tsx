import React, { createContext, useCallback, useContext, useEffect, useState } from 'react';
import { isEmpty } from 'lodash';
import { IHaveChildrenProps } from '../components/common/Props';
import { IContestCategoryTreeType } from '../common/types';
import { useHttp } from './use-http';
import { useUrls } from './use-urls';
import { useLoading } from './use-loading';

interface IContestCategoriesContext {
    state: {
        categories: IContestCategoryTreeType[];
        isLoaded: boolean,
    };
    actions: {
        load: () => Promise<void>;
    };
}

interface IContestCategoriesProviderProps extends IHaveChildrenProps {
}

const defaultState = { state: { categories: [] as IContestCategoryTreeType[] } };

const ContestCategoriesContext = createContext<IContestCategoriesContext>(defaultState as IContestCategoriesContext);

const ContestCategoriesProvider = ({ children }: IContestCategoriesProviderProps) => {
    const [ categories, setCategories ] = useState(defaultState.state.categories);
    const { getCategoriesTreeUrl } = useUrls();
    const { startLoading, stopLoading } = useLoading();

    const {
        get,
        data,
        isSuccess,
    } = useHttp(getCategoriesTreeUrl);

    const load = useCallback(
        async () => {
            startLoading();
            await get();
            stopLoading();
        },
        [ get, startLoading, stopLoading ],
    );

    useEffect(
        () => {
            if (isEmpty(data)) {
                return;
            }

            setCategories(data as IContestCategoryTreeType[]);
        },
        [ data ],
    );

    const value = {
        state: {
            categories,
            isLoaded: isSuccess,
        },
        actions: { load },
    };

    return (
        <ContestCategoriesContext.Provider value={value}>
            {children}
        </ContestCategoriesContext.Provider>
    );
};

const useContestCategories = () => useContext(ContestCategoriesContext);

export default ContestCategoriesProvider;

export {
    useContestCategories,
};