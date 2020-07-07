import { IHistoryState } from '../interfaces/IPortalState';
import * as _ from 'lodash';
import * as ActionConst from "../const/actionConstants";
import { handleActions, Action } from 'redux-actions';
import { IHistoryFilterDto } from '../interfaces/IHistoryFilterDto';
import { ColdObservable } from 'rxjs/testing/ColdObservable';

const initialState: IHistoryState = {
    history: [],
    pages: -1,
    isLoading: false,
    loadingError: null,
    filter: {
        isAnsweredMl: true,
        isAnsweredEve: true,
        isAnsweredButton: true,
        isAnsweredNo: true,
        isAnsweredOther: true,

        isLikeYes: true,
        isLikeNo: true,
        isDisLike: true,

        isMlYes: true,
        isMlAnswer: true,
        isMlWrong: true,
        isMlNo: true
    },
    tblSorted: [{
        id: "questionDate",
        desc: true
    }],
    tblFiltered: [],
    tblPage: 0,
    tblPageSize: 20
};

function updateState<T>(States: Array<T>): T {
    var result = <T>{};
    for (var state in States) {
        _.assign(result, States[state]);
    }
    return result;
}

export const historyReducer = handleActions<any, any>({

    [ActionConst.HISTORY_PAGE_LOADING]: (state: IHistoryState, action: Action<any>): any => {
        return updateState([state, {
            isLoading: true,
            loadingError: null
        }]);
    },

    [ActionConst.HISTORY_PAGE_LOADED]: (state: IHistoryState, action: Action<any>): any => {
        let data = (action.payload as any);
        return updateState([state, {
                history: data.items,
                pages: Math.ceil(data.count / data.pageSize),
                isLoading: false,
                loadingError: null,
            }]);
    },

    [ActionConst.HISTORY_PAGE_LOADING_ERROR]: (state: IHistoryState, action: Action<any>): any => {
        return updateState([state, {
            isLoading: false,
            loadingError: action.payload
        }]);
    },

    [ActionConst.HISTORY_FILTER_CHANGED]: (state: IHistoryState, action: Action<any>): any => {
        var filter = (action.payload as IHistoryFilterDto);
        return updateState([state, {filter: {...filter}}]);
    }

}, initialState);

