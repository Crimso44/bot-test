import {IChatBotPermissionDto} from "../interfaces/IChatBotPermissionDto";
import * as Const from "../const/constants";
import {DataQuery} from "../endpoints/DataQuery";
import { AxiosResponse } from "axios";
import {StoreService} from "./StoreService";

class PermissionServiceApp
{
    public static Permissions: IChatBotPermissionDto = {
        canReadChatBot: false,
        canEditChatBot: false
    };

    static getPermissions = (): Promise<IChatBotPermissionDto> => {

        return (new Promise<IChatBotPermissionDto>((resolve: (value: IChatBotPermissionDto) => void, reject: (reason?: any) => void): void => {

            let query = new DataQuery(StoreService.auth, StoreService.servicesApi, "chatbotuser/permissions");
            query.execute()
                .then((response: AxiosResponse) => {
                    resolve(response.data as IChatBotPermissionDto);
                })
                .catch((error: any) => {
                    reject(error);
                });
        }));
    }

    public static Init = () => {
        PermissionServiceApp.getPermissions().then((permissions: IChatBotPermissionDto) => {
            PermissionServiceApp.Permissions = permissions;
        });
    }
}

export const PermissionService = PermissionServiceApp;