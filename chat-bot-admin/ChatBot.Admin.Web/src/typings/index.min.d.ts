import * as React from "react";

export interface IAuthService {
    token: string;
    getToken(urlTemplate?: string): Promise<string>;
    init(): Promise<boolean>;
    //keycloak: any;
    tokenAlwaysNull: boolean;
}

export interface IPortalPageProps {
    authService: IAuthService;
}

export class Main extends React.Component<IPortalPageProps, any> { }