import { IPartitionListState } from '../interfaces/IPortalState';
import { IPartitionDto } from '../interfaces/IPartitionDto';
import { ISubpartDto } from '../interfaces/ISubpartDto';
import { ICollectionDto } from "../interfaces/ICollectionDto";
import { ILoadPageResponseDto } from "../interfaces/ILoadPageResponseDto";
import * as _ from 'lodash';
import * as ActionConst from "../const/actionConstants";
import { handleActions, Action } from 'redux-actions';

const initialState: IPartitionListState = {
    items: [],
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

export const partListReducer = handleActions<any, any>({

    [ActionConst.PARTITION_LIST_PAGE_LOADING]: (state: IPartitionListState, action: Action<any>): any => {
        return updateState([state, {
            isLoading: true,
            loadingError: null
        }]);
    },

    [ActionConst.PARTITION_LIST_PAGE_LOADED]: (state: IPartitionListState, action: Action<any>): any => {
        let data = (action.payload as ILoadPageResponseDto<IPartitionDto>);

        return updateState([state, {
                items: data.data.items,
                isLoading: false,
                loadingError: null,
                selectedPartId: null,
                selectedPartCaption: null
            }]);
    },

    [ActionConst.PARTITION_LIST_PAGE_LOADING_ERROR]: (state: IPartitionListState, action: Action<any>): any => {
        return updateState([state, {
            isLoading: false,
            loadingError: action.payload
        }]);
    },

    [ActionConst.SUBPART_LIST_PAGE_LOADING]: (state: IPartitionListState, action: Action<any>): any => {
        let data = (action.payload as string[]);

        return updateState([state, {
            selectedPartId: data[0],
            selectedPartCaption: data[1],
            isLoading: true,
            loadingError: null
        }]);
    },

    [ActionConst.SUBPART_LIST_PAGE_LOADED]: (state: IPartitionListState, action: Action<any>): any => {
        let data = (action.payload as ILoadPageResponseDto<ISubpartDto>);

        return updateState([state, {
                subitems: data.data.items,
                isLoading: false,
                loadingError: null
            }]);
    },

    [ActionConst.SUBPART_LIST_PAGE_LOADING_ERROR]: (state: IPartitionListState, action: Action<any>): any => {
        return updateState([state, {
            isLoading: false,
            loadingError: action.payload
        }]);
    }
}, initialState);

