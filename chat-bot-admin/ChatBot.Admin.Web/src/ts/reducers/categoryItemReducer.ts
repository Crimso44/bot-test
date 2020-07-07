import { ICategoryItemState } from '../interfaces/IPortalState';
import { ICategoryDto } from '../interfaces/ICategoryDto';
import { ILearningDto } from '../interfaces/ILearningDto';
import * as _ from 'lodash';
import * as ActionConst from "../const/actionConstants";
import { handleActions, Action } from 'redux-actions';
import * as GuidService from "../services/GuidService";


const initialState: ICategoryItemState = {
    versionVirtId: null,
    item: null,
    isLoading: false,
    isAdding: false,
    loadingError: null,
    partitionId: null,
    partitionCaption: null,
    subPartId: null,
    subPartCaption: null,
    learnings: [],
    pages: -1
};

function updateState<T>(States: Array<T>): T {
    var result = <T>{};
    for (var state in States) {
        _.assign(result, States[state]);
    }
    return result;
}

export const categoryItemReducer = handleActions<any, any>({


    [ActionConst.CATEGORY_ITEM_LOADING]: (state: ICategoryItemState, action: Action<any>): any => {
        let data = (action.payload as ICategoryItemState);
        return updateState([state, {
            versionVirtId: null,
            item: null,
            isLoading: true,
            isAdding: data.isAdding,
            loadingError: null,
            partitionId : data.partitionId,
            partitionCaption : data.partitionCaption,
            subPartId : data.subPartId,
            subPartCaption : data.subPartCaption
        }]);
    },

    [ActionConst.CATEGORY_ITEM_LOADED]: (state: ICategoryItemState, action: Action<any>): any => {
        let data = (action.payload as ICategoryDto);

        return updateState([state, {
            versionVirtId: GuidService.getNewGUIDString(),
            item: data,
            
            partitionId: data.upperPartition ? data.upperPartition.id : null,
            partitionCaption: data.upperPartition ? data.upperPartition.title : null,
            subPartId: data.partition ? data.partition.id : null,
            subPartCaption: data.partition ? data.partition.title : null,
            
            isAdding: !data.id,
            isLoading: false,
            isIneligible: data.isIneligible,
            isDisabled: data.isDisabled,
            requiredRoster: data.requiredRoster,
            requiredRosterName: data.requiredRosterName,
            loadingError: null
            }]);
    },

    [ActionConst.CATEGORY_ITEM_LOADING_ERROR]: (state: ICategoryItemState, action: Action<any>): any => {
        return updateState([state, {
            versionVirtId: null,
            item: null,
            isLoading: false,
            loadingError: action.payload
        }]);
    },

    [ActionConst.LEARNING_LOADING]: (state: ICategoryItemState, action: Action<any>): any => {
        let data = (action.payload as ICategoryItemState);
        return updateState([state, {
            isLoading: true
        }]);
    },

    [ActionConst.LEARNING_LOADED]: (state: ICategoryItemState, action: Action<any>): any => {
        let data = (action.payload as any);
        return updateState([state, {
            isLoading: false,
            learnings: data.items,
            pages: Math.ceil(data.count / data.pageSize)
    }]);
    },

    [ActionConst.LEARNING_LOADING_ERROR]: (state: ICategoryItemState, action: Action<any>): any => {
        return updateState([state, {
            isLoading: false,
            loadingError: action.payload
        }]);
    },

    [ActionConst.CATEGORY_ITEM_END_EDIT]: (state: ICategoryItemState, action: Action<any>): any => {
        return updateState([state, {
            versionVirtId: null,
            item: null,
            isLoading: false,
            loadingError: null
        }]);
    },

    [ActionConst.CATEGORY_ITEM_SET_CURRENT_PARTITION]: (state: ICategoryItemState, action: Action<any>): any => {
        let data = (action.payload as string[]);
        return updateState([state, {
            groupId: data[0],
            groupCaption: data[1],
            subgroupId: data[2],
            subgroupCaption: data[3]
        }]);
    }

}, initialState);
