import React, { FC } from 'react';
import { Route, Routes } from 'react-router-dom';

import AdministrationPage from '../../pages/administration/AdministrationPage';
import SubmissionRetestPage from '../../pages/administration/SubmissionRetestPage';
import ContestPage from '../../pages/contest/ContestPage';
import ContestResultsPage from '../../pages/contest-results/ContestResultsPage';
import ContestsPage from '../../pages/contests/ContestsPage';
import HomePage from '../../pages/home/HomePage';
import LoginPage from '../../pages/login/LoginPage';
import LogoutPage from '../../pages/logout/LogoutPage';
import ProfilePage from '../../pages/profile/ProfilePage';
import RegisterPage from '../../pages/register/RegisterPage';
import SearchPage from '../../pages/search/SearchPage';
import { asPage } from '../../pages/shared/set-page-params';
import { withTitle } from '../../pages/shared/set-page-title';
import SubmissionDetailsPage from '../../pages/submission-details/SubmissionDetailsPage';
import SubmissionsPage from '../../pages/submissions/SubmissionsPage';

import styles from './PageContent.module.scss';

const routes = [
    {
        path: '/login',
        Element: LoginPage,
        title: 'Login',
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
        title: 'My Profile',
    },
    {
        path: '/submissions',
        Element: SubmissionsPage,
        title: 'Submissions',
    },
    {
        path: '/submissions/:submissionId/details',
        Element: SubmissionDetailsPage,
    },
    {
        title: 'Contests',
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
    {
        path: '/Submissions/Retest',
        Element: SubmissionRetestPage,
    },
    {
        path: '/search',
        Element: SearchPage,
    },
];

const PageContent = () => {
    const renderRoute = (path: string, Element: FC, title: string | undefined) => {
        const WrappedElement = asPage(withTitle(Element, title));
        return (
            <Route key={path} path={path} element={<WrappedElement />} />
        );
    };

    return (
        <main className={styles.main}>
            <Routes>
                {routes.map(({ path, Element, title }) => renderRoute(path, Element, title))}
            </Routes>
        </main>
    );
};

export default PageContent;
