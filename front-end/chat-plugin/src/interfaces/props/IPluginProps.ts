import { Subject } from "rxjs";

export interface IAuthService {
    token: string;
    getToken(urlTemplate?: string): Promise<string>;
    init(): Promise<boolean>;
    getPortalEmployee(): Promise<IEmployee>;
}

export interface INotification {
    id: string;
    text: string;
    isRead: boolean;
}

export interface INotificationService {
    put(notificationText: string): void;
    remove(notification: INotification): void;
    markAsRead(notification: INotification): void;
    markAsUnRead(notification: INotification): void;
    getNotificationList(skip: number, take: number): INotification[];
    watch(): Subject<INotification>;
}

interface IModal<E> {
    handler: JSX.Element;
    close: Promise<any>;
    error: Promise<any>;
    setState(state: IModalState<E>): void;
    setExternal(external: E): void;
}

export interface IModalProperties<E = any> {
    content?: any;
    externalProps?: E;
    showFade?: boolean;
    onEvent?: Function;
    containerProps?: {
        className?: string;
        style?: any;
    }
    fadeProps?: {
        className?: string;
        style?: any;
    }
    modalProps?: {
        className?: string;
        style?: any;
    }
}

export interface IModalState<E = any> {
    externalProps?: E,
    style?: any,
    className?: string,
    fadeClassName?: string,
    fadeStyle?: any,
    modalClassName?: string,
    modalStyle?: any,
    showFade?: boolean
}

const defaultProperties: IModalProperties = {
    showFade: true
}

export interface IModalService<E = any> {
    open(properties?: IModalProperties<E>): IModal<E>;
    close(): void;
}

export interface IRosterDto {
    id: string;
    name: string;
    code: string;
    source: string;
}

export interface IEmployee {
    id: string;
    name: string;
    firstName: string;
    lastName: string;
    middleName: string;
}

export interface IPluginProps {
    place?: 'layout';
    stream?: Subject<any>;
    auth?: IAuthService;
    modals?: <E = any>(props: IModalProperties<E>) => IModalService<E>;
    notification?: INotificationService;
    onCreate?: (e: Element) => void;
    location?: any;
}

export interface ISelectItem {
    id: string;
    title: string;
    source: string;
    subtitle?: string;
    data?: any;
}