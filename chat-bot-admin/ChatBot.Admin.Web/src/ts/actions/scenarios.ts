import * as Actions from "../actions/actions";
import * as Const from "../const/constants";
import { StoreService } from '../services/StoreService';
import { CategoryListQuery } from "../endpoints/CategoryListQuery";
import { ItemQuery } from "../endpoints/ItemQuery";
import { PartitionListQuery } from "../endpoints/PartitionListQuery";
import { SubpartListQuery } from "../endpoints/SubpartListQuery";
import * as GuidService from "../services/GuidService";
import { RawPostQuery } from "../endpoints/RawPostQuery";
import { DataQuery } from "../endpoints/DataQuery";
import { HistoryQuery } from "../endpoints/HistoryQuery";
import { RawPutQuery } from "../endpoints/RawPutQuery";
import { AxiosResponse } from 'axios';
import { ILoadPageResponseDto } from "../interfaces/ILoadPageResponseDto";
import { ICategoryDto } from "../interfaces/ICategoryDto";
import { IPartitionDto } from "../interfaces/IPartitionDto";
import { ISubpartDto } from "../interfaces/ISubpartDto";
import { ICategoryFilter } from '../interfaces/IPortalState';
import {IEditCategoryCommand} from "../interfaces/commands/IEditCategoryCommand";
import {IDeleteCategoryCommand} from "../interfaces/commands/IDeleteCategoryCommand";
import {ICreateCategoryCommand} from "../interfaces/commands/ICreateCategoryCommand";
import {IEditPartCommand} from "../interfaces/commands/IEditPartCommand";
import {IDeletePartCommand} from "../interfaces/commands/IDeletePartCommand";
import {ICreatePartCommand} from "../interfaces/commands/ICreatePartCommand";
import {IEditSubpartCommand} from "../interfaces/commands/IEditSubpartCommand";
import {IDeleteSubpartCommand} from "../interfaces/commands/IDeleteSubpartCommand";
import {ICreateSubpartCommand} from "../interfaces/commands/ICreateSubpartCommand";
import {IPublishCategoriesCommand} from "../interfaces/commands/IPublishCategoriesCommand";
import {IUnpublishCategoriesCommand} from "../interfaces/commands/IUnpublishCategoriesCommand";
import {ISetSettingValueCommand} from "../interfaces/commands/ISetSettingValueCommand";
import {ISetSettingsValueCommand} from "../interfaces/commands/ISetSettingsValueCommand";
import {IStoreLearningRecordCommand} from "../interfaces/commands/IStoreLearningRecordCommand"
import {IDeleteLearningRecordCommand} from "../interfaces/commands/IDeleteLearningRecordCommand"
import {IRecalcLearningTokensCommand} from "../interfaces/commands/IRecalcLearningTokensCommand";
import {IStorePatternCommand} from "../interfaces/commands/IStorePatternCommand"
import {IDeletePatternCommand} from "../interfaces/commands/IDeletePatternCommand"
import {ILearnModelCommand} from "../interfaces/commands/ILearnModelCommand";
import {IPublishModelCommand} from "../interfaces/commands/IPublishModelCommand";
import {ICopyPatternToLearnCommand} from "../interfaces/commands/ICopyPatternToLearnCommand";
import {ICommand} from "../interfaces/commands/ICommand";
import moment = require("moment");
import { IHistoryFilterDto, ILearningFilterDto, IPatternsFilterDto, IModelFilterDto } from "../interfaces/IHistoryFilterDto";


export const notifyOk = (message: string): void => {
    StoreService.notification.send(message, 'normal', 3000);
}

export const notifyError = (error: any): void => {


    console.log(error);
    let buildErrorText = (data: any): string => {

        let errorText = data.text;

        if(!data.payload || !Array.isArray(data.payload))
            return errorText;

        if (data.payload && data.payload.length > 0 && errorText == data.payload[0].message)
            errorText = "Ошибка сервиса";

        errorText  += ": " + data.payload.map(val => { return val.message; }).join("; ");

        return errorText;
    }

    let errorText = "Ошибка сервиса";

    if(error && error.response && error.response.data)
        errorText = buildErrorText(error.response.data);
    else if(error)
        errorText = error.message;

    StoreService.notification.send(errorText, 'error', 8000);
}

