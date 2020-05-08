import jsonServerProvider from 'ra-data-json-server';
import { fetchUtils } from 'react-admin';
import { stringify } from 'query-string';

const apiUrl = 'http://localhost:5000/api';
// const httpClient = fetchUtils.fetchJson;

const httpClient = (url, options = {}) => {
    if (!options.headers) {
        options.headers = new Headers({ Accept: 'application/json' });
    }
    const token = localStorage.getItem('token');
    options.headers.set('Authorization', `Bearer ${token}`);
    return fetchUtils.fetchJson(url, options);
};

const dataProvider = jsonServerProvider(apiUrl, httpClient);

const myDataProvider = {
    ...dataProvider,
    getMany: (resource, params) => {
        const query = params.ids
            .map(id => `id=${id}`)
            .join('&');
        const url = `${apiUrl}/${resource}/many?${query}`;
        return httpClient(url).then(({ json }) => ({ data: json }));
    },
};

export default myDataProvider;
