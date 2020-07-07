import { Store } from "react-redux";
import { Subject } from "rxjs";
import * as Rx from 'rxjs';
import { IModalProperties, IModalService, ModalService } from "./modal-service";

export interface INotification {
    id: string;
    text: string;
    isRead: boolean;
    type?: string;
    dalay?: number;
}

export interface INotificationService {

    send(notificationText: string, type: string, delay: number, closeable: boolean): void
    put(notificationText: string): void;
    remove(notification: INotification): void;
    markAsRead(notification: INotification): void;
    markAsUnRead(notification: INotification): void;
    getNotificationList(skip: number, take: number): INotification[];
    watch(subscriber: (INotification) => void, filter?: (INotification) => boolean): void;
    getStream(): Rx.Subject<INotification>
}

export interface IAuthService {
    token: string;
    getToken(urlTemplate?: string): Promise<string>;
    init(): Promise<boolean>;
    getEmployee(): Promise<IEmployee>;
    getDelegate(): Promise<IEmployee>;
    getEmployeePermissionsByModule(name: string);
    //keycloak: any;
    tokenAlwaysNull: boolean;
}

export interface IEmployee {
    id: string;
    name: string;
    firstName: string;
    lastName: string;
    middleName: string;
    link: IEmployeeLinks;
    permission: IEmployeePermissions;
}

interface IPermissionObject {
    any: boolean;
    create: boolean;
    edit: boolean;
    read: boolean;
}

export interface IEmployeePermissions {
    [moduleId: number]: IPermissionObject;
}

export interface IEmployeeLinks {
    profile: string;
    photo: string;
}

export interface IMenuItem {
    id: string;
    level: number;
    countActive?: number;
    iconClassTaskTree?: string;
    text: string;
    order?: number;
    subMenu?: IMenuItem[];
    isActive?: boolean;
    isOpen?: boolean;
    internalName: string;
    parent?: IMenuItem;
    getUri?: Function;
}

export interface IWidgetStore {
    widgets: IWidget[];
}

export interface IWidget {
    id: string;
    version: string;
    type: string;
    displayName: string;
    previewImage?: string;
    discription?: string;
    supportedDimensions?: { [name: string]: { x?: number; y?: number; w: number; h: number; } }
    size?: 'small' | 'medium' | 'big';
    defaultSize?: 'small' | 'medium' | 'big';
    resizeable?: boolean;
    isInstalled?: boolean;
    color?: 'color-blue' | 'color-yellow' | 'color-green' | 'color-purple';
}

export interface IWidgetWithDimensions extends IWidget {
    dimensions: {
        x?: number;
        y?: number;
        w?: number;
        h?: number;
    }
}

export interface IState {
    employee?: IEmployee;
    delegate?: IEmployee;
    menuItems?: IMenuItem[];
    widgetsStore?: IWidgetStore;
    dashboard?: IDashboardState;
    plugins?: IPlugin[];
    pages?: IPage[];

    hideMenu?: boolean;

    hideRightBlock?: boolean;
    hideTopBlock?: boolean;
    hideLeftBlock?: boolean;
}

export interface IPlugin {
    version?: string;
    id: string;
    name: string;
    place: "layout";
    displayName: string;
    previewImage?: string;
    discription?: string;
    isInstalled?: boolean;
}

export interface IPage {
    version: string;
    id: string;
    name: string;
}

export interface IDashboardState {
    mode?: number;
    widgets?: IWidgetWithDimensions[];
}

export interface IPortalService {
    auth: IAuthService;
    stream: Subject<{ type: string, body: any }>;
    notification: INotificationService;
    store: Store<{ core: IState, [moduleName: string]: any }>;
    history: History;
    getPermissions(moduleName: string): IEmployeePermissions;
    shared: { [moduleName: string]: any };
    getShared(moduleName: string): any;
    modals<E>(props: IModalProperties<E>): IModalService<E>;
}

export const auth: IAuthService = null;
export const stream: Subject<{ type: string, body: any }> = null;
export const notification: INotificationService = null;
export const store: Store<{ core: IState, [moduleName: string]: any }> = null;
export const history: History = null;
export const getPermissions: (moduleName: string) => IEmployeePermissions = null;
export const shared: { [moduleName: string]: any } = null;
export const getShared: (moduleName: string) => any = null;
export const modals = <E>(props: IModalProperties<E>) => new ModalService<E>(props);
