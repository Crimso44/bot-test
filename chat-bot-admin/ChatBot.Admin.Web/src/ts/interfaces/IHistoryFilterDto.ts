import * as moment from 'moment';
import { IDropdownItem } from "./IDropdownItem";

export interface ISortingDto
{
    id: string;
    desc: boolean;
}

export interface IFilteringDto
{
    id: string;
    value: string;
}


export interface IHistoryFilterDto {
    search?: string;
    skip?: number;
    take?: number;

    from?: moment.Moment | string;
    to?: moment.Moment | string;
    sorting?: ISortingDto[];
    filtering?: IFilteringDto[];

    isAnsweredMl?: boolean;
    isAnsweredEve?: boolean;
    isAnsweredButton?: boolean;
    isAnsweredNo?: boolean;
    isAnsweredOther?: boolean;

    isLikeYes?: boolean;
    isLikeNo?: boolean;
    isDisLike?: boolean;

    isMlYes?: boolean;
    isMlAnswer?: boolean;
    isMlWrong?: boolean;
    isMlNo?: boolean;

    categoryOriginId?: string;
}

export interface ILearningFilterDto {
    search?: string;
    skip?: number;
    take?: number;

    sorting?: ISortingDto[];
    filtering?: IFilteringDto[];

    category?: IDropdownItem;
    partitionId?: string;
    upperPartitionId?: string;
}

export interface IPatternsFilterDto {
    search?: string;
    skip?: number;
    take?: number;

    sorting?: ISortingDto[];
    filtering?: IFilteringDto[];

    category?: IDropdownItem;
    partitionId?: string;
    upperPartitionId?: string;
}

export interface IModelFilterDto {
    search?: string;
    skip?: number;
    take?: number;

    sorting?: ISortingDto[];
    filtering?: IFilteringDto[];
}

export interface IModelReportFilterDto {
    search?: string;
    skip?: number;
    take?: number;

    sorting?: ISortingDto[];
    filtering?: IFilteringDto[];

    category?: IDropdownItem[];
    partitionId?: string;
    upperPartitionId?: string;

    partitionCaption?: string;
    upperPartitionCaption?: string;
}
