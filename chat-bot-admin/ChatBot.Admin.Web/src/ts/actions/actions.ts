import * as ActionConst from "../const/actionConstants";
import { createAction } from 'redux-actions';

// LinkList
export const categoryListPageLoading = createAction(ActionConst.CATEGORY_LIST_PAGE_LOADING);
export const categoryListPageLoaded = createAction(ActionConst.CATEGORY_LIST_PAGE_LOADED);
export const categoryListPageLoadingError = createAction(ActionConst.CATEGORY_LIST_PAGE_LOADING_ERROR);
export const categoryListPageSetCurrentPage = createAction(ActionConst.CATEGORY_LIST_PAGE_SET_CURRENT_PAGE);
export const categoryListPageSetPartition = createAction(ActionConst.CATEGORY_LIST_PAGE_SET_CURRENT_PARTITION);
export const categoryListPublishing = createAction(ActionConst.CATEGORY_LIST_PUBLISHING);
export const categoryListPublished = createAction(ActionConst.CATEGORY_LIST_PUBLISHED);

export const learningLoading = createAction(ActionConst.LEARNING_LOADING);
export const learningLoaded = createAction(ActionConst.LEARNING_LOADED);
export const learningLoadingError = createAction(ActionConst.LEARNING_LOADING_ERROR);

export const learningPageLoading = createAction(ActionConst.LEARNING_PAGE_LOADING);
export const learningPageLoaded = createAction(ActionConst.LEARNING_PAGE_LOADED);
export const learningPageLoadingError = createAction(ActionConst.LEARNING_PAGE_LOADING_ERROR);
export const learningFilterChanged = createAction(ActionConst.LEARNING_FILTER_CHANGED);

export const patternsPageLoading = createAction(ActionConst.PATTERNS_PAGE_LOADING);
export const patternsPageLoaded = createAction(ActionConst.PATTERNS_PAGE_LOADED);
export const patternsPageLoadingError = createAction(ActionConst.PATTERNS_PAGE_LOADING_ERROR);
export const patternsFilterChanged = createAction(ActionConst.PATTERNS_FILTER_CHANGED);

export const modelPageLoading = createAction(ActionConst.MODEL_PAGE_LOADING);
export const modelPageLoaded = createAction(ActionConst.MODEL_PAGE_LOADED);
export const modelPageLoadingError = createAction(ActionConst.MODEL_PAGE_LOADING_ERROR);
export const modelFilterChanged = createAction(ActionConst.MODEL_FILTER_CHANGED);

// Settings
export const settingsPageUpdate = createAction(ActionConst.SETTINGS_PAGE_UPDATE);
export const settingsPageLoading = createAction(ActionConst.SETTINGS_PAGE_LOADING);
export const settingsPageLoaded = createAction(ActionConst.SETTINGS_PAGE_LOADED);
export const settingsPageLoadingError = createAction(ActionConst.SETTINGS_PAGE_LOADING_ERROR);

// History
export const historyPageLoading = createAction(ActionConst.HISTORY_PAGE_LOADING);
export const historyPageLoaded = createAction(ActionConst.HISTORY_PAGE_LOADED);
export const historyPageLoadingError = createAction(ActionConst.HISTORY_PAGE_LOADING_ERROR);
export const historyFilterChanged = createAction(ActionConst.HISTORY_FILTER_CHANGED);

// LinkListItem
export const categoryItemLoading = createAction(ActionConst.CATEGORY_ITEM_LOADING);
export const categoryItemLoaded = createAction(ActionConst.CATEGORY_ITEM_LOADED);
export const categoryItemLoadingError = createAction(ActionConst.CATEGORY_ITEM_LOADING_ERROR);
export const categoryItemEndEdit = createAction(ActionConst.CATEGORY_ITEM_END_EDIT);
export const categoryItemSetPartition = createAction(ActionConst.CATEGORY_ITEM_SET_CURRENT_PARTITION);

// GroupList
export const partitionListPageLoading = createAction(ActionConst.PARTITION_LIST_PAGE_LOADING);
export const partitionListPageLoaded = createAction(ActionConst.PARTITION_LIST_PAGE_LOADED);
export const partitionListPageLoadingError = createAction(ActionConst.PARTITION_LIST_PAGE_LOADING_ERROR);

// GroupListItem
export const partitionItemLoading = createAction(ActionConst.PARTITION_ITEM_LOADING);
export const partitionItemLoaded = createAction(ActionConst.PARTITION_ITEM_LOADED);
export const partitionItemLoadingError = createAction(ActionConst.PARTITION_ITEM_LOADING_ERROR);
export const partitionItemEndEdit = createAction(ActionConst.PARTITION_ITEM_END_EDIT);

// SubgroupList
export const subpartListPageLoading = createAction(ActionConst.SUBPART_LIST_PAGE_LOADING);
export const subpartListPageLoaded = createAction(ActionConst.SUBPART_LIST_PAGE_LOADED);
export const subpartListPageLoadingError = createAction(ActionConst.SUBPART_LIST_PAGE_LOADING_ERROR);

// SubgroupListItem
export const subPartItemLoading = createAction(ActionConst.SUBPART_ITEM_LOADING);
export const subPartItemLoaded = createAction(ActionConst.SUBPART_ITEM_LOADED);
export const subPartItemLoadingError = createAction(ActionConst.SUBPART_ITEM_LOADING_ERROR);
export const subPartItemEndEdit = createAction(ActionConst.SUBPART_ITEM_END_EDIT);
export const subPartItemSetPart = createAction(ActionConst.SUBPART_ITEM_SET_PARTITION);

