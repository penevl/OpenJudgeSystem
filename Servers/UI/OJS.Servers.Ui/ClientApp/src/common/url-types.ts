/* eslint-disable import/prefer-default-export */
import { IFilter, ISort } from './contest-types';

interface IStartContestUrlParams {
    id: number;
    official: boolean;
}

interface IGetContestByProblemUrlParams {
    problemId: number;
}

interface IAllContestsUrlParams {
    filters: IFilter[];
    sorting: ISort[];
    page?: number;
}

interface IContestCategoriesUrlParams {
    id: number;
}

interface IRegisterForContestUrlParams {
    id: number;
    isOfficial: boolean;
}

interface IStartContestParticipationUrlParams {
    id: number;
    isOfficial: boolean;
}

interface IGetContestParticipationScoresForParticipantUrlParams {
    participantId: number;
}

interface ISubmitContestPasswordUrlParams {
    id: number;
    isOfficial: boolean;
}

interface IDownloadProblemResourceUrlParams {
    id: number | null;
}

interface IDownloadSubmissionFileUrlParams {
    id: number | null;
}

interface IGetPublicSubmissionsUrlParams {
    page: number;
}

interface IGetSubmissionResultsByProblemUrlParams {
    problemId: number;
    isOfficial: boolean;
    take: number;
}

interface IGetContestResultsParams {
    id: number;
    official: boolean;
    full: boolean;
}

interface IGetSubmissionDetailsByIdUrlParams {
    submissionId: number;
    isOfficial: boolean;
    take: number;
}

interface IRetestSubmissionUrlParams {
    id: number;
}

interface IGetSearchResultsUrlParams {
    searchTerm: string;
    page: number;
    selectedTerms: [];
}

export type {
    IRegisterForContestUrlParams,
    ISubmitContestPasswordUrlParams,
    IStartContestUrlParams,
    IAllContestsUrlParams,
    IContestCategoriesUrlParams,
    IStartContestParticipationUrlParams,
    IGetContestParticipationScoresForParticipantUrlParams,
    IDownloadProblemResourceUrlParams,
    IGetPublicSubmissionsUrlParams,
    IGetSubmissionResultsByProblemUrlParams,
    IGetSubmissionDetailsByIdUrlParams,
    IGetContestResultsParams,
    IRetestSubmissionUrlParams,
    IGetSearchResultsUrlParams,
    IGetContestByProblemUrlParams,
    IDownloadSubmissionFileUrlParams,
};
