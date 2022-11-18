import React, { createContext, useCallback, useContext, useEffect, useMemo, useState } from 'react';
import isNil from 'lodash/isNil';
import sum from 'lodash/sum';

import { UrlType } from '../common/common-types';
import { IContestType, IRegisterForContestResponseType, IStartParticipationResponseType } from '../common/types';
import { IRegisterForContestUrlParams, ISubmitContestPasswordUrlParams } from '../common/url-types';
import { IHaveChildrenProps } from '../components/common/Props';

import { useHttp } from './use-http';
import { useLoading } from './use-loading';
import { useUrls } from './use-urls';

interface IStartContestArgs {
    id: number;
    isOfficial: boolean;
}

interface ISubmitContestPasswordArgs {
    id: number;
    isOfficial: boolean;
    password: string;
}

interface ICurrentContestContext {
    state: {
        contest: IContestType | null;
        contestPassword: string | null;
        score: number;
        maxScore: number;
        isOfficial: boolean;
        requirePassword: boolean | null;
        submitContestPasswordErrorMessage: string | null;
        isPasswordValid: boolean | null;
        remainingTimeInMilliseconds: number;
        totalParticipantsCount: number;
        activeParticipantsCount: number;
    };
    actions: {
        setContestPassword: (password: string) => void;
        start: (info: IStartContestArgs) => void;
        register: (info: IStartContestArgs) => void;
        submitPassword: (info: ISubmitContestPasswordArgs) => void;
    };
}

const defaultState = {
    state: {
        contest: null,
        contestPassword: null,
        score: 0,
        maxScore: 0,
        isOfficial: false,
        requirePassword: false,
        remainingTimeInMilliseconds: 0.0,
        totalParticipantsCount: 0,
        activeParticipantsCount: 0,
    },
};

const CurrentContestsContext = createContext<ICurrentContestContext>(defaultState as ICurrentContestContext);

type ICurrentContestsProviderProps = IHaveChildrenProps

interface IContestToStartType {
    id: number;
    isOfficial: boolean;
}

