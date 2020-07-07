import { ISubPartItemState } from '../interfaces/IPortalState';
import { ISubpartDto } from '../interfaces/ISubpartDto';
import * as _ from 'lodash';
import * as ActionConst from "../const/actionConstants";
import { handleActions, Action } from 'redux-actions';
import * as GuidService from "../services/GuidService";

const initialState: ISubPartItemState = {
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

export const subPartItemReducer = handleActions<any, any>({

    [ActionConst.SUBPART_ITEM_LOADING]: (state: ISubPartItemState, action: Action<any>): any => {
        return updateState([state, {
            versionVirtId: null,
            item: null,
            isLoading: true,
            loadingError: null
        }]);
    },

    [ActionConst.SUBPART_ITEM_LOADED]: (state: ISubPartItemState, action: Action<any>): any => {
        let data = (action.payload as ISubpartDto);

        return updateState([state, {
            versionVirtId: GuidService.getNewGUIDString(),
            item: data,
            isLoading: false,
            loadingError: null
            }]);
    },

    [ActionConst.SUBPART_ITEM_LOADING_ERROR]: (state: ISubPartItemState, action: Action<any>): any => {
        return updateState([state, {
            versionVirtId: null,
            item: null,
            isLoading: false,
            loadingError: action.payload
        }]);
    },

    [ActionConst.SUBPART_ITEM_END_EDIT]: (state: ISubPartItemState, action: Action<any>): any => {
        return updateState([state, {
            versionVirtId: null,
            item: null,
            isLoading: false,
            loadingError: null
        }]);
    },

    [ActionConst.SUBPART_ITEM_SET_PARTITION]: (state: ISubPartItemState, action: Action<any>): any => {
        let data = (action.payload as string[]);
        return updateState([state, {
            parentPartId: data[0],
            parentPartCaption: data[1]
        }]);
    }

}, initialState);
