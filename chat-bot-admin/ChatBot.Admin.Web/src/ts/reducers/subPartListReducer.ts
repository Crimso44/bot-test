import { ISubPartListState } from '../interfaces/IPortalState';
import { ISubpartDto } from '../interfaces/ISubpartDto';
import { ILoadPageResponseDto } from "../interfaces/ILoadPageResponseDto";
import * as _ from 'lodash';
import * as ActionConst from "../const/actionConstants";
import { handleActions, Action } from 'redux-actions';

const initialState: ISubPartListState = {
    items: [],
    currentPageNumber: 0,
    totalItemsCount: undefined,
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

export const subPartListReducer = handleActions<any, any>({

}, initialState);