// Links
export const loadCategoryListPage = (pageNum? : number, filter?: ICategoryFilter): void =>
{
    StoreService.dispatch(Actions.categoryListPageLoading(filter));

    let skip = Const.CategoryListSize * pageNum || 0;
    let query = new CategoryListQuery(StoreService.auth, StoreService.servicesApi, "chatbot/collection");
    query.execute(skip, Const.CategoryListSize, filter)
        .then((response: AxiosResponse) => {
            let data : ILoadPageResponseDto<ICategoryDto> = {
                data: response.data,
                pageNumber: pageNum
            };
            StoreService.dispatch(Actions.categoryListPageLoaded(data));
        })
        .catch((error: any) => {
            notifyError(error);
            StoreService.dispatch(Actions.categoryListPageLoadingError(error));
        });
}


export const loadSettings = (): void =>
{
    StoreService.dispatch(Actions.settingsPageLoading());

    let query = new DataQuery(StoreService.auth, StoreService.servicesApi, "chatbotconfig/settings");
    query.execute()
        .then((response: AxiosResponse) => {
            StoreService.dispatch(Actions.settingsPageLoaded(response.data));
        })
        .catch((error: any) => {
            notifyError(error);
            StoreService.dispatch(Actions.settingsPageLoadingError(error));
        });
}

export const loadLearning = (state: any, originId: string): void => {
    StoreService.dispatch(Actions.learningLoading());

    let query = new HistoryQuery(StoreService.auth, StoreService.servicesApi, "chatbot/learning");
    let filter = {
        category: [{id: originId}],
        skip: state.page * state.pageSize,
        take: state.pageSize,
        sorting: state.sorted,
        filtering: state.filtered
    }
    query.execute(filter)
        .then((response: AxiosResponse) => {
            response.data.pageSize = state.pageSize;
            StoreService.dispatch(Actions.learningLoaded(response.data));
        })
        .catch((error: any) => {
            notifyError(error);
            StoreService.dispatch(Actions.learningLoadingError(error));
        });
}

export const loadHistory = (state: any, filter: IHistoryFilterDto): void =>
{
    StoreService.dispatch(Actions.historyPageLoading());

    let query = new HistoryQuery(StoreService.auth, StoreService.servicesApi, "chatbothistory/list");
    filter.skip = state.page * state.pageSize;
    filter.take = state.pageSize;
    filter.sorting = state.sorted;
    filter.filtering = state.filtered;
    query.execute(filter)
        .then((response: AxiosResponse) => {
            response.data.pageSize = state.pageSize;
            StoreService.dispatch(Actions.historyPageLoaded(response.data));
        })
        .catch((error: any) => {
            notifyError(error);
            StoreService.dispatch(Actions.historyPageLoadingError(error));
        });
}

export const loadLearnings = (state: any, filter: ILearningFilterDto): void =>
{
    StoreService.dispatch(Actions.learningPageLoading());

    let query = new HistoryQuery(StoreService.auth, StoreService.servicesApi, "chatbot/learning");
    filter.skip = state.page * state.pageSize;
    filter.take = state.pageSize;
    filter.sorting = state.sorted;
    filter.filtering = state.filtered;

    query.execute(filter)
        .then((response: AxiosResponse) => {
            response.data.pageSize = state.pageSize;
            StoreService.dispatch(Actions.learningPageLoaded(response.data));
        })
        .catch((error: any) => {
            notifyError(error);
            StoreService.dispatch(Actions.learningPageLoadingError(error));
        });
}

export const loadPatternss = (state: any, filter: IPatternsFilterDto): void =>
{
    StoreService.dispatch(Actions.patternsPageLoading());

    let query = new HistoryQuery(StoreService.auth, StoreService.servicesApi, "chatbot/patterns");
    filter.skip = state.page * state.pageSize;
    filter.take = state.pageSize;
    filter.sorting = state.sorted;
    filter.filtering = state.filtered;

    query.execute(filter)
        .then((response: AxiosResponse) => {
            response.data.pageSize = state.pageSize;
            StoreService.dispatch(Actions.patternsPageLoaded(response.data));
        })
        .catch((error: any) => {
            notifyError(error);
            StoreService.dispatch(Actions.patternsPageLoadingError(error));
        });
}

