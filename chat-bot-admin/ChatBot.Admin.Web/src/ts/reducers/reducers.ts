import * as Redux from 'redux';
import { categoryListReducer } from './categoryListReducer';
import { categoryItemReducer } from './categoryItemReducer';
import { partListReducer } from './partListReducer';
import { partItemReducer } from './partItemReducer';
import { subPartListReducer } from './subPartListReducer';
import { subPartItemReducer } from './subPartItemReducer';
import { settingsReducer } from './settingsReducer';
import { historyReducer } from './historyReducer';
import { learningReducer } from './learningReducer';
import { patternsReducer } from './patternsReducer';
import { modelReducer } from './modelReducer';
import { IAppState } from "../interfaces/IPortalState";

export const reducers = Redux.combineReducers<IAppState>({
    categoryList: categoryListReducer,
    categoryItem: categoryItemReducer,
    partitionList: partListReducer,
    partitionItem: partItemReducer,
    subPartList: subPartListReducer,
    subPartItem: subPartItemReducer,
    settings: settingsReducer,
    history: historyReducer,
    learning: learningReducer,
    patterns: patternsReducer,
    model: modelReducer
});