import Axios, { AxiosResponse } from 'axios';
import { IAuthService } from "../interfaces/IPortalPage";

export class RawPutQuery {
    uri: string;
    baseUri: string;
    authService: IAuthService;

    constructor(authService: IAuthService, baseUri: string, uri: string) {
        this.uri = uri;
        this.baseUri = baseUri;
        this.authService = authService;
    }

    execute(postData: any): Promise<AxiosResponse> {

        return new Promise<AxiosResponse>((resolve: (value: AxiosResponse) => void, reject: (reason?: any) => void): void => {
            this.authService.getToken().then(token =>
                {
                    var fullUri = `${this.baseUri ? this.baseUri + '/' : ''}${this.uri}`;

                    Axios
                        .put(
                            fullUri,
                            postData,
                            { withCredentials: true })
                        .then(
                            val => resolve(val),
                            err => reject(err));
                });
        });
    }
}