export const loadModels = (state: any, filter: IModelFilterDto): void =>
{
    StoreService.dispatch(Actions.modelPageLoading());

    let query = new HistoryQuery(StoreService.auth, StoreService.servicesApi, "chatbot/model");
    filter.skip = state.page * state.pageSize;
    filter.take = state.pageSize;
    filter.sorting = state.sorted;
    filter.filtering = state.filtered;

    query.execute(filter)
        .then((response: AxiosResponse) => {
            response.data.pageSize = state.pageSize;
            StoreService.dispatch(Actions.modelPageLoaded(response.data));
        })
        .catch((error: any) => {
            notifyError(error);
            StoreService.dispatch(Actions.modelPageLoadingError(error));
        });
}

export const filterChanged = (filter: IHistoryFilterDto): void => {
    StoreService.dispatch(Actions.historyFilterChanged({...filter}));
}

export const filterLearningChanged = (filter: ILearningFilterDto): void => {
    StoreService.dispatch(Actions.learningFilterChanged({...filter}));
}

export const filterPatternsChanged = (filter: IPatternsFilterDto): void => {
    StoreService.dispatch(Actions.patternsFilterChanged({...filter}));
}

export const updateSettings = (val: any): void => {
    StoreService.dispatch(Actions.settingsPageUpdate(val));
}

export const applySettings = (val: any): void => {
        StoreService.dispatch(Actions.settingsPageLoading());
    
        let payload: ISetSettingsValueCommand = {settings: [
            { name: 'UseModel', value: val.useModel },
            { name: 'UseMLThreshold', value: val.useMLThreshold },
            { name: 'UseMLMultiAnswer', value: val.useMLMultiAnswer },
            { name: 'MLThreshold', value: val.mLThreshold },
            { name: 'MLMultiThreshold', value: val.mLMultiThreshold },
        ]};

        let command: ICommand = {
            id: GuidService.getNewGUIDString(),
            name: "SetSettingsValue",
            payload: payload
        };
    
        let query = new RawPostQuery(StoreService.auth, StoreService.servicesApi, "commands");
        query.execute(command)
            .then((response: AxiosResponse) => {
                notifyOk(response.data.text);
            })
            .catch((error: any) => {
                notifyError(error);
            });
}




export const endEditCategoryItem = (): void =>
{
    StoreService.dispatch(Actions.categoryItemEndEdit());
    StoreService.history.push(Const.NavigationPathCategoryList);
}

export const saveEditedCategoryItem = (commandId: string, payload: IEditCategoryCommand): void => {
    let command: ICommand = {
        id: commandId,
        name: "EditChatBotCategory",
        payload: payload
    };

    let query = new RawPostQuery(StoreService.auth, StoreService.servicesApi, "commands");
    query.execute(command)
        .then((response: AxiosResponse) => {
            StoreService.dispatch(Actions.categoryItemLoading({isAdding:false}));
            notifyOk(response.data.text);
            getCategoryItem(payload.id.toString());
        })
        .catch((error: any) => {
            notifyError(error);
        });
}

export const storeLearningItem = (payload: IStoreLearningRecordCommand, state: any, originId: string): void => {
    let command: ICommand = {
        id: GuidService.getNewGUIDString(),
        name: "StoreLearningRecord",
        payload: payload
    };

    let query = new RawPostQuery(StoreService.auth, StoreService.servicesApi, "commands");
    query.execute(command)
        .then((response: AxiosResponse) => {
            notifyOk(response.data.text);
            loadLearning(state, originId);
        })
        .catch((error: any) => {
            notifyError(error);
        });
}

export const storeLearningItemFromHistory = (payload: IStoreLearningRecordCommand, state: any, filter: IHistoryFilterDto): void => {
    let command: ICommand = {
        id: GuidService.getNewGUIDString(),
        name: "StoreLearningRecord",
        payload: payload
    };

    let query = new RawPostQuery(StoreService.auth, StoreService.servicesApi, "commands");
    query.execute(command)
        .then((response: AxiosResponse) => {
            notifyOk(response.data.text);
            loadHistory(state, filter);
        })
        .catch((error: any) => {
            notifyError(error);
        });
}


export const storeLearningItemFromLearnings = (payload: IStoreLearningRecordCommand, state: any, filter: ILearningFilterDto): void => {
    let command: ICommand = {
        id: GuidService.getNewGUIDString(),
        name: "StoreLearningRecord",
        payload: payload
    };

    let query = new RawPostQuery(StoreService.auth, StoreService.servicesApi, "commands");
    query.execute(command)
        .then((response: AxiosResponse) => {
            notifyOk(response.data.text);
            loadLearnings(state, filter);
        })
        .catch((error: any) => {
            notifyError(error);
        });
}

