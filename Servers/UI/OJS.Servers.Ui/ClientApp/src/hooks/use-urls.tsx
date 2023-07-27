import React, { createContext, useContext, useMemo } from 'react';
import isNil from 'lodash/isNil';

import { SearchParams } from '../common/search-types';
import {
    IAllContestsUrlParams,
    IDownloadProblemResourceUrlParams,
    IDownloadSubmissionFileUrlParams,
    IGetContestByProblemUrlParams,
    IGetContestParticipationScoresForParticipantUrlParams,
    IGetContestResultsParams,
    IGetPublicSubmissionsUrlParams,
    IGetSearchResultsUrlParams,
    IGetSubmissionDetailsByIdUrlParams,
    IGetSubmissionResultsByProblemUrlParams,
    IRetestSubmissionUrlParams,
    IStartContestParticipationUrlParams,
    ISubmitContestPasswordUrlParams,
} from '../common/url-types';
import { IHaveChildrenProps } from '../components/common/Props';

interface IUrlsContext {
    getLoginSubmitUrl: () => string;
    getLogoutUrl: () => string;
    getUserAuthInfoUrl: () => string;
    getPlatformRegisterUrl: () => string;
    getAdministrationContestsGridUrl: () => string;
    getAdministrationNavigation: () => string;
    getProfileInfoUrl: () => string;
    getSubmissionsForProfileUrl: () => string;
    getParticipationsForProfileUrl: () => string;
    getIndexContestsUrl: () => string;
    getAllContestsUrl: (params: IAllContestsUrlParams) => string;
    getRegisterForContestUrl: (params: IStartContestParticipationUrlParams) => string;
    getSubmitContestPasswordUrl: (params: ISubmitContestPasswordUrlParams) => string;
    getStartContestParticipationUrl: (params: IStartContestParticipationUrlParams) => string;
    getContestParticipantScoresForParticipantUrl: (params: IGetContestParticipationScoresForParticipantUrlParams) => string;
    getSubmissionResultsByProblemUrl: (params: IGetSubmissionResultsByProblemUrlParams) => string;
    getSubmissionDetailsResultsUrl: (params: IGetSubmissionDetailsByIdUrlParams) => string;
    getPublicSubmissionsUrl: (params: IGetPublicSubmissionsUrlParams) => string;
    getSubmissionsTotalCountUrl: () => string;
    getSubmissionsDetailsUrl: () => string;
    getSubmissionDetailsByIdUrl: (params: IGetSubmissionDetailsByIdUrlParams) => string;
    getSubmitUrl: () => string;
    getSubmitFileUrl: () => string;
    getDownloadProblemResourceUrl: (params: IDownloadProblemResourceUrlParams) => string;
    getCategoriesTreeUrl: () => string;
    getAllContestStrategyFiltersUrl: () => string;
    getContestResultsUrl: (params: IGetContestResultsParams) => string;
    getHomeStatisticsUrl: () => string;
    getAdministrationRetestSubmission: (params: IRetestSubmissionUrlParams) => string;
    getSearchResults: (searchTerm: IGetSearchResultsUrlParams) => string;
    getContestByProblemUrl: (params: IGetContestByProblemUrlParams) => string;
    getSubmissionFileDownloadUrl: (params: IDownloadSubmissionFileUrlParams) => string;
}

const UrlsContext = createContext<IUrlsContext>({} as IUrlsContext);

type IUrlsProviderProps = IHaveChildrenProps

const
    {
        URLS:
            {
                UI_URL: baseUrl,
                ADMINISTRATION_URL: administrationBaseUrl,
                PLATFORM_URL: platformBaseUrl,
            },
    } = window;

const baseApiUrl = `${baseUrl}/api`;

// auth
const getLoginSubmitUrl = () => `${baseUrl}/Account/Login`;
const getLogoutUrl = () => `${baseUrl}/Account/Logout`;

const getUserAuthInfoUrl = () => `${baseApiUrl}/Users/GetUserAuthInfo`;
const getPlatformRegisterUrl = () => `${platformBaseUrl}/identity/register`;

// admin
const getAdministrationContestsGridUrl = () => `${administrationBaseUrl}/Contests`;
const getAdministrationNavigation = () => '/administration';
const getAdministrationRetestSubmission = ({ id }: IRetestSubmissionUrlParams) => `
${administrationBaseUrl}/Submissions/Retest?PK=${id}`;

// profile
const getProfileInfoUrl = () => `${baseApiUrl}/Users/GetProfileInfo`;
const getSubmissionsForProfileUrl = () => `${baseApiUrl}/Submissions/GetForProfile`;
const getParticipationsForProfileUrl = () => `${baseApiUrl}/Participations/GetForProfile`;

// contests
const getIndexContestsUrl = () => `${baseApiUrl}/Contests/GetForHomeIndex`;
const getAllContestsUrl = ({ filters, sorting, page }: IAllContestsUrlParams) => {
    const filtersQuery = `${filters
        .map(({ value, type }) => `${type.toLowerCase()}=${value}`)
        .join('&')
    }`;

    const sortingQuery = `${sorting
        .map(({ value, type }) => `${type.toLowerCase()}=${value}`)
        .join('&')
    }`;

    const pageQuery = isNil(page)
        ? ''
        : `page=${page}`;

    return `${baseApiUrl}/Contests/GetAll?${filtersQuery}&${sortingQuery}&${pageQuery}`;
};

const getRegisterForContestUrl = ({
    id,
    isOfficial,
}: IStartContestParticipationUrlParams) => `${baseApiUrl}/Contests/Register/${id}?official=${isOfficial}`;

