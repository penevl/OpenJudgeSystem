import axios, { ResponseType } from 'axios';
import isFunction from 'lodash/isFunction';

import { IDictionary, UrlType } from '../common/common-types';

const getUrl = <P>(url: UrlType<P>, params?: IDictionary<P> | null) => (
    isFunction(url)
        ? url(params)
        : url
);

interface IHttpCallParams {
    url: string;
    method: string;
    headers?: IDictionary<string>;
    responseType: string;
    onSuccess: (response: any) => Promise<void>;
    onError: (err: any) => Promise<void>;
    body?: any;
    onBeforeCall?: () => Promise<void>;
    onAfterCall?: () => Promise<void>;
}

const makeHttpCall = async ({
    url,
    method,
    headers = {},
    body = null,
    responseType,
    onSuccess,
    onError,
    onBeforeCall = () => Promise.resolve(),
    onAfterCall = () => Promise.resolve(),
}: IHttpCallParams) => {
    const makeCall = () => axios({
        method,
        url,
        data: body,
        responseType: responseType as ResponseType,
        withCredentials: true,
        headers,
    });

    try {
        await onBeforeCall();
        const resp = await makeCall();
        await onSuccess(await resp);
    } catch (err: any) {
        await onError(err);
    } finally {
        await onAfterCall();
    }
};

export {
    getUrl,
    makeHttpCall,
};