export const storePatternsItemFromPatterns = (payload: IStorePatternCommand, state: any, filter: IPatternsFilterDto): void => {
    let command: ICommand = {
        id: GuidService.getNewGUIDString(),
        name: "StorePattern",
        payload: payload
    };

    let query = new RawPostQuery(StoreService.auth, StoreService.servicesApi, "commands");
    query.execute(command)
        .then((response: AxiosResponse) => {
            notifyOk(response.data.text);
            loadPatternss(state, filter);
        })
        .catch((error: any) => {
            notifyError(error);
        });
}

export const deleteLearningItem = (payload: IDeleteLearningRecordCommand, state: any, originId: string): void => {
    let command: ICommand = {
        id: GuidService.getNewGUIDString(),
        name: "DeleteLearningRecord",
        payload: payload
    };

    let query = new RawPostQuery(StoreService.auth, StoreService.servicesApi, "commands");
    query.execute(command)
        .then((response: AxiosResponse) => {
            notifyOk(response.data.text);
            loadLearning(state, originId);
        })
        .catch((error: any) => {
            notifyError(error);
        });
}

export const doSpellCheck = (payload: IRecalcLearningTokensCommand): void => {
    let command: ICommand = {
        id: GuidService.getNewGUIDString(),
        name: "RecalcLearningTokens",
        payload: payload
    };

    let query = new RawPostQuery(StoreService.auth, StoreService.servicesApi, "commands");
    query.execute(command)
        .then((response: AxiosResponse) => {
            notifyOk(response.data.text);
        })
        .catch((error: any) => {
            notifyError(error);
        });
}

export const doLearnModel = (payload: ILearnModelCommand, state: any, filter: IModelFilterDto): void => {
    let command: ICommand = {
        id: GuidService.getNewGUIDString(),
        name: "LearnModel",
        payload: payload
    };

    let query = new RawPostQuery(StoreService.auth, StoreService.servicesApi, "commands");
    query.execute(command)
        .then((response: AxiosResponse) => {
            notifyOk(response.data.text);
            loadModels(state, filter);
        })
        .catch((error: any) => {
            notifyError(error);
        });
}

export const doPublishModel = (payload: IPublishModelCommand, state: any, filter: IModelFilterDto): void => {
    let command: ICommand = {
        id: GuidService.getNewGUIDString(),
        name: "PublishModel",
        payload: payload
    };

    let query = new RawPostQuery(StoreService.auth, StoreService.servicesApi, "commands");
    query.execute(command)
        .then((response: AxiosResponse) => {
            notifyOk(response.data.text);
            loadModels(state, filter);
        })
        .catch((error: any) => {
            notifyError(error);
        });
}

export const deleteLearningsItem = (payload: IDeleteLearningRecordCommand, state: any, filter: ILearningFilterDto): void => {
    let command: ICommand = {
        id: GuidService.getNewGUIDString(),
        name: "DeleteLearningRecord",
        payload: payload
    };

    let query = new RawPostQuery(StoreService.auth, StoreService.servicesApi, "commands");
    query.execute(command)
        .then((response: AxiosResponse) => {
            notifyOk(response.data.text);
            loadLearnings(state, filter);
        })
        .catch((error: any) => {
            notifyError(error);
        });
}

export const deletePatternsItem = (payload: IDeletePatternCommand, state: any, filter: IPatternsFilterDto): void => {
    let command: ICommand = {
        id: GuidService.getNewGUIDString(),
        name: "DeletePattern",
        payload: payload
    };

    let query = new RawPostQuery(StoreService.auth, StoreService.servicesApi, "commands");
    query.execute(command)
        .then((response: AxiosResponse) => {
            notifyOk(response.data.text);
            loadPatternss(state, filter);
        })
        .catch((error: any) => {
            notifyError(error);
        });
}


