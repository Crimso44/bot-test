import { IModelState } from '../interfaces/IPortalState';
import * as _ from 'lodash';
import * as ActionConst from "../const/actionConstants";
import { handleActions, Action } from 'redux-actions';
import { IModelFilterDto } from '../interfaces/IHistoryFilterDto';
import { ColdObservable } from 'rxjs/testing/ColdObservable';

const initialState: IModelState = {
    models: [],
    pages: -1,
    isLoading: false,
    loadingError: null,
    filter: {}
};

function updateState<T>(States: Array<T>): T {
    var result = <T>{};
    for (var state in States) {
        _.assign(result, States[state]);
    }
    return result;
}

export const modelReducer = handleActions<any, any>({

    [ActionConst.MODEL_PAGE_LOADING]: (state: IModelState, action: Action<any>): any => {
        return updateState([state, {
            isLoading: true,
            loadingError: null
        }]);
    },

    [ActionConst.MODEL_PAGE_LOADED]: (state: IModelState, action: Action<any>): any => {
        let data = (action.payload as any);
        return updateState([state, {
                models: data.items,
                pages: Math.ceil(data.count / data.pageSize),
                isLoading: false,
                loadingError: null,
            }]);
    },

    [ActionConst.MODEL_PAGE_LOADING_ERROR]: (state: IModelState, action: Action<any>): any => {
        return updateState([state, {
            isLoading: false,
            loadingError: action.payload
        }]);
    },

    [ActionConst.MODEL_FILTER_CHANGED]: (state: IModelState, action: Action<any>): any => {
        var filter = (action.payload as IModelFilterDto);
        return updateState([state, {filter: {...filter}}]);
    }

}, initialState);

