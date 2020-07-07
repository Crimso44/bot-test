import { ISettingsState } from '../interfaces/IPortalState';
import * as _ from 'lodash';
import * as ActionConst from "../const/actionConstants";
import { handleActions, Action } from 'redux-actions';

const initialState: ISettingsState = {
    useModel: true,
    useMLThreshold: false, 
    useMLMultiAnswer: false, 
    mLThreshold: "", 
    mLMultiThreshold: "",
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

export const settingsReducer = handleActions<any, any>({

    [ActionConst.SETTINGS_PAGE_UPDATE]: (state: ISettingsState, action: Action<any>): any => {
        let data = (action.payload as any);
        return updateState([state, {
            useModel: data.useModel,
            useMLThreshold: data.useMLThreshold, 
            useMLMultiAnswer: data.useMLMultiAnswer, 
            mLThreshold: data.mLThreshold, 
            mLMultiThreshold: data.mLMultiThreshold,
        }]);
    },

    [ActionConst.SETTINGS_PAGE_LOADING]: (state: ISettingsState, action: Action<any>): any => {
        return updateState([state, {
            isLoading: true,
            loadingError: null
        }]);
    },

    [ActionConst.SETTINGS_PAGE_LOADED]: (state: ISettingsState, action: Action<any>): any => {
        let data = (action.payload as any);
        return updateState([state, {
                useModel: data['useModel'] != 'False',
                useMLThreshold: data['useMLThreshold'] == 'True',
                useMLMultiAnswer: data['useMLMultiAnswer'] == 'True',
                mLThreshold: data['mlThreshold'],
                mLMultiThreshold: data['mlMultiThreshold'],
                isLoading: false,
                loadingError: null,
            }]);
    },

    [ActionConst.SETTINGS_PAGE_LOADING_ERROR]: (state: ISettingsState, action: Action<any>): any => {
        return updateState([state, {
            isLoading: false,
            loadingError: action.payload
        }]);
    }

}, initialState);