export const deleteCategoryItem = (commandId: string, payload: IDeleteCategoryCommand): void => {

    StoreService.dispatch(Actions.categoryItemLoading({isAdding:false}));

    let command: ICommand = {
        id: commandId,
        name: "DeleteChatBotCategory",
        payload: payload
    };

    let query = new RawPostQuery(StoreService.auth, StoreService.servicesApi, "commands");
    query.execute(command)
        .then((response: AxiosResponse) => {
            notifyOk(response.data.text);
            StoreService.dispatch(Actions.categoryListPageSetCurrentPage(0));
            endEditCategoryItem();
        })
        .catch((error: any) => {
            notifyError(error);
        });
}

export const addCategoryItem = (partId?: string, partCaption?: string, subpartId?: string, subpartCaption?: string): void => {
    StoreService.history.push(Const.NavigationPathCategoryItemAdd);
    StoreService.dispatch(Actions.categoryItemLoading({isAdding: true, partitionId: partId, partitionCaption: partCaption, subPartId: subpartId, subPartCaption: subpartCaption}));
}

export const addCategoryItemInit = (partId?: string, partCaption?: string, subpartId?: string, subpartCaption?: string): void => {
    var newCat = {
        id: 0,
        name: '',
        response: '',
        setContext: '',
        setMode: '',
        isIneligible: false,
        isDisabled: false,
        requiredRoster: null,
        upperPartition: partId ? { id: partId, title: partCaption } : null,
        partition: subpartId ? { id: subpartId, title: subpartCaption } : null,
        patterns: []
    }
    StoreService.dispatch(Actions.categoryItemLoaded(newCat));
}

export const editCategoryItem = (id: string): void =>
{
    StoreService.history.push(Const.NavigationPathCategoryItemEdit.replace(":id", id.toString()));
    StoreService.dispatch(Actions.categoryItemLoading({isAdding:false}));
    getCategoryItem(id);
    
}

export const getCategoryItem = (id: string): void =>
{
    let query = new ItemQuery(StoreService.auth, StoreService.servicesApi, "chatbot");
    console.log('getCategoryItem', id, StoreService.servicesApi);
    query.execute(id)
        .then((response: AxiosResponse) => {
            console.log('getCategoryItem ok', response);
            StoreService.dispatch(Actions.categoryItemLoaded(response.data));
        })
        .catch((error: any) => {
            notifyError(error);
            StoreService.dispatch(Actions.categoryItemLoadingError(error));
        });
}

export const addCategoryItemCancel = (): void => {
    StoreService.history.push(Const.NavigationPathCategoryList);
}

export const createCategoryItem = (commandId: string, payload: ICreateCategoryCommand): void => {

    let command: ICommand = {
        id: commandId,
        name: "CreateChatBotCategory",
        payload: payload
    };

    let query = new RawPostQuery(StoreService.auth, StoreService.servicesApi, "commands");
    query.execute(command)
        .then((response: AxiosResponse) => {
            notifyOk(response.data.text);
            StoreService.history.push(Const.NavigationPathCategoryList);
        })
        .catch((error: any) => {
            notifyError(error);
        });
}

export const unpublishCategories = (commandId: string, payload: IUnpublishCategoriesCommand, pageNum? : number, filter?: ICategoryFilter): void => {

    StoreService.dispatch(Actions.categoryListPublishing());

    let command: ICommand = {
        id: commandId,
        name: "ChatBotUnpublishCategories",
        payload: payload
    };

    let query = new RawPostQuery(StoreService.auth, StoreService.servicesApi, "commands");
    query.execute(command)
        .then((response: AxiosResponse) => {
            StoreService.dispatch(Actions.categoryListPublished());
            notifyOk(response.data.text);
            loadCategoryListPage(pageNum, filter);
        })
        .catch((error: any) => {
            StoreService.dispatch(Actions.categoryListPublished());
            notifyError(error);
        });
}

export const publishCategories = (commandId: string, payload: IPublishCategoriesCommand, pageNum? : number, filter?: ICategoryFilter): void => {

    StoreService.dispatch(Actions.categoryListPublishing());

    let command: ICommand = {
        id: commandId,
        name: "ChatBotPublishCategories",
        payload: payload
    };

    let query = new RawPostQuery(StoreService.auth, StoreService.servicesApi, "commands");
    query.execute(command)
        .then((response: AxiosResponse) => {
            StoreService.dispatch(Actions.categoryListPublished());
            notifyOk(response.data.text);
            loadCategoryListPage(pageNum, filter);
        })
        .catch((error: any) => {
            StoreService.dispatch(Actions.categoryListPublished());
            notifyError(error);
        });
}

