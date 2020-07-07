import { Subject } from "rxjs";
import { IEmployee } from "../interfaces/IEmployee";
import { Query } from "../endpoints/controllers/query-controller";

//declare const Keycloak;
export interface IAuthService {
    token: string;
    getToken(urlTemplate?: string): Promise<string>;
    init(): Promise<boolean>;
    //keycloak: any;
    tokenAlwaysNull: boolean;
}

class $AuthService implements IAuthService {
    token = null;
    //keycloak = Keycloak();
    tokenAlwaysNull: boolean;

    private employee: IEmployee;

    constructor(tokenAlwaysNull: boolean = true) {
        this.tokenAlwaysNull = tokenAlwaysNull;
    }

    init(): Promise<boolean> {
        var self = this;
        return new Promise<boolean>((resolve, reject) => {
            if (!this.token) {
                //this.keycloak
                    //.init({ 'onLoad': 'login-required' }).success((authenticated) => {
                        this.token = 'this.keycloak.token';
                        resolve(true /*authenticated*/);
                    /*})
                    .error(() => {
                        this.token = null;
                        reject(false);
                    });*/
            }
            else {
                resolve(true);
            }
        });
    }

    getToken(urlTemplate?: string): Promise<string> {
        var self = this;
        return new Promise<string>((resolve, reject) => {
            //this.keycloak.updateToken(30)
                //.success(() => {
                    this.token = 'this.keycloak.token';
                    if (!urlTemplate) {
                        resolve('this.keycloak.token');
                    }
                    else {
                        resolve(urlTemplate.replace('$$token$$', 'this.keycloak.token'));
                    }
                /*})
                .error(() => {
                    this.keycloak.login();
                });*/
        });
    }
}

const AuthService = new $AuthService();
export default AuthService;