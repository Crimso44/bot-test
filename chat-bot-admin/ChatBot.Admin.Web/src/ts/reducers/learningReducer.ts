import { ILearningState } from '../interfaces/IPortalState';
import * as _ from 'lodash';
import * as ActionConst from "../const/actionConstants";
import { handleActions, Action } from 'redux-actions';
import { ILearningFilterDto } from '../interfaces/IHistoryFilterDto';
import { ColdObservable } from 'rxjs/testing/ColdObservable';

const initialState: ILearningState = {
    learnings: [],
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

export const learningReducer = handleActions<any, any>({

    [ActionConst.LEARNING_PAGE_LOADING]: (state: ILearningState, action: Action<any>): any => {
        return updateState([state, {
            isLoading: true,
            loadingError: null
        }]);
    },

    [ActionConst.LEARNING_PAGE_LOADED]: (state: ILearningState, action: Action<any>): any => {
        let data = (action.payload as any);
        return updateState([state, {
                learnings: data.items,
                pages: Math.ceil(data.count / data.pageSize),
                isLoading: false,
                loadingError: null,
            }]);
    },

    [ActionConst.LEARNING_PAGE_LOADING_ERROR]: (state: ILearningState, action: Action<any>): any => {
        return updateState([state, {
            isLoading: false,
            loadingError: action.payload
        }]);
    },

    [ActionConst.LEARNING_FILTER_CHANGED]: (state: ILearningState, action: Action<any>): any => {
        var filter = (action.payload as ILearningFilterDto);
        return updateState([state, {filter: {...filter}}]);
    }

}, initialState);

