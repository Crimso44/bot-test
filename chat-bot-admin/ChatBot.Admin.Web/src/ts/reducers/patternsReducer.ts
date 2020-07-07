import { IPatternsState } from '../interfaces/IPortalState';
import * as _ from 'lodash';
import * as ActionConst from "../const/actionConstants";
import { handleActions, Action } from 'redux-actions';
import { IPatternsFilterDto } from '../interfaces/IHistoryFilterDto';
import { ColdObservable } from 'rxjs/testing/ColdObservable';

const initialState: IPatternsState = {
    patterns: [],
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

export const patternsReducer = handleActions<any, any>({

    [ActionConst.PATTERNS_PAGE_LOADING]: (state: IPatternsState, action: Action<any>): any => {
        return updateState([state, {
            isLoading: true,
            loadingError: null
        }]);
    },

    [ActionConst.PATTERNS_PAGE_LOADED]: (state: IPatternsState, action: Action<any>): any => {
        let data = (action.payload as any);
        return updateState([state, {
                patterns: data.items,
                pages: Math.ceil(data.count / data.pageSize),
                isLoading: false,
                loadingError: null,
            }]);
    },

    [ActionConst.PATTERNS_PAGE_LOADING_ERROR]: (state: IPatternsState, action: Action<any>): any => {
        return updateState([state, {
            isLoading: false,
            loadingError: action.payload
        }]);
    },

    [ActionConst.PATTERNS_FILTER_CHANGED]: (state: IPatternsState, action: Action<any>): any => {
        var filter = (action.payload as IPatternsFilterDto);
        return updateState([state, {filter: {...filter}}]);
    }

}, initialState);