const CurrentContestsProvider = ({ children }: ICurrentContestsProviderProps) => {
    const [ contest, setContest ] = useState<IContestType | null>(defaultState.state.contest);
    const [ contestPassword, setContestPassword ] = useState<string | null>(defaultState.state.contest);
    const [ score, setScore ] = useState(defaultState.state.score);
    const [ maxScore, setMaxScore ] = useState(defaultState.state.maxScore);
    const [ isOfficial, setIsOfficial ] = useState(defaultState.state.isOfficial);
    const [ requirePassword, setRequirePassword ] = useState<boolean | null>(null);
    const [ contestToStart, setContestToStart ] = useState<IContestToStartType | null>(null);
    const [ registerForContestParams, setRegisterForContestParams ] = useState<IRegisterForContestUrlParams | null>(null);
    const [ submitContestPasswordUrlParams, setSubmitContestPasswordUrlParams ] = useState<ISubmitContestPasswordUrlParams | null>(null);
    const [ submitContestPasswordErrorMessage, setSubmitContestPasswordErrorMessage ] = useState<string | null>(null);
    const [ isPasswordValid, setIsPasswordValid ] = useState<boolean | null>(null);
    const [ remainingTimeInMilliseconds, setRemainingTimeInMilliseconds ] = useState(defaultState.state.remainingTimeInMilliseconds);
    const [ totalParticipantsCount, setTotalParticipantsCount ] = useState(defaultState.state.totalParticipantsCount);
    const [ activeParticipantsCount, setActiveParticipantsCount ] = useState(defaultState.state.activeParticipantsCount);

    const {
        startLoading,
        stopLoading,
    } = useLoading();

    const {
        getStartContestParticipationUrl,
        getRegisterForContestUrl,
        getSubmitContestPasswordUrl,
    } = useUrls();

    const {
        get: startContest,
        data: startContestData,
    } = useHttp(getStartContestParticipationUrl as UrlType, contestToStart);

    const {
        get: registerForContest,
        data: registerForContestData,
    } = useHttp(getRegisterForContestUrl as UrlType, registerForContestParams);

    const {
        post: submitContestPassword,
        data: submitContestPasswordData,
        response: submitContestPasswordResponse,
    } = useHttp(getSubmitContestPasswordUrl as UrlType, submitContestPasswordUrlParams);

    const start = useCallback((obj: IContestToStartType) => {
        setContestToStart(obj);
    }, []);

    const register = useCallback((obj: IStartContestArgs) => {
        const { id, isOfficial: official } = obj;

        setRegisterForContestParams({ id, isOfficial: official } as IRegisterForContestUrlParams);
    }, []);

    const submitPassword = useCallback(({ id, isOfficial: official, password }: ISubmitContestPasswordArgs) => {
        setSubmitContestPasswordUrlParams({
            id,
            isOfficial: official,
        } as ISubmitContestPasswordUrlParams);
        setContestPassword(password);
    }, []);

    useEffect(() => {
        if (isNil(contestToStart)) {
            return;
        }

        (async () => {
            startLoading();
            await startContest();
            stopLoading();
        })();
    }, [ contestToStart, startContest, startLoading, stopLoading ]);

    useEffect(() => {
        if (isNil(startContestData)) {
            return;
        }

        const responseData = startContestData as IStartParticipationResponseType;
        const {
            contest: newContest,
            contestIsCompete,
            remainingTimeInMilliseconds: newRemainingTimeInMilliseconds,
            totalParticipantsCount: newTotalParticipants,
            activeParticipantsCount: newActiveParticipants,
        } = responseData;

        setContest(newContest);
        setIsOfficial(contestIsCompete);
        setRemainingTimeInMilliseconds(newRemainingTimeInMilliseconds);
        setTotalParticipantsCount(newTotalParticipants);
        setActiveParticipantsCount(newActiveParticipants);
    }, [ startContestData ]);

    useEffect(() => {
        if (isNil(registerForContestParams)) {
            return;
        }

        (async () => {
            startLoading();
            await registerForContest();
            stopLoading();
        })();
    }, [ registerForContest, registerForContestParams, startLoading, stopLoading ]);

    useEffect(() => {
        if (isNil(registerForContestData)) {
            return;
        }

        const responseData = registerForContestData as IRegisterForContestResponseType;
        const { requirePassword: responseRequirePassword } = responseData;

        setContest({ id: responseData.id, name: responseData.name } as IContestType);
        setRequirePassword(responseRequirePassword);
    }, [ registerForContestData ]);

    useEffect(() => {
        if (isNil(submitContestPasswordUrlParams)) {
            return;
        }

        (async () => {
            startLoading();
            await submitContestPassword({ password: contestPassword });
            stopLoading();
        })();
    }, [ contestPassword, submitContestPassword, submitContestPasswordUrlParams, startLoading, stopLoading ]);

    useEffect(() => {
        if (isNil(submitContestPasswordData)) {
            return;
        }

        // TODO: fix this https://github.com/SoftUni-Internal/exam-systems-issues/issues/224
        if (!isNil(submitContestPasswordResponse) && submitContestPasswordResponse.status !== 200) {
            setSubmitContestPasswordErrorMessage('Incorrect password');
            setIsPasswordValid(false);

            return;
        }

        setIsPasswordValid(true);
        setSubmitContestPasswordErrorMessage(null);
    }, [ registerForContestData, submitContestPasswordData, submitContestPasswordResponse ]);

    useEffect(
        () => {
            const { problems } = contest || {};

            if (isNil(problems)) {
                return;
            }

            setScore(sum(problems.map((p) => p.points)));
            setMaxScore(sum(problems.map((p) => p.maximumPoints)));
        },
        [ contest ],
    );

    const value = useMemo(
        () => ({
            state: {
                contest,
                contestPassword,
                score,
                maxScore,
                isOfficial,
                requirePassword,
                submitContestPasswordErrorMessage,
                isPasswordValid,
                remainingTimeInMilliseconds,
                totalParticipantsCount,
                activeParticipantsCount,
            },
            actions: {
                setContestPassword,
                register,
                start,
                submitPassword,
            },
        }),
        [
            contest,
            contestPassword,
            isOfficial,
            isPasswordValid,
            maxScore,
            register,
            remainingTimeInMilliseconds,
            requirePassword,
            score,
            start,
            submitContestPasswordErrorMessage,
            submitPassword,
            totalParticipantsCount,
            activeParticipantsCount,
        ],
    );

    return (
        <CurrentContestsContext.Provider value={value}>
            {children}
        </CurrentContestsContext.Provider>
    );
};

const useCurrentContest = () => useContext(CurrentContestsContext);

export default CurrentContestsProvider;

export {
    useCurrentContest,
};
