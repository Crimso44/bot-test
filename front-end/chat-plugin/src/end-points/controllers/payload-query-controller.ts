import Axios, { AxiosResponse } from 'axios';
import { IAuthService } from "../../interfaces/props/IPluginProps";

export class PayloadQuery {
    uri: string;
    baseUri: string;
    authService: IAuthService;

    constructor(authService: IAuthService, baseUri: string, uri: string) {
        this.uri = uri;
        this.baseUri = baseUri;
        this.authService = authService;
    }

    execute(payload: any): Promise<AxiosResponse> {

        return new Promise<AxiosResponse>((resolve: (value: AxiosResponse) => void, reject: (reason?: any) => void): void => {
            this.authService.getToken().then(token =>
                {
                    var fullUri = `${this.baseUri ? this.baseUri + '/' : ''}${this.uri}`;

                    Axios
                        .post(
                            fullUri,
                            payload,
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