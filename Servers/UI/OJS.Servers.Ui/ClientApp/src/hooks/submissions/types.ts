﻿import { IUserProfileType } from '../use-users';

interface IProblemType {
    id: number,
    name: string,
    maximumPoints: number,
}

interface ITestRunType {
    id: number,
    maxUsedTime: number,
    maxUsedMemory: number,
    executionComment: string,
    checkerComment: string,
    resultType: string,
    expectedOutputFragment: string,
    userOutputFragment: string,
}

interface ISubmissionType {
    id: number,
    submittedOn: Date,
    problem: IProblemType,
    submissionTypeName: string,
    points: number,
    testRuns: ITestRunType[]
    maxUsedTime: number,
    maxUsedMemory: number
}

interface ITestRunDetailsType extends ITestRunType {
    isTrialTest: boolean
}

interface ISubmissionDetailsType extends ISubmissionType {
    testRuns: ITestRunDetailsType[]
    user: IUserProfileType
}

export type {
    IProblemType,
    ITestRunType,
    ISubmissionType,
    ITestRunDetailsType,
    ISubmissionDetailsType,
};