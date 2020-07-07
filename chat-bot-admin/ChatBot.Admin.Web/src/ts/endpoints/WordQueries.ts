import Axios, { AxiosResponse } from 'axios';
import { IAuthService } from "../interfaces/IPortalPage";
import { IWordDto } from "../interfaces/IWordDto";
import { IPatternDto } from '../interfaces/IPatternDto';

export class FillWordFormsQuery {
    uri: string;
    baseUri: string;
    authService: IAuthService;

    constructor(authService: IAuthService, baseUri: string, uri: string) {
        this.uri = uri;
        this.baseUri = baseUri;
        this.authService = authService;
    }

    execute(word: IWordDto): Promise<AxiosResponse> {

        return new Promise<AxiosResponse>((resolve: (value: AxiosResponse) => void, reject: (reason?: any) => void): void => {
            this.authService.getToken().then(token =>
                {
                    var fullUri = `${this.baseUri ? this.baseUri + '/' : ''}${this.uri}`;

                    Axios
                        .post(
                            fullUri,
                            word,
                            { withCredentials: true })
                        .then(
                            val => resolve(val),
                            err => reject(err));
                });
        });
    }
}

export class PatternCalculateQuery {
    uri: string;
    baseUri: string;
    authService: IAuthService;

    constructor(authService: IAuthService, baseUri: string, uri: string) {
        this.uri = uri;
        this.baseUri = baseUri;
        this.authService = authService;
    }

    execute(pattern: IPatternDto): Promise<AxiosResponse> {

        return new Promise<AxiosResponse>((resolve: (value: AxiosResponse) => void, reject: (reason?: any) => void): void => {
            this.authService.getToken().then(token =>
                {
                    var fullUri = `${this.baseUri ? this.baseUri + '/' : ''}${this.uri}`;

                    Axios
                        .post(
                            fullUri,
                            pattern,
                            { withCredentials: true })
                        .then(
                            val => resolve(val),
                            err => reject(err));
                });
        });
    }
}

export class GetPatternQuery {
    uri: string;
    baseUri: string;
    authService: IAuthService;

    constructor(authService: IAuthService, baseUri: string, uri: string) {
        this.uri = uri;
        this.baseUri = baseUri;
        this.authService = authService;
    }

    execute(patternId: number): Promise<AxiosResponse> {

        return new Promise<AxiosResponse>((resolve: (value: AxiosResponse) => void, reject: (reason?: any) => void): void => {
            this.authService.getToken().then(token =>
                {
                    var fullUri = `${this.baseUri ? this.baseUri + '/' : ''}${this.uri}/${patternId}`;

                    Axios
                        .get(
                            fullUri,
                            { withCredentials: true })
                        .then(
                            val => resolve(val),
                            err => reject(err));
                });
        });
    }
}

