import * as Config from "../config/config";
import { IAuthService, IRosterDto, ISelectItem } from "../interfaces/props/IPluginProps";
import { EmployeeListQuery } from "../end-points/controllers/employee-list-query-controller";

class EmployeeFindServiceImpl
{
    authService: IAuthService;

    find = (query: string, skip: number, take: number, source: string): Promise<any> => {

        return new Promise<any>((resolve: (items?: ISelectItem[]) => void, reject: (reason?: any) => void): void =>
        {
            let askQuery = new EmployeeListQuery(this.authService, Config.EmployeeWebApiBase, "find");

            askQuery.execute(query, skip, take, source).then(
                response => {
                    let data = response.data as IRosterDto[];
                    let items: ISelectItem[]  = data.map(it => { return { id: it.code, title: it.name, source: it.source } });
                    resolve(items);
                },
                error => {
                    reject(error);
                }
            );
        });
    }
}

export const EmployeeFindService = new EmployeeFindServiceImpl();