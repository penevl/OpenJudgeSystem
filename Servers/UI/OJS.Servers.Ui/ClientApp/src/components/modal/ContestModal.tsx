import React, { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import Box from '@mui/material/Box';
import Modal from '@mui/material/Modal';
import Typography from '@mui/material/Typography';

import { IParticipateInContestTypeUrlParams } from '../../common/app-url-types';
import { ContestParticipationType } from '../../common/constants';
import { IContestModal } from '../../common/types';
import { useAppUrls } from '../../hooks/use-app-urls';
import { useCurrentContest } from '../../hooks/use-current-contest';
import Button, { ButtonSize, ButtonType } from '../guidelines/buttons/Button';

import styles from './ContestModal.module.scss';

interface IContestModalProps {
    contest: IContestModal;
    isShowing: boolean;
    toggle: () => void;
}

const ContestModal = ({ contest, isShowing, toggle }: IContestModalProps) => {
    const { getParticipateInContestUrl } = useAppUrls();
    const navigate = useNavigate();
    const { actions: { setIsUserParticipant } } = useCurrentContest();
    const { getHomePageUrl } = useAppUrls();

    const startContestAndHideModal = useCallback(
        () => {
            setIsUserParticipant(true);

            navigate(getParticipateInContestUrl({
                id: contest.id,
                participationType: ContestParticipationType.Compete,
                problemIndex: 1,
            } as IParticipateInContestTypeUrlParams));

            toggle();
        },
        [ contest, getParticipateInContestUrl, navigate, toggle, setIsUserParticipant ],
    );

    const toggleAndRedirectToHomePage = useCallback(
        () => {
            toggle();
            navigate(getHomePageUrl());
        },
        [ toggle, navigate, getHomePageUrl ],
    );

    return isShowing
        ? (
            <div>
                <Modal
                  open={isShowing}
                  sx={{ '& .MuiBackdrop-root': { backgroundColor: 'transparent' }, backdropFilter: 'blur(5px)' }}
                  onClose={() => toggleAndRedirectToHomePage()}
                  aria-labelledby="modal-modal-title"
                  aria-describedby="modal-modal-description"
                >
                    <Box className={styles.modal}>
                        <Typography id="modal-modal-title" variant="h6" className={styles.modalHeading}>
                            <p className={styles.headingText}>
                                Starting now
                                {' '}
                                <span className={styles.boldedText}>
                                    {' '}
                                    you will have
                                    {' '}
                                    {contest.duration?.toString().substring(0, 5)}
                                    {' '}
                                    hours
                                    {' '}
                                </span>
                                {' '}
                                to complete the contest
                                {' '}
                                <span className={styles.boldedText}>{contest.name}</span>
                                .
                            </p>
                        </Typography>
                        <Typography id="modal-modal-description">
                            <span className={styles.bodyText}>
                                Your time will start counting down when you press the &quot;Compete&quot; button.
                                <br />
                                <br />
                                In the case of unexpected problems (turning off your computer, exiting the page/system,
                                internet connection failure),
                                {' '}
                                <span className={styles.boldedText}>the time lost will not be restored</span>
                                . When time runs out,
                                you
                                {' '}
                                <span className={styles.boldedText}>will not</span>
                                {' '}
                                be able to compete in this competition again.
                                <br />
                                <br />
                                When you click the &quot;Compete&quot; button,
                                {' '}
                                <span className={styles.boldedText}>
                                    {contest.numberOfProblems}
                                    {' '}
                                    random problems, one of each type, will be generated
                                </span>
                                {' '}
                                for you.
                                <br />
                                <br />
                                <span className={styles.questionText}>Are you sure you want to start the contest now?</span>
                            </span>
                        </Typography>
                        <div className={styles.horizontalLine} />
                        <span className={styles.buttons}>
                            <Button
                              id="button-card-compete"
                              onClick={() => startContestAndHideModal()}
                              text="Compete"
                              size={ButtonSize.large}
                            />
                            <Button
                              onClick={() => toggleAndRedirectToHomePage()}
                              size={ButtonSize.large}
                              type={ButtonType.secondary}
                            >
                                Cancel
                            </Button>
                        </span>
                    </Box>
                </Modal>
            </div>
        )
        : null;
};

export default ContestModal;