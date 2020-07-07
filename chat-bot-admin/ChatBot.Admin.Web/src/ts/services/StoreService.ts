import * as Redux from 'redux';
import { IModalProperties, IModalService, IAuthService } from "../interfaces/IPortalPage";
import { INotificationService } from './notification-service'

class $StoreService {
    auth: IAuthService = null;
    store: Redux.Store<any> = null;
    history: any = null;
    baseUrl: string = '';
    ms: <E = any>(props: IModalProperties<E>) => IModalService<E> = null;
    dispatch: any = null;
    servicesApi: string = null;
    isDevelopment: boolean = false;
    notification: INotificationService;
}

export const StoreService = new $StoreService();