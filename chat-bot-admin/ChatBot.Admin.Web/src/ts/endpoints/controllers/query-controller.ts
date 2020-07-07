import Axios, { AxiosResponse } from 'axios';
import AuthService from '../../services/auth-service';

export class Query<V, R> {

    uri: string;

    query: string;

    variables: V;

    static baseUri: string;

    constructor(uri: string, query?: string, variables?: V) {
        this.uri = uri;
        this.query = query;
        this.variables = variables;
    }

    exexute(): Promise<AxiosResponse> {
        return AuthService.getToken().then(token => {
            var fullUri = `${Query.baseUri ? Query.baseUri + '/' : ''}${this.uri}`;
            /*var headers = {
                Authorization: `Bearer ${token}`
            };*/
            return Axios.post(
                fullUri,
                {
                    query: this.query,
                    variables: this.variables || null
                },
                {
                    //headers: headers
                }
            )
                .then(
                val => new Promise((resolve) => resolve(val.data.data)),
                err => new Promise((resolve, reject) => reject(err)));
        });
    }

    setQueryString(query: string) {
        this.query = query;
    }

    setQueryVariables(variables: V) {
        this.variables = variables;
    }
}