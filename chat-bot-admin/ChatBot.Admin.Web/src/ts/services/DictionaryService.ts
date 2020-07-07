import {IDictionaryDto} from "../interfaces/IDictionaryDto";
import {ISubpartDto } from '../interfaces/ISubpartDto';
import {DictionaryQuery} from "../endpoints/DictionaryQuery";
import {SubpartForPartListQuery} from "../endpoints/SubpartForPartListQuery";
import {IDropdownItem} from "@sbt/react-ui-components";
import {ISubpartDropdownItem} from "../interfaces/ISubpartDropdownItem";
import {StoreService} from "./StoreService";
import { AxiosResponse } from "axios";
import * as Scenarios from "../actions/scenarios";

class DictionaryServiceApp
{
    onPartitionQuery = (search: string,
        skip: number,
        take: number,
        callback: (items: IDropdownItem[]) => void): void => {

        let query = new DictionaryQuery(StoreService.auth, StoreService.servicesApi, "chatbotdictionary/partitions");
        query.execute(search, skip, take)
            .then((response: AxiosResponse) => {
                callback(response.data.items.map((value: IDictionaryDto) => { return { id: value.id, title: value.title } }));
            })
            .catch((error: any) => {
                Scenarios.notifyError(error);
            });
    }

    onSubpartitionQuery = (search: string,
        partId: string,
        skip: number,
        take: number,
        callback: (items: ISubpartDropdownItem[]) => void): void => {

        let query = new SubpartForPartListQuery(StoreService.auth, StoreService.servicesApi, "chatbotsubparts/collection");
        query.execute(search, partId, skip, take)
            .then((response: AxiosResponse) => {
                callback(response.data.items.map((value: ISubpartDto) => { return {
                    id: value.id,
                    title: value.title + (partId ? "" : " (" + value.parentTitle + ")"),
                    source: value
                }}));
            })
            .catch((error: any) => {
                Scenarios.notifyError(error);
            });
    }

    onChangersQuery = (
        callback: (items: IDropdownItem[]) => void): void => {

        let query = new DictionaryQuery(StoreService.auth, StoreService.servicesApi, "chatbotdictionary/changers");
        query.executeSimple()
            .then((response: AxiosResponse) => {
                callback(response.data.items.map((value: IDictionaryDto) => { return { id: value.id, title: value.title } }));
            })
            .catch((error: any) => {
                Scenarios.notifyError(error);
            });
    }

    onSourcesQuery = (
        callback: (items: IDropdownItem[]) => void): void => {

        let query = new DictionaryQuery(StoreService.auth, StoreService.servicesApi, "chatbotdictionary/sources");
        query.executeSimple()
            .then((response: AxiosResponse) => {
                callback(response.data.items.map((value: IDictionaryDto) => { return { id: value.id, title: value.title } }));
                //this._sources = response.data.items.map((value: IDictionaryDto) => { return { id: value.id, title: value.title } });
                //callback(this._sources);
            })
            .catch((error: any) => {
                Scenarios.notifyError(error);
            });
    }

}

export const DictionaryService = new DictionaryServiceApp();