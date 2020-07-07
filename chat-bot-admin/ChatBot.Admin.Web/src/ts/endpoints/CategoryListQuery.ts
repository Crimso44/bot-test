import Axios, { AxiosResponse } from 'axios';
import { IAuthService } from "../interfaces/IPortalPage";
import { ICategoryFilter } from '../interfaces/IPortalState';

export class CategoryListQuery {
    uri: string;
    baseUri: string;
    authService: IAuthService;

    constructor(authService: IAuthService, baseUri: string, uri: string) {
        this.uri = uri;
        this.baseUri = baseUri;
        this.authService = authService;
    }

    execute(skip: number, take: number, filter: ICategoryFilter): Promise<AxiosResponse> {

        return new Promise<AxiosResponse>((resolve: (value: AxiosResponse) => void, reject: (reason?: any) => void): void => {
            this.authService.getToken().then(token =>
                {
                    var fullUri = `${this.baseUri ? this.baseUri + '/' : ''}${this.uri}`;

                    Axios
                        .post(
                            fullUri,
                            { 
                                search: filter.filterCategory, skip:skip, take: take,
                                pattern: filter.filterPattern, answer: filter.filterResponse, context: filter.filterContext, 
                                partitionId: filter.partitionId, subPartitionId: filter.subPartId, changedBy: filter.filterChangedBy,
                                sortColumn: filter.sorting, sortDescent: filter.sortDescent, isDisabled: filter.isDisabled
                            },
                            { withCredentials: true })
                        .then(
                            val => resolve(val),
                            err => reject(err));
                });
        });
    }


    executeStat(filter: ICategoryFilter): Promise<AxiosResponse> {

        return new Promise<AxiosResponse>((resolve: (value: AxiosResponse) => void, reject: (reason?: any) => void): void => {
            this.authService.getToken().then(token =>
                {
                    var fullUri = `${this.baseUri ? this.baseUri + '/' : ''}${this.uri}`;

                    Axios
                        .post(
                            fullUri,
                            { 
                                partitionId: filter.partitionId, subPartitionId: filter.subPartId
                            },
                            { withCredentials: true })
                        .then(
                            val => resolve(val),
                            err => reject(err));
                });
        });
    }
}

