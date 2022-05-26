import React, { useCallback, useEffect, useState } from 'react';
import { isNil } from 'lodash';
import List, { Orientation } from '../../guidelines/lists/List';
import Heading, { HeadingType } from '../../guidelines/headings/Heading';

import { FilterType, IFilter } from '../../../common/contest-types';
import ContestCategories from '../contest-categories/ContestCategories';

import styles from './ContestFilters.module.scss';
import Button, { ButtonSize, ButtonType } from '../../guidelines/buttons/Button';
import { useContests } from '../../../hooks/use-contests';
import { groupByType } from '../../../common/filter-utils';
import ExpandButton from '../../guidelines/buttons/ExpandButton';
import concatClassNames from '../../../utils/class-names';

interface IFiltersGroup {
    type: FilterType;
    filters: IFilter[];
}

const ContestFilters = () => {
    const [ filtersGroups, setFiltersGroups ] = useState<IFiltersGroup[]>([]);
    const [ expanded, setExpanded ] = useState(false);

    const {
        state: {
            possibleFilters,
            filters,
        },
        actions: { applyFilter },
    } = useContests();

    const handleFilterClick = useCallback(
        (filterId: number) => {
            const filter = possibleFilters.find(({ id }) => filterId === id);
            if (isNil(filter)) {
                return;
            }

            applyFilter(filter);
        },
        [ applyFilter, possibleFilters ],
    );

    const renderFilterItem = useCallback(
        ({ id, name }: IFilter) => {
            const filterIsSelected = filters.some((f) => f.id === id);
            const type = filterIsSelected
                ? ButtonType.primary
                : ButtonType.secondary;
            const size = filterIsSelected
                ? ButtonSize.medium
                : ButtonSize.small;

            return (
                <Button
                  type={type}
                  onClick={() => handleFilterClick(id)}
                  className={styles.btnSelectFilter}
                  text={name}
                  size={size}
                />
            );
        },
        [ handleFilterClick, filters ],
    );

    const toggleFiltersExpanded = useCallback(
        () => {
            setExpanded(!expanded);
        },
        [ expanded ],
    );

    const renderExpandButton = useCallback(
        (allFilters: IFilter[]) => {
            const maxFiltersToDisplayCount = 3;
            return allFilters.length > maxFiltersToDisplayCount
                ? <ExpandButton onClick={toggleFiltersExpanded} />
                : null;
        },
        [ toggleFiltersExpanded ],
    );

    const renderFilter = useCallback(
        (fg: IFiltersGroup) => {
            const { type, filters: groupFilters } = fg;
            const className = concatClassNames(
                styles.listFilterItems,
                expanded
                    ? styles.expanded
                    : '',
            );

            return (
                <div className={styles.filterTypeContainer}>
                    <Heading
                      type={HeadingType.small}
                      className={styles.heading}
                    >
                        {type}
                    </Heading>
                    <List
                      values={groupFilters}
                      itemFunc={renderFilterItem}
                      orientation={Orientation.horizontal}
                      className={className}
                      itemClassName={styles.listFilterItem}
                    />
                    {renderExpandButton(groupFilters)}
                </div>
            );
        },
        [ expanded, renderFilterItem, renderExpandButton ],
    );

    useEffect(
        () => {
            const plainFilters = possibleFilters.filter(({ type }) => type !== FilterType.Category);
            setFiltersGroups(groupByType(plainFilters));
        },
        [ possibleFilters ],
    );

    return (
        <div className={styles.container}>
            <ContestCategories className={styles.filterTypeContainer} />
            <List
              values={filtersGroups}
              itemFunc={renderFilter}
              fullWidth
            />
        </div>
    );
};

export default ContestFilters;
