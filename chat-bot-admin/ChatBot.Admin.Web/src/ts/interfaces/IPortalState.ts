import { ICategoryDto } from "./ICategoryDto";
import { IPartitionDto } from "./IPartitionDto";
import { ISubpartDto } from "./ISubpartDto";
import { IHistoryDto } from "./IHistoryDto";
import { IHistoryFilterDto, ILearningFilterDto, IPatternsFilterDto, IModelFilterDto } from "../interfaces/IHistoryFilterDto"
import * as moment from 'moment';
import { ILearningDto } from "./ILearningDto";
import { IPatternsDto } from "./IPatternsDto";
import { IModelDto } from "./IModelDto";

export interface IPortalState extends IAppState {
    state?: any;
    router?: any;
}

export interface IAppState{
    categoryList: ICategoryListState;
    categoryItem: ICategoryItemState;
    partitionList: IPartitionListState;
    partitionItem: IPartitionItemState;
    subPartList: ISubPartListState;
    subPartItem: ISubPartItemState;
    settings: ISettingsState;
    history: IHistoryState;
    learning: ILearningState;
    patterns: IPatternsState;
    model: IModelState;
}

export interface ICategoryFilter {
    partitionId?: string;
    partitionCaption?: string;
    subPartId?: string;
    subPartCaption?: string;
    filterCategory?: string;
    filterPattern?: string;
    filterResponse?: string;
    filterContext?: string;
    filterChangedBy?: string;
    filterChangedByName?: string;
    sorting?: string;
    sortingName?: string;
    sortDescent?: boolean;
    isDisabled?: boolean;
}

export interface ICategoryListState extends ICategoryFilter {
    items?: ICategoryDto[];
    currentPageNumber?: number;
    totalItemsCount?: number;
    isLoading: boolean;
    loadingError?: string;
}

export interface ICategoryItemState {
    versionVirtId: string;
    item?: ICategoryDto;
    isLoading: boolean;
    isAdding: boolean;
    loadingError?: string;
    partitionId?: string;
    partitionCaption?: string;
    subPartId?: string;
    subPartCaption?: string;
    learnings: ILearningDto[];
    pages: number;
}

export interface IPartitionListState {
    items?: IPartitionDto[];
    subitems?: ISubpartDto[];
    isLoading: boolean;
    loadingError?: string;
    selectedPartId?: string;
    selectedPartCaption?: string;
}

export interface ISettingsState {
    useModel: boolean;
    useMLThreshold: boolean;
    useMLMultiAnswer: boolean;
    mLThreshold: string;
    mLMultiThreshold: string;
    isLoading: boolean;
    loadingError?: string;
}

export interface IHistoryState {
    history: IHistoryDto[];
    pages: number;
    isLoading: boolean;
    loadingError?: string;
    filter: IHistoryFilterDto;
    tblSorted: any;
    tblFiltered: any;
    tblPage: number;
    tblPageSize: number;
}

export interface ILearningState {
    learnings: ILearningDto[];
    pages: number;
    isLoading: boolean;
    loadingError?: string;
    filter: ILearningFilterDto;
}

export interface IPatternsState {
    patterns: IPatternsDto[];
    pages: number;
    isLoading: boolean;
    loadingError?: string;
    filter: IPatternsFilterDto;
}

export interface IModelState {
    models: IModelDto[];
    pages: number;
    isLoading: boolean;
    loadingError?: string;
    filter: IModelFilterDto;
}

export interface IPartitionItemState {
    versionVirtId: string;
    item?: IPartitionDto;
    isLoading: boolean;
    loadingError?: string;
}

export interface ISubPartListState {
    items?: ISubpartDto[];
    currentPageNumber?: number;
    totalItemsCount?: number;
    isLoading: boolean;
    loadingError?: string;
}

export interface ISubPartItemState {
    versionVirtId: string;
    item?: ISubpartDto;
    isLoading: boolean;
    loadingError?: string;
    parentPartId?: string;
    parentPartCaption?: string;
}