// Partitions
export const loadPartitionListPage = (search?: string): void =>
{
    StoreService.dispatch(Actions.partitionListPageLoading());

    let skip = 0;
    let query = new PartitionListQuery(StoreService.auth, StoreService.servicesApi, "chatbotpartitions/collection");
    query.execute(search, skip, 0)
        .then((response: AxiosResponse) => {
            let data : ILoadPageResponseDto<IPartitionDto> = {
                data: response.data,
                pageNumber: 0
            };
            StoreService.dispatch(Actions.partitionListPageLoaded(data));
            StoreService.dispatch(Actions.subpartListPageLoaded({data:{items:[]}}));
        })
        .catch((error: any) => {
            notifyError(error);
            StoreService.dispatch(Actions.partitionListPageLoadingError(error));
        });
}

const getPartitionItem = (id: string): void =>
{
    let query = new ItemQuery(StoreService.auth, StoreService.servicesApi, "chatbotpartitions");
    query.execute(id)
        .then((response: AxiosResponse) => {
            StoreService.dispatch(Actions.partitionItemLoaded(response.data));
        })
        .catch((error: any) => {
            notifyError(error);
            StoreService.dispatch(Actions.partitionItemLoadingError(error));
        });
}

export const editPartitionItem = (id: string): void =>
{
    StoreService.dispatch(Actions.partitionItemLoading());
    StoreService.history.push(Const.NavigationPathPartitionItemEdit);
    getPartitionItem(id);
}

export const endEditPartitionItem = (): void =>
{
    StoreService.dispatch(Actions.partitionItemEndEdit());
    StoreService.history.push(Const.NavigationPathPartitionList);
    loadPartitionListPage(null);
}

export const saveEditedPartitionItem = (commandId: string, payload: IEditPartCommand): void => {

    StoreService.dispatch(Actions.partitionItemLoading());

    let command: ICommand = {
        id: commandId,
        name: "EditChatBotPartition",
        payload: payload
    };

    let query = new RawPostQuery(StoreService.auth, StoreService.servicesApi, "commands");
    query.execute(command)
        .then((response: AxiosResponse) => {
            notifyOk(response.data.text);
            getPartitionItem(payload.id);
        })
        .catch((error: any) => {
            notifyError(error);
        });
}

export const deletePartitionItem = (commandId: string, payload: IDeletePartCommand): void => {

    StoreService.dispatch(Actions.partitionItemLoading());

    let command: ICommand = {
        id: commandId,
        name: "DeleteChatBotPartition",
        payload: payload
    };

    let query = new RawPostQuery(StoreService.auth, StoreService.servicesApi, "commands");
    query.execute(command)
        .then((response: AxiosResponse) => {
            notifyOk(response.data.text);
            endEditPartitionItem();
        })
        .catch((error: any) => {
            notifyError(error);
        });
}

export const addPartitionItem = (): void => {
    StoreService.history.push(Const.NavigationPathPartitionItemAdd);
}

export const addPartitionItemCancel = (): void => {
    StoreService.history.push(Const.NavigationPathPartitionList);
}

export const createPartitionItem = (commandId: string, payload: ICreatePartCommand): void => {

    let command: ICommand = {
        id: commandId,
        name: "CreateChatBotPartition",
        payload: payload
    };

    let query = new RawPostQuery(StoreService.auth, StoreService.servicesApi, "commands");
    query.execute(command)
        .then((response: AxiosResponse) => {
            notifyOk(response.data.text);
            StoreService.history.push(Const.NavigationPathPartitionList);
            //editPartItem(response.data.payload.id as string);
        })
        .catch((error: any) => {
            notifyError(error);
        });
}


// Subparts
export const loadSubpartListPage = (search?: string, partId?: string, partCaption?: string): void =>
{
    StoreService.dispatch(Actions.subpartListPageLoading([partId, partCaption]));

    if (!partId) {
        StoreService.dispatch(Actions.subpartListPageLoaded({data:{items:[]}}));
        return;
    }

    let skip = 0;
    let query = new PartitionListQuery(StoreService.auth, StoreService.servicesApi, "chatbotsubparts/collection");
    query.execute(search, skip, 0, partId)
        .then((response: AxiosResponse) => {
            let data : ILoadPageResponseDto<ISubpartDto> = {
                data: response.data,
                pageNumber: 0
            };
            StoreService.dispatch(Actions.subpartListPageLoaded(data));
        })
        .catch((error: any) => {
            notifyError(error);
            StoreService.dispatch(Actions.subpartListPageLoadingError(error));
        });
}

