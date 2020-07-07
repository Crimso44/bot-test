export interface IEmployee {
    id: string;
    name: string;
    firstName: string;
    lastName: string;
    middleName: string;
    link: IEmployeeLinks;
    permission: IEmployeePermissions;
}

export interface IEmployeePermissions {
    canViewDashboard: boolean;
}

export interface IEmployeeLinks {
    profile: string;
    photo: string;
}