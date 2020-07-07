import { IPartitionItemState } from '../interfaces/IPortalState';
import { IPartitionDto } from '../interfaces/IPartitionDto';
import * as _ from 'lodash';
import * as ActionConst from "../const/actionConstants";
import { handleActions, Action } from 'redux-actions';
import * as GuidService from "../services/GuidService";


const initialState: IPartitionItemState = {
    versionVirtId: null,
    item: null,
    isLoading: false,
    loadingError: null
};

function updateState<T>(States: Array<T>): T {
    var result = <T>{};
    for (var state in States) {
        _.assign(result, States[state]);
    }
    return result;
}

export const partItemReducer = handleActions<any, any>({


    [ActionConst.PARTITION_ITEM_LOADING]: (state: IPartitionItemState, action: Action<any>): any => {
        return updateState([state, {
            versionVirtId: null,
            item: null,
            isLoading: true,
            loadingError: null
        }]);
    },

    [ActionConst.PARTITION_ITEM_LOADED]: (state: IPartitionItemState, action: Action<any>): any => {
        let data = (action.payload as IPartitionDto);

        return updateState([state, {
            versionVirtId: GuidService.getNewGUIDString(),
            item: data,
            isLoading: false,
            loadingError: null
            }]);
    },

    [ActionConst.PARTITION_ITEM_LOADING_ERROR]: (state: IPartitionItemState, action: Action<any>): any => {
        return updateState([state, {
            versionVirtId: null,
            item: null,
            isLoading: false,
            loadingError: action.payload
        }]);
    },

    [ActionConst.PARTITION_ITEM_END_EDIT]: (state: IPartitionItemState, action: Action<any>): any => {
        return updateState([state, {
            versionVirtId: null,
            item: null,
            isLoading: false,
            loadingError: null
        }]);
    }

}, initialState);
