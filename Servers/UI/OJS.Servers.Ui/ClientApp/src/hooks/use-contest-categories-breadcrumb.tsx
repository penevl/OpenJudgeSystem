import React, { createContext, useCallback, useContext, useMemo, useState } from 'react';
import isNil from 'lodash/isNil';

import ITreeItemType from '../common/tree-types';
import { IHaveChildrenProps } from '../components/common/Props';

interface ICategoriesBreadcrumbContext {
    state: {
        breadcrumbItems: ICategoriesBreadcrumbItem[];
        selectedBreadcrumbCategoryId: string;
    };
    actions: {
        updateBreadcrumb: (category: ITreeItemType | undefined, flattenTree: ITreeItemType[] | []) => void;
        clearBreadcrumb: () => void;
    };
}

type ICategoriesBreadcrumbProviderProps = IHaveChildrenProps

interface ICategoriesBreadcrumbItem {
    id: string;
    isLast: boolean;
    value: string;
    orderBy: number;
}

const defaultState = { state: { breadcrumbItems: [] as ICategoriesBreadcrumbItem[], selectedBreadcrumbCategoryId: '' } };

const CategoriesBreadcrumbContext = createContext<ICategoriesBreadcrumbContext>(defaultState as ICategoriesBreadcrumbContext);

const orderByAsc = (x : ICategoriesBreadcrumbItem, y: ICategoriesBreadcrumbItem) => y.orderBy - x.orderBy;

const CategoriesBreadcrumbProvider = ({ children }: ICategoriesBreadcrumbProviderProps) => {
    const [ breadcrumbItems, setBreadcrumbItems ] = useState(defaultState.state.breadcrumbItems);
    const [ selectedBreadcrumbCategoryId, setSelectedBreadcrumbCategoryId ] =
        useState<string>(defaultState.state.selectedBreadcrumbCategoryId);

    const clearBreadcrumb = useCallback(
        () => setBreadcrumbItems([]),
        [],
    );

    const updateBreadcrumb = useCallback(
        (category: ITreeItemType | undefined, categoriesTree: ITreeItemType[] | []) => {
            if (isNil(category) || isNil(categoriesTree)) {
                return;
            }

            const { id, name, parentId } = category;
            setSelectedBreadcrumbCategoryId(id);

            const allBreadcrumbItems = [] as ICategoriesBreadcrumbItem[];
            let index = 0;

            allBreadcrumbItems.push({
                id,
                value: name,
                isLast: true,
                orderBy: index,
            } as ICategoriesBreadcrumbItem);

            const populateBreadcrumbItemsByParents = (categoryParentId?: string) => {
                if (isNil(categoryParentId)) {
                    return;
                }

                index += 1;

                const { id: parentCategoryId, name: parentCategoryName, parentId: currentParrentId } = categoriesTree
                    .find((x) => x.id === categoryParentId) as ITreeItemType;

                if (isNil(parentCategoryId)) {
                    return;
                }

                allBreadcrumbItems.push({
                    id: parentCategoryId,
                    value: parentCategoryName,
                    isLast: false,
                    orderBy: index,
                } as ICategoriesBreadcrumbItem);

                populateBreadcrumbItemsByParents(currentParrentId);
            };

            populateBreadcrumbItemsByParents(parentId);

            allBreadcrumbItems
                .sort(orderByAsc);

            setBreadcrumbItems([ ...allBreadcrumbItems ]);
        },
        [ setBreadcrumbItems ],
    );

    const value = useMemo(
        () => ({
            state: { breadcrumbItems, selectedBreadcrumbCategoryId },
            actions: { updateBreadcrumb, clearBreadcrumb },
        }),
        [ breadcrumbItems, updateBreadcrumb, clearBreadcrumb, selectedBreadcrumbCategoryId ],
    );

    return (
        <CategoriesBreadcrumbContext.Provider value={value}>
            {children}
        </CategoriesBreadcrumbContext.Provider>
    );
};

const useCategoriesBreadcrumbs = () => useContext(CategoriesBreadcrumbContext);

export default CategoriesBreadcrumbProvider;

export {
    useCategoriesBreadcrumbs,
};

export type {
    ICategoriesBreadcrumbItem,
};
