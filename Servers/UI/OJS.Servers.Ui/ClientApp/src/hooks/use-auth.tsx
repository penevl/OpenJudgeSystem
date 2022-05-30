import React, { createContext, useCallback, useContext, useEffect, useState } from 'react';
import { IHaveChildrenProps } from '../components/common/Props';
import { useLoading } from './use-loading';
import { useHttp } from './use-http';
import { useNotifications } from './use-notifications';
import { useUrls } from './use-urls';
import { getCookie } from '../utils/cookies';
import { HttpStatus } from '../common/common';
import { INotificationType } from '../common/common-types';

type UserType = {
    username: string,
    permissions: IUserPermissionsType,
    isLoggedIn?: boolean | null,
};

interface IUserPermissionsType {
    canAccessAdministration: boolean,
}

interface IAuthContext {
    user: UserType,
    signIn: () => void;
    signOut: () => Promise<void>;
    getUser: () => UserType;
    setUsername: (value: string) => void;
    setPassword: (value: string) => void;
}

const defaultState = {
    user: {
        username: '',
        isLoggedIn: null,
        permissions: { canAccessAdministration: false } as IUserPermissionsType,
    },
};

const AuthContext = createContext<IAuthContext>(defaultState as IAuthContext);

interface IAuthProviderProps extends IHaveChildrenProps {
}

const AuthProvider = ({ children }: IAuthProviderProps) => {
    const { startLoading, stopLoading } = useLoading();
    const [ user, setUser ] = useState<UserType>(defaultState.user);
    const [ username, setUsername ] = useState<string>(defaultState.user.username);
    const [ password, setPassword ] = useState<string>();
    const { showError } = useNotifications();

    const { getLogoutUrl, getLoginSubmitUrl } = useUrls();

    const {
        post: loginSubmit,
        response: loginSubmitResponse,
        status: loginSubmitStatus,
    } = useHttp(getLoginSubmitUrl);

    const { post: logout, response: logoutResponse } = useHttp(getLogoutUrl);

    const signIn = useCallback(
        async () => {
            startLoading();
            await loginSubmit({
                Username: username,
                Password: password,
                RememberMe: true,
            });
            stopLoading();
        },
        [ loginSubmit, password, startLoading, stopLoading, username ],
    );

    const signOut = useCallback(async () => {
        startLoading();
        await logout({});
        setUser(defaultState.user);
        stopLoading();
    }, [ logout, startLoading, stopLoading ]);

    const getUser = useCallback(() => user, [ user ]);

    const tryGetUserDetailsFromCookie = useCallback(() => {
        const loggedInUsername = getCookie('logged_in_username');
        const canAccessAdministrationCookie = getCookie('can_access_administration');
        let { permissions } = defaultState.user;
        let loggedIn = false;

        if (loggedInUsername) {
            const canAccessAdministration = canAccessAdministrationCookie.length > 0;
            permissions = { canAccessAdministration } as IUserPermissionsType;
            loggedIn = true;
        }

        return {
            username: loggedInUsername,
            isLoggedIn: loggedIn,
            permissions,
        };
    }, []);

    const setUserDetails = useCallback((userDetails: UserType | null) => {
        if (userDetails == null) {
            return;
        }

        setUser(userDetails);
    }, []);

    useEffect(() => {
        const loadedUser = tryGetUserDetailsFromCookie();
        setUserDetails(loadedUser);
    }, [ setUserDetails, tryGetUserDetailsFromCookie ]);

    useEffect(() => {
        if (loginSubmitResponse) {
            if (loginSubmitStatus === HttpStatus.Unauthorized) {
                showError({ message: 'Invalid credentials.' } as INotificationType);
            }

            const loadedUser = tryGetUserDetailsFromCookie();
            setUserDetails(loadedUser);
        }
    }, [ loginSubmitResponse, loginSubmitStatus, showError, setUserDetails, tryGetUserDetailsFromCookie ]);

    useEffect(() => {
        if (logoutResponse) {
            setUser(defaultState.user);
        }
    }, [ logoutResponse ]);

    const value = {
        user,
        signIn,
        signOut,
        getUser,
        setUsername,
        setPassword,
    };

    return (
        <AuthContext.Provider value={value}>
            {children}
        </AuthContext.Provider>
    );
};

const useAuth = () => useContext(AuthContext);

export {
    useAuth,
};

export default AuthProvider;
