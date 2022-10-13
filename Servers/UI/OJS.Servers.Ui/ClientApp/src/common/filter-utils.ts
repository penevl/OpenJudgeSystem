/* eslint-disable import/prefer-default-export */
import { groupBy } from 'lodash';
import { FilterInfo, FilterSortType, FilterType, IContestParam, IFilter, SortType } from './contest-types';
import { areStringEqual } from '../utils/compare-utils';

const getNextIdGen = function* () {
    let index = 1;

    while (true) {
        yield index;
        index += 1;
    }
};

const getId = getNextIdGen();

const generateFilterItems = (type: FilterType, ...filters: FilterInfo[]) =>
    filters.map(({ name, value }) => ({
        name,
        type,
        value,
        id: getId.next().value,
    }));

const groupByType = (filters: IFilter[]) => {
    const groupedGroups = groupBy(filters, (f) => f.type);

    return Object.keys(groupedGroups)
        .map((groupType) => ({
            type: groupType as FilterType,
            filters: groupedGroups[groupType],
        }));
};

const filterByType = (filters: IContestParam<FilterSortType>[], filterType: FilterType) =>
    filters.filter(({ type }) => filterType === type);

const findFilterByTypeAndName = <T extends FilterSortType>(filters: IContestParam<T>[], type: string, value: any) => filters
    .find(({ type: filterType, id }) =>
        areStringEqual(filterType, type, false) &&
        areStringEqual(value, id, false));

export {
    generateFilterItems,
    groupByType,
    filterByType,
    findFilterByTypeAndName,
};
