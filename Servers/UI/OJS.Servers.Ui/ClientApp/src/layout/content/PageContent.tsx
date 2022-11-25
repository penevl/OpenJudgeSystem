import React, { FC } from 'react';
import { Route, Routes } from 'react-router-dom';

import AdministrationPage from '../../pages/administration/AdministrationPage';
import ContestPage from '../../pages/contest/ContestPage';
import ContestRegisterPage from '../../pages/contest/ContestRegisterPage';
import ContestResultsPage from '../../pages/contest-results/ContestResultsPage';
import ContestsPage from '../../pages/contests/ContestsPage';
import HomePage from '../../pages/home/HomePage';
import LoginPage from '../../pages/login/LoginPage';
import LogoutPage from '../../pages/logout/LogoutPage';
import ProfilePage from '../../pages/profile/ProfilePage';
import RegisterPage from '../../pages/register/RegisterPage';
import { asPage } from '../../pages/shared/set-page-params';
import SubmissionDetailsPage from '../../pages/submission-details/SubmissionDetailsPage';
import SubmissionsPage from '../../pages/submissions/SubmissionsPage';

import styles from './PageContent.module.scss';

const routes = [
    {
        path: '/login',
        Element: LoginPage,
    },
    {
        path: '/register',
        Element: RegisterPage,
    },
    {
        path: '/logout',
        Element: LogoutPage,
    },
    {
        path: '/',
        Element: HomePage,
    },
    {
        path: '/profile',
        Element: ProfilePage,
    },
    {
        path: '/submissions',
        Element: SubmissionsPage,
    },
    {
        path: '/submissions/:submissionId/details',
        Element: SubmissionDetailsPage,
    },
    {
        path: '/contests/:contestId/register/:participationType',
        Element: ContestRegisterPage,
    },
    {
        path: '/contests',
        Element: ContestsPage,
    },
    {
        path: '/contests/:contestId/:participationType',
        Element: ContestPage,
    },
    {
        path: '/contests/:contestId/:participationType/results/:resultType',
        Element: ContestResultsPage,
    },
    {
        path: '/administration',
        Element: AdministrationPage,
    },
];

const PageContent = () => {
    const renderRoute = (path: string, Element: FC) => {
        const WrappedElement = asPage(Element);
        return (
            <Route key={path} path={path} element={<WrappedElement />} />
        );
    };

    return (
        <main className={styles.main}>
            <Routes>
                {routes.map(({ path, Element }) => renderRoute(path, Element))}
            </Routes>
        </main>
    );
};
export default PageContent;