const getSubpartItem = (id: string): void =>
{
    let query = new ItemQuery(StoreService.auth, StoreService.servicesApi, "chatbotsubparts");
    query.execute(id)
        .then((response: AxiosResponse) => {
            StoreService.dispatch(Actions.subPartItemLoaded(response.data));
        })
        .catch((error: any) => {
            notifyError(error);
            StoreService.dispatch(Actions.subPartItemLoadingError(error));
        });
}

export const editSubpartItem = (id: string): void =>
{
    StoreService.dispatch(Actions.subPartItemLoading());
    StoreService.history.push(Const.NavigationPathSubpartItemEdit);
    getSubpartItem(id);
}
export const viewCategoryList = (partId: string,partCaption: string,subpartId: string,subpartCaption: string): void =>
{
    StoreService.history.push(Const.NavigationPathCategoryList);
    StoreService.dispatch(Actions.categoryListPageSetPartition([partId, partCaption, subpartId, subpartCaption]));
}

export const endEditSubpartItem = (): void =>
{
    StoreService.dispatch(Actions.partitionItemEndEdit());
    StoreService.history.push(Const.NavigationPathPartitionList);
    loadPartitionListPage(null);
    loadSubpartListPage(null);
}

export const saveEditedSubpartItem = (commandId: string, payload: IEditSubpartCommand): void => {

    StoreService.dispatch(Actions.subPartItemLoading());

    let command: ICommand = {
        id: commandId,
        name: "EditChatBotSubpartition",
        payload: payload
    };

    let query = new RawPostQuery(StoreService.auth, StoreService.servicesApi, "commands");
    query.execute(command)
        .then((response: AxiosResponse) => {
            notifyOk(response.data.text);
            getSubpartItem(payload.id);
        })
        .catch((error: any) => {
            notifyError(error);
        });
}

export const deleteSubpartItem = (commandId: string, payload: IDeleteSubpartCommand): void => {

    StoreService.dispatch(Actions.subPartItemLoading());

    let command: ICommand = {
        id: commandId,
        name: "DeleteChatBotSubpartition",
        payload: payload
    };

    let query = new RawPostQuery(StoreService.auth, StoreService.servicesApi, "commands");
    query.execute(command)
        .then((response: AxiosResponse) => {
            notifyOk(response.data.text);
            endEditSubpartItem();
        })
        .catch((error: any) => {
            notifyError(error);
        });
}

export const addSubpartItem = (partId?: string, partCaption?: string): void => {
    StoreService.history.push(Const.NavigationPathSubpartItemAdd);
    StoreService.dispatch(Actions.subPartItemSetPart([partId, partCaption]));
}

export const addSubpartItemCancel = (): void => {
    StoreService.history.push(Const.NavigationPathPartitionList);
}

export const createSubpartItem = (commandId: string, payload: ICreateSubpartCommand): void => {

    let command: ICommand = {
        id: commandId,
        name: "CreateChatBotSubpartition",
        payload: payload
    };

    let query = new RawPostQuery(StoreService.auth, StoreService.servicesApi, "commands");
    query.execute(command)
        .then((response: AxiosResponse) => {
            notifyOk(response.data.text);
            StoreService.history.push(Const.NavigationPathPartitionList);
        })
        .catch((error: any) => {
            notifyError(error);
        });
}

export const makeCopy = (payload: ICategoryDto) => {
    var nextId = -1;
    payload.patterns.forEach(function(pattern) {pattern.id = nextId--;});
    if (payload.name) payload.name += ' (копия)';
    StoreService.dispatch(Actions.categoryItemLoaded(payload));
}

export const patternToLearn = (commandId: string, payload: ICopyPatternToLearnCommand) => {
    let command: ICommand = {
        id: commandId,
        name: "CopyPatternToLearn",
        payload: payload
    };

    let query = new RawPostQuery(StoreService.auth, StoreService.servicesApi, "commands");
    query.execute(command)
        .then((response: AxiosResponse) => {
            notifyOk(response.data.text);
        })
        .catch((error: any) => {
            notifyError(error);
        });
}