import { ICategoryListState } from '../interfaces/IPortalState';
import { ICategoryDto } from '../interfaces/ICategoryDto';
import { ILoadPageResponseDto } from "../interfaces/ILoadPageResponseDto";
import * as _ from 'lodash';
import * as ActionConst from "../const/actionConstants";
import { handleActions, Action } from 'redux-actions';
import { ICategoryFilter } from '../interfaces/IPortalState';

const initialState: ICategoryListState = {

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

export const categoryListReducer = handleActions<any, any>({

    [ActionConst.CATEGORY_LIST_PAGE_LOADING]: (state: ICategoryListState, action: Action<any>): any => {
        let data = (action.payload as ICategoryFilter);
        return updateState([state, {
            isLoading: true,
            loadingError: null,
            partitionId: data.partitionId,
            partitionCaption: data.partitionCaption,
            subPartId: data.subPartId,
            subPartCaption: data.subPartCaption,
            filterCategory: data.filterCategory,
            filterPattern: data.filterPattern,
            filterResponse: data.filterResponse,
            filterContext: data.filterContext,
            filterChangedBy: data.filterChangedBy,
            filterChangedByName: data.filterChangedByName,
            sorting: data.sorting,
            sortingName: data.sortingName,
            sortDescent: data.sortDescent,
            isDisabled: data.isDisabled
        }]);
    },

    [ActionConst.CATEGORY_LIST_PAGE_LOADED]: (state: ICategoryListState, action: Action<any>): any => {
        let data = (action.payload as ILoadPageResponseDto<ICategoryDto>);

        return updateState([state, {
                items: data.data.items,
                totalItemsCount: data.data.count,
                currentPageNumber: data.pageNumber,
                isLoading: false,
                loadingError: null
            }]);
    },

    [ActionConst.CATEGORY_LIST_PAGE_LOADING_ERROR]: (state: ICategoryListState, action: Action<any>): any => {
        return updateState([state, {
            isLoading: false,
            loadingError: action.payload
        }]);
    },

    [ActionConst.CATEGORY_LIST_PAGE_SET_CURRENT_PAGE]: (state: ICategoryListState, action: Action<any>): any => {
        return updateState([state, {
            currentPageNumber: action.payload
        }]);
    },

    [ActionConst.CATEGORY_LIST_PAGE_SET_CURRENT_PARTITION]: (state: ICategoryListState, action: Action<any>): any => {
        let data = (action.payload as string[]);
        return updateState([state, {
            partitionId: data[0],
            partitionCaption: data[1],
            subPartId: data[2],
            subPartCaption: data[3]
        }]);
    },

    [ActionConst.CATEGORY_LIST_PUBLISHING]: (state: ICategoryListState, action: Action<any>): any => {
        return updateState([state, {
            isLoading: true
        }]);
    },

    [ActionConst.CATEGORY_LIST_PUBLISHED]: (state: ICategoryListState, action: Action<any>): any => {
        let data = (action.payload as ILoadPageResponseDto<ICategoryDto>);

        return updateState([state, {
                isLoading: false
            }]);
    }



}, initialState);

