import { Subject } from "rxjs";
import { INotificationService } from '../services/notification-service'

export interface IAuthService {
    token: string;
    getToken(urlTemplate?: string): Promise<string>;
    init(): Promise<boolean>;
}

export interface INotification {
    id: string;
    text: string;
    isRead: boolean;
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


export interface IPortalPageProps {
    stream?: Subject<any>;
    auth?: IAuthService;
    modals?: <E = any>(props: IModalProperties<E>) => IModalService<E>;
    notification?: INotificationService;
    onCreate?: (e: Element) => void;
    history?: History;
}

export interface IAppPageProps extends IPortalPageProps {
    store?: any
    baseUrl?: string
    dispatch: any
    servicesApi: string
    state: any,
    employee: any,
    employeeTimesheet: any,
    match?: any
}