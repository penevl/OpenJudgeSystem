import IndexPage from '../pageobjects/index-page';

describe('Testing index', () => {
    let activeCards;
    let pastCards;
    before(async () => {
        await IndexPage.open();
        activeCards = await IndexPage.allCardsForActiveContests;
        pastCards = await IndexPage.allCardsForPastContests;
    });
    const contestCardsChecker = {
        cardsDisplayedCheck: (cards) => cards.length > 0,
        headersDisplayedCheck: (cards, headers) => cards.length === headers.length,
        categoriesDisplayedCheck: (cards, categories) => cards.length === categories.length,
        countersDisplayedCheck: (cards, counters) => cards.length === counters.length,

    };

    it('Expect logInButton to exist', async () => {
        const btn = await IndexPage.logInButton;
        await expect(btn).toExist();
        await expect(btn).toHaveText('LOGIN');
        await expect(btn).toHaveHrefContaining('/login');
    });

    it('Expect registerButton to exist', async () => {
        const btn = await IndexPage.registerButton;
        await expect(btn).toExist();
        await expect(btn).toHaveText('REGISTER');
        await expect(btn).toHaveHrefContaining('/register');
    });

    it('Expect navBar to have headerLogoLink with href', async () => {
        const a = await IndexPage.headerLogoLink;
        await expect(a).toHaveHrefContaining('/');
    });

    it('Expect to have imgTag with alt', async () => {
        const img = await IndexPage.headingImage;
        await expect(img).toExist();
        await expect(img).toHaveAttr('alt', 'softuni logo');
    });

    it('Expect footer to exist', async () => {
        const footer = await IndexPage.footer;
        await expect(footer).toExist();
    });

    it('Expect contests link in navigation to exist and have the correct href', async () => {
        const contestsLink = await IndexPage.navContestsLink;
        await expect(contestsLink).toExist();
        await expect(contestsLink).toHaveHrefContaining('/contests/all');
    });

    it('Expect submissions link in navigation to exist and have the correct href', async () => {
        const submissionsLink = await IndexPage.navSubmissionssLink;
        await expect(submissionsLink).toExist();
        await expect(submissionsLink).toHaveHrefContaining('/submissions');
    });

    it('Expect "See contests" button in navigation to exist and have the correct href', async () => {
        const btn = await IndexPage.seeContestsButton;
        await expect(btn).toExist();
    //  await expect(btn).toHaveHrefContaining('.....'); once the page is ready we will add it
    });

    it('Expect YouTub video to exist', async () => {
        const video = await IndexPage.youtubeVideo;
        await expect(video).toExist();
    });

    it('Expect Youtube video to have src', async () => {
        const video = await IndexPage.youtubeVideo;
        await expect(video).toHaveAttr('src');
        const src = await video.getAttribute('src');
        await expect(src).not.toBeNull();
    });

    it('Expect "See all" button in active contest section to be diplayed and redirect properly', async () => {
        const btn = await IndexPage.seeAllActiveContestsButton;
        await expect(btn).toExist();
        await expect(btn).toHaveHrefContaining('/contests'); // must be to active
        await expect(btn).toBeClickable();
    });

    it('Expect "See all" button in active contest section to exist and redirect properly', async () => {
        const btn = await IndexPage.seeAllPastContestsButton;
        await expect(btn).toExist();
        await expect(btn).toHaveHrefContaining('/contests'); // must be to past
        await expect(btn).toBeClickable();
    });

    it('Expect having at least one active contest card', async () => {
        const check = await contestCardsChecker.cardsDisplayedCheck(activeCards);
        await expect(check).toBeTruthy();
    });

    it('Expect having at least one past contest card', async () => {
        const check = await contestCardsChecker.cardsDisplayedCheck(pastCards);
        await expect(check).toBeTruthy();
    });

    it('Expect every active contest card to have header', async () => {
        const headers = await IndexPage.allActiveContestCardsHeaders;
        const check = await contestCardsChecker.headersDisplayedCheck(activeCards, headers);
        await expect(check).toBeTruthy();
    });

    it('Expect every past contest card to have header', async () => {
        const headers = await IndexPage.allCardsForPastContests;
        const check = await contestCardsChecker.headersDisplayedCheck(activeCards, headers);
        await expect(check).toBeTruthy();
    });

    it('Expect every active contest card to have category', async () => {
        const categories = await IndexPage.allActiveContestCardsCategories;
        const check = await contestCardsChecker.categoriesDisplayedCheck(activeCards, categories);
        await expect(check).toBeTruthy();
    });

    it('Expect every past contest card to have category', async () => {
        const categories = await IndexPage.allActiveContestCardsCategories;
        const check = await contestCardsChecker.categoriesDisplayedCheck(activeCards, categories);
        await expect(check).toBeTruthy();
    });
});
