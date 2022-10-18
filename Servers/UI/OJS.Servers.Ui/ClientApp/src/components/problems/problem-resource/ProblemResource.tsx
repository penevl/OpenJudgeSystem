import * as React from 'react';

import { useCallback } from 'react';

import { Button, ButtonType } from '../../guidelines/buttons/Button';
import { IProblemResourceType } from '../../../common/types';

import { useProblems } from '../../../hooks/use-problems';

import styles from './ProblemResource.module.scss';

interface IProblemResourceProps {
    resource: IProblemResourceType
}

// TODO: These should be extracted into `Icons`
const resourceTypeToIconClassName : { [name: number]: string } = {
    1: 'fa-file-alt',
    2: 'fa-lightbulb',
    3: 'fa-link',
};

const ProblemResource = ({ resource }: IProblemResourceProps) => {
    const { actions: { downloadProblemResourceFile } } = useProblems();

    const handleDownloadResourceFile = useCallback(async () => {
        await downloadProblemResourceFile(resource.id);
    }, [ downloadProblemResourceFile, resource ]);

    const renderResourceLink = useCallback((linkContent: React.ReactNode) => (resource.type === 3
        ? (
            <a
              href={resource.link}
              className={styles.resourceLink}
              target="_blank"
              rel="noreferrer"
            >
                {linkContent}
            </a>
        )
        : (
            <Button
              type={ButtonType.plain}
              className={styles.resourceLinkButton}
              onClick={() => handleDownloadResourceFile()}
            >
                {linkContent}
            </Button>
        )), [ handleDownloadResourceFile, resource ]);

    const resourceTypeIconClassName = resource.type == null
        ? resourceTypeToIconClassName[1]
        : resourceTypeToIconClassName[resource.type];

    const getResourceLinkContent = useCallback(() => (
        <>
            <i className={`fal ${resourceTypeIconClassName}`} />
            {resource.name}
        </>
    ), [ resource, resourceTypeIconClassName ]);

    return (
        <div className={styles.resourceWrapper}>
            {renderResourceLink(getResourceLinkContent())}
        </div>
    );
};

export default ProblemResource;
