import { useParams } from 'react-router';
import isNil from 'lodash/isNil';

import { RedirectExternal } from '../../components/common/RedirectExternal';
import { getAdministrationProblems } from '../../utils/urls';

const ContestProblemsPage = () => {
    const params = useParams();
    const { contestId } = params;
    if (isNil(contestId)) {
        return null;
    }

    return RedirectExternal(getAdministrationProblems({ id: Number(contestId) }));
};

export default ContestProblemsPage;
