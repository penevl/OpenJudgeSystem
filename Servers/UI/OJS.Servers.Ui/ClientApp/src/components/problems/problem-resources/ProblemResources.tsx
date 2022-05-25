import * as React from 'react';
import { IProblemResourceType } from '../../../common/types';
import List from '../../guidelines/lists/List';
import ProblemResource from '../problem-resource/ProblemResource';
import styles from './ProblemResources.module.scss';

interface IProblemResourcesProps {
    resources: IProblemResourceType[] | undefined
}

const ProblemResources = ({ resources }: IProblemResourcesProps) => {
    const renderResource = (resource: IProblemResourceType) => (<ProblemResource resource={resource} />);

    return (
        resources == null
            ? <p>No additional resources.</p>
            : (
                <List
                  values={resources}
                  itemFunc={renderResource}
                  className={styles.resourcesList}
                />
            )
    );
};

export default ProblemResources;
