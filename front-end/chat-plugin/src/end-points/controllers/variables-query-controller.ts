import Axios, { AxiosResponse } from 'axios';
import { IAuthService } from "../../interfaces/props/IPluginProps";

export class VariablesQuery<V, R> {
    uri: string;
    query: string;
    variables: V;
    baseUri: string;
    authService: IAuthService;

    constructor(authService: IAuthService, baseUri: string, uri: string, query?: string, variables?: V) {
        this.uri = uri;
        this.baseUri = baseUri;
        this.query = query;
        this.variables = variables;
        this.authService = authService;
    }

    execute(query?, variables?): Promise<AxiosResponse> {

        return new Promise<AxiosResponse>((resolve: (value: AxiosResponse) => void, reject: (reason?: any) => void): void => {
            this.authService.getToken().then(token =>
                {
                    var fullUri = `${this.baseUri ? this.baseUri + '/' : ''}${this.uri}`;

                    Axios
                        .post(
                            fullUri,
                            {
                                query: this.query || query || null,
                                variables: this.variables || variables || null
                            },
                            {
                                withCredentials: true
                            })
                        .then(
                            val => resolve(val),
                            err => reject(err));
                });
        });
    }
}