const getSubmitContestPasswordUrl = ({
    id,
    isOfficial,
}: ISubmitContestPasswordUrlParams) => `${baseApiUrl}/Contests/SubmitContestPassword/${id}?official=${isOfficial}`;

const getStartContestParticipationUrl = ({
    id,
    isOfficial,
}: IStartContestParticipationUrlParams) => `${baseApiUrl}/Compete/Index/${id}?official=${isOfficial}`;

const getContestParticipantScoresForParticipantUrl =
    ({ participantId }: IGetContestParticipationScoresForParticipantUrlParams) => `
    ${baseApiUrl}/ParticipantScores/GetScoresForParticipant/${participantId}`;

const getContestByProblemUrl = ({ problemId }: IGetContestByProblemUrlParams) => `${baseApiUrl}/Contests/GetByProblem/${problemId}`;

const getCategoriesTreeUrl =
    () => `${baseApiUrl}/ContestCategories/GetCategoriesTree`;

const getContestResultsUrl = ({
    id,
    official,
    full,
} : IGetContestResultsParams) => `${baseApiUrl}/ContestResults/GetResults/${id}?official=${official}&full=${full}`;

// submissions
const getSubmissionResultsByProblemUrl = ({
    problemId,
    isOfficial,
    take,
}: IGetSubmissionResultsByProblemUrlParams) => `
    ${baseApiUrl}/Submissions/GetSubmissionResultsByProblem/${problemId}?isOfficial=${isOfficial}&take=${take}`;

const getSubmissionDetailsResultsUrl = ({
    submissionId,
    isOfficial,
    take,
}: IGetSubmissionDetailsByIdUrlParams) => `
    ${baseApiUrl}/Submissions/GetSubmissionDetailsResults/${submissionId}?isOfficial=${isOfficial}&take=${take}`;
const getPublicSubmissionsUrl = ({ page }: IGetPublicSubmissionsUrlParams) => {
    const pageQuery = isNil(page)
        ? ''
        : `page=${page}`;

    return `${baseApiUrl}/Submissions/Public?${pageQuery}`;
};
const getSubmissionsTotalCountUrl = () => `${baseApiUrl}/Submissions/TotalCount`;
const getSubmissionsDetailsUrl = () => `${baseApiUrl}/Submissions/Details`;
const getSubmissionDetailsByIdUrl =
    ({ submissionId }: IGetSubmissionDetailsByIdUrlParams) => `${getSubmissionsDetailsUrl()}/${submissionId}`;
const getSubmitUrl = () => `${baseApiUrl}/Compete/Submit`;
const getSubmitFileUrl = () => `${baseApiUrl}/Compete/SubmitFileSubmission`;
const getSubmissionFileDownloadUrl =
    ({ id }: IDownloadSubmissionFileUrlParams) => `${baseApiUrl}/Submissions/Download/${id}`;

// Submission types
const getAllContestStrategyFiltersUrl =
    () => `${baseApiUrl}/SubmissionTypes/GetAllOrderedByLatestUsage`;

// problem resources
const getDownloadProblemResourceUrl = ({ id }: IDownloadProblemResourceUrlParams) => `${baseApiUrl}/ProblemResources/GetResource/${id}`;

// Statistics
const getHomeStatisticsUrl = () => `${baseApiUrl}/StatisticsPreview/GetForHome`;

// Search
const getSearchResults = ({ searchTerm, page, selectedTerms }: IGetSearchResultsUrlParams) => {
    const searchQuery = `${SearchParams.search}=${searchTerm}`;

    const pageQuery = `page=${page}`;

    const selectedTermQuery = `&${selectedTerms
        .map(({ key, value }) => `${key}=${value}`)
        .join('&')
    }`;

    return `${baseApiUrl}/Search/GetSearchResults?${searchQuery}${selectedTermQuery}&${pageQuery}`;
};

const UrlsProvider = ({ children }: IUrlsProviderProps) => {
    const value = useMemo(
        () => ({
            getLoginSubmitUrl,
            getLogoutUrl,
            getUserAuthInfoUrl,
            getPlatformRegisterUrl,
            getAdministrationContestsGridUrl,
            getAdministrationNavigation,
            getAllContestsUrl,
            getRegisterForContestUrl,
            getSubmitContestPasswordUrl,
            getStartContestParticipationUrl,
            getContestParticipantScoresForParticipantUrl,
            getDownloadProblemResourceUrl,
            getSubmissionResultsByProblemUrl,
            getSubmissionDetailsResultsUrl,
            getPublicSubmissionsUrl,
            getSubmissionsTotalCountUrl,
            getIndexContestsUrl,
            getProfileInfoUrl,
            getSubmissionsDetailsUrl,
            getSubmissionDetailsByIdUrl,
            getSubmissionsForProfileUrl,
            getParticipationsForProfileUrl,
            getSubmitUrl,
            getSubmitFileUrl,
            getCategoriesTreeUrl,
            getAllContestStrategyFiltersUrl,
            getContestResultsUrl,
            getHomeStatisticsUrl,
            getAdministrationRetestSubmission,
            getSearchResults,
            getContestByProblemUrl,
            getSubmissionFileDownloadUrl,
        }),
        [],
    );

    return (
        <UrlsContext.Provider value={value}>
            {children}
        </UrlsContext.Provider>
    );
};

const useUrls = () => useContext(UrlsContext);

export default UrlsProvider;

export {
    useUrls,
};
