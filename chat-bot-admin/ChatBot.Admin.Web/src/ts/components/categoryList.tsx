import * as React from "react";
import { withRouter } from "react-router";
import { connect } from "react-redux";
import MainElement from "./main-element";
import ListHeader from "./categoryHeader";
import { IPortalState, ICategoryFilter } from '../interfaces/IPortalState';
import { ICategoryDto } from '../interfaces/ICategoryDto';
import * as Scenarios from "../actions/scenarios";
import { StoreService } from '../services/StoreService';
import { PermissionService } from "../services/PermissionService";
import * as StringHelpers from '../services/StringHelpers';
import TopMenu from "./topMenu";
import GroupSubgroup from "./group-subgroup";
import * as GuidService from "../services/GuidService";
import {DictionaryService} from "../services/DictionaryService";
import * as Const from "../const/constants";
import { CategoryListQuery } from "../endpoints/CategoryListQuery";
import { AxiosResponse } from 'axios';
import {
    RegistryPlate,
    Paginator,
    IRegistryPlateSpec,
    IRegistryPlateBody,
    IRegistryPlateHeader,
    IRegistryPlateField,
    ButtonBox, Select,
    IconFont, TextField,
    Grid, Row, Col,
    IDropdownItem,
    ButtonTextedWithIcon,
    WrapPreloader,
    Checkbox
} from "@sbt/react-ui-components";
import { formatDateTime } from "../services/DateTimeHelpers";
import { forEach } from "async";
import { IDictionaryDto } from "../interfaces/IDictionaryDto";
import ModalConfirmForm from "./modalConfirmForm";
import {IPublishCategoriesCommand} from "../interfaces/commands/IPublishCategoriesCommand";
import {IUnpublishCategoriesCommand} from "../interfaces/commands/IUnpublishCategoriesCommand";
import * as _ from 'lodash';

interface ICategoryListProps extends ICategoryFilter {
    categoryItems?: ICategoryDto[];
    pageNumber?: number;
    totalItemsCount?: number;
    isLoading?: boolean;
    loadingError?: string;
}

interface ICategoryListState extends ICategoryFilter {
    isFilterChanged: boolean;
}

class CategoryListApp extends MainElement<ICategoryListProps, ICategoryListState>{
    constructor(props) {
        super(props);
        this.onSelectPartition = this.onSelectPartition.bind(this);
    }
    state : ICategoryListState = {
        partitionId: undefined,
        partitionCaption: undefined,
        subPartId: undefined,
        subPartCaption: undefined,
        isFilterChanged: false,
        sorting: "chng",
        sortingName: "Изменено",
        sortDescent: true,
        isDisabled: false
    };

    componentDidMount()
    {
        this.copyPropsToState(this.props);
        let filter = {...this.props, ...{
            sorting: this.props.sorting ? this.props.sorting : this.state.sorting,
            sortingName: this.props.sorting ? this.props.sortingName : this.state.sortingName,
            sortDescent: this.props.sorting ? this.props.sortDescent : this.state.sortDescent
        }}
        Scenarios.loadCategoryListPage(this.props.pageNumber, filter);
    }

    componentWillReceiveProps(nextProps: ICategoryListProps) { 
        this.copyPropsToState(nextProps);
    }

    copyPropsToState = (nextProps: ICategoryListProps) => {
        this.setState({
            partitionId: nextProps.partitionId,
            partitionCaption: nextProps.partitionCaption,
            subPartId: nextProps.subPartId,
            subPartCaption: nextProps.subPartCaption,
            filterCategory: nextProps.filterCategory,
            filterPattern: nextProps.filterPattern,
            filterResponse: nextProps.filterResponse,
            filterContext: nextProps.filterContext,
            filterChangedBy: nextProps.filterChangedBy,
            filterChangedByName: nextProps.filterChangedByName,
            isDisabled: nextProps.isDisabled,
            sorting: nextProps.sorting ? nextProps.sorting : this.state.sorting,
            sortingName: nextProps.sorting ? nextProps.sortingName : this.state.sortingName,
            sortDescent: nextProps.sorting ? nextProps.sortDescent : this.state.sortDescent
        });
    }

    isReadOnly = (): boolean => {
        return !PermissionService.Permissions.canEditChatBot;
    }
    
    onPaginatorPageIconClick = (pageIconNumber: number): void => {
        let pageNumber = pageIconNumber - 1;
        Scenarios.loadCategoryListPage(pageNumber, this.state);
    }
    onSelectPartition = (grId, grCaption, subgrId, subgrCaption):void=>
    {
        this.setState({partitionId:grId, partitionCaption: grCaption, subPartId:subgrId, subPartCaption:subgrCaption, isFilterChanged: true});
    }

    onAddCategoryClick = (): void => {
        Scenarios.addCategoryItem(this.state.partitionId, this.state.partitionCaption, this.state.subPartId, this.state.subPartCaption);
    }

    onListItemClick = (categoryId: string): void => {
        Scenarios.editCategoryItem(categoryId);
    }

    isAnyChanged = () : boolean => {
        var res = false;
        this.props.categoryItems.forEach(value => {
            res = res || value.isChanged || value.isAdded});
        return res;
    }

    buildRegistryPlateItems = () : JSX.Element[] => {
        let items: ICategoryDto[] = this.props.categoryItems;
        return items.map(value => {

            let header: IRegistryPlateHeader = {
                title: value.name,
                statusTheme: value.isDisabled ? 'danger' : 'muted-info',
                statusString: (value.partition ? value.partition.title : "") + (value.isDisabled ? " (отключено)" : "")
            };

            var patterns = [];
            value.patterns.forEach(item => {
                if (item && item.phrase) patterns.push({text: item.phrase});
            });

            let leftBodyFields: IRegistryPlateField[] = [
                {
                    caption: "Ответ:",
                    value: [{text: value.response || "-"}]
                },
                {
                    caption: "Паттерны:",
                    value: patterns.length == 0 ? [{text: "-"}] : patterns
                }
            ];

            let rightBodyFields = 
                value.learningCount ? 
                    <div className="text-1">Выборка для ML: {value.learningCount}</div>
                : undefined;

            let body: IRegistryPlateBody = {
                left: leftBodyFields,
                right: rightBodyFields
            };

            let footer: JSX.Element[] = [
                <div key="{0}">{value.changedOn ? formatDateTime(value.changedOn) + " " + value.changedByName : ""}</div>
            ];

            let spec: IRegistryPlateSpec = {
                header: header,
                body: body,
                footer: footer
            };

            return (
                <div className={value.isChanged || value.isAdded ? 'category-changed' : 'category-unchanged'} key={value.id}>
                    <a href={`/#${Const.NavigationPathCategoryItemEdit.replace(":id", value.id.toString())}`}>
                        <RegistryPlate 
                            spec={spec}
                            onClick={(e: any) => this.onListItemClick(value.id.toString())}
                            id={value.id}
                            />
                    </a>
                </div>
            );
        
            
        });

        
    }

    buildPaginator = (): Paginator => {

        let totalPages: number = Math.ceil(this.props.totalItemsCount / Const.CategoryListSize);

        return (
            (this.props.totalItemsCount > 0)&&
            <Paginator
                active={this.props.pageNumber + 1}
                end={totalPages}
                onChange={this.onPaginatorPageIconClick}
                start={1}
            />
        );
    }
    
    onChangeFilterCategory = (val: string) => {
        this.setState({ filterCategory: val, isFilterChanged: true });
    }

    onChangeFilterPattern = (val: string) => {
        this.setState({ filterPattern: val, isFilterChanged: true });
    }

    onChangeFilterResponse = (val: string) => {
        this.setState({ filterResponse: val, isFilterChanged: true });
    }

    onChangeFilterContext = (val: string) => {
        this.setState({ filterContext: val, isFilterChanged: true });
    }

    onChangeFilterChangedBy = (val: IDictionaryDto) => {
        this.setState({ filterChangedBy: val ? val.id : null, filterChangedByName: val ? val.title: null, isFilterChanged: true });
    }

    onApplyFilterClick = () => {
        this.setState({ isFilterChanged: false });
        Scenarios.loadCategoryListPage(0, this.state);
    }

    onClearFilterClick = () => {
        this.setState({
            partitionId: null,
            partitionCaption: null,
            subPartId: null,
            subPartCaption: null,
            filterCategory: "",
            filterPattern: null,
            filterResponse: null,
            filterContext: null,
            filterChangedBy: null,
            filterChangedByName: null,
            isDisabled: false,
            isFilterChanged: true
        });
    }

    onCategoryIsDisabledChanged = (value: any) => {
        this.setState({
            isDisabled: value.checked,
            isFilterChanged: true
        });
    }

    onChangeSorting = (val: IDictionaryDto) => {
        this.setState({ isFilterChanged: false });
        let changeState = { sorting: val.id, sortingName: val.title };
        var newState = {...this.state, ...changeState};
        Scenarios.loadCategoryListPage(this.props.pageNumber, newState);
    }

    onDescendClick = () => {
        this.setState({ isFilterChanged: false });
        let changeState = {  sortDescent: true };
        var newState = {...this.state, ...changeState};
        Scenarios.loadCategoryListPage(this.props.pageNumber, newState);
    }
    
    onAscendClick = () => {
        this.setState({ isFilterChanged: false });
        let changeState = {  sortDescent: false };
        var newState = {...this.state, ...changeState};
        Scenarios.loadCategoryListPage(this.props.pageNumber, newState);
    }
    
    onSortingQuery = (
        callback: (items: IDropdownItem[]) => void): void => {

        var items = [
            { id: "razd", title: "Раздел" },
            { id: "cat", title: "Название" },
            { id: "pat", title: "Паттерн" },
            { id: "answ", title: "Ответ" },
            { id: "cont", title: "Контекст" },
            { id: "chng", title: "Изменено" }
        ];
        callback(items);
    }

    onDoPublishClick = (confirmed: boolean): void => {
        if(!confirmed) return;

        let payload: IPublishCategoriesCommand = { 
            partitionId: this.state.partitionId || null,
            subPartId: this.state.subPartId || null
        };

        Scenarios.publishCategories(GuidService.getNewGUIDString(), payload, this.props.pageNumber, this.state);
    }

    onPublishClick = () => {
        var question = "Опубликовать изменения";
        question += (
            this.state.partitionId || this.state.subPartId 
            ? " в разделе " + 
                (this.state.partitionId ? this.state.partitionCaption : "") + " " +
                (this.state.subPartId ? this.state.subPartCaption : "") + " "
            : ""
        ) + "?";

        var modal = StoreService.ms({content: ModalConfirmForm, externalProps: {text: question}});
        var handler = modal.open();
        handler.close.then(val => this.onDoPublishClick(val));
    }

    onDoUnpublishClick = (confirmed: boolean): void => {
        if(!confirmed) return;

        let payload: IUnpublishCategoriesCommand = { 
            partitionId: this.state.partitionId || null,
            subPartId: this.state.subPartId || null
        };

        Scenarios.unpublishCategories(GuidService.getNewGUIDString(), payload, this.props.pageNumber, this.state);
    }

    onUnpublishClick = () => {
        let query = new CategoryListQuery(StoreService.auth, StoreService.servicesApi, "chatbot/stat");
        query.executeStat(this.state)
            .then((response: AxiosResponse) => {
                var question = "Удалить последние изменения";
                if (response.data) {
                    question = response.data + "\nВсе равно удалить последние изменения"
                }
                question += (
                    this.state.partitionId || this.state.subPartId 
                    ? " в разделе " + 
                        (this.state.partitionId ? this.state.partitionCaption : "") + " " +
                        (this.state.subPartId ? this.state.subPartCaption : "") + " "
                    : ""
                ) + "?";

                var modal = StoreService.ms({content: ModalConfirmForm, externalProps: {text: question}});
                var handler = modal.open();
                handler.close.then(val => this.onDoUnpublishClick(val));
            });

    }

    onXlsClick = () => {
        window.open(StoreService.servicesApi + '/chatbot/xls');
    }


    view() {
        var items = this.props.categoryItems && this.buildRegistryPlateItems() || null;
        var paginator = this.props.categoryItems && this.buildPaginator() || null;

        return (
            <div className={'group-list-container wide'}>
            {/*<div className="category-list">*/}
                <TopMenu activeItemId={1}/>
                <ListHeader header="Категории" buttonCaption="Добавить категорию" readOnly={this.isReadOnly()} isChanged={this.isAnyChanged()}
                    onClick={this.onAddCategoryClick} onPublishClick={this.onPublishClick} onUnpublishClick={this.onUnpublishClick} onXlsClick={this.onXlsClick} 
                    />
                <div className="category-list__filter">
                <Grid>
                    <GroupSubgroup 
                        onClick={(grId, grCaption, subgrId, subgrCaption) => this.onSelectPartition(grId, grCaption, subgrId, subgrCaption)} 
                        PartId={this.state.partitionId} 
                        SubpartId={this.state.subPartId} 
                        PartCaption={this.state.partitionCaption} 
                        SubpartCaption={this.state.subPartCaption}/>
                    <Row>
                        <Col baseSize={6} breakpointSizes={['md-6', 'sm-12']}>
                            <TextField hasTooltip={true} title="Категория" value={this.state.filterCategory || ''} onChange={this.onChangeFilterCategory} />
                        </Col>
                        <Col baseSize={6} breakpointSizes={['md-6', 'sm-12']}>
                            <TextField hasTooltip={true} title="Паттерн" value={this.state.filterPattern || ''} onChange={this.onChangeFilterPattern} />
                        </Col>
                    </Row>
                    <Row>
                        <Col baseSize={6} breakpointSizes={['md-6', 'sm-12']}>
                            <TextField hasTooltip={true} title="Контекст" value={this.state.filterContext} onChange={this.onChangeFilterContext} />
                        </Col>
                        <Col baseSize={6} breakpointSizes={['md-6', 'sm-12']}>
                            <TextField hasTooltip={true} title="Ответ" value={this.state.filterResponse || ''} onChange={this.onChangeFilterResponse} />
                        </Col>
                    </Row>
                    <Row>
                        <Col baseSize={6} breakpointSizes={['md-6', 'sm-12']}>
                            <Select
                                title="Кто изменил"
                                placeholder=""
                                isRequired={false}
                                maxHeight={200}
                                onSelect={this.onChangeFilterChangedBy}
                                query={(search: string, skip: number, take: number, callback: (items: IDropdownItem[]) => void): void =>
                                    DictionaryService.onChangersQuery(callback)}
                                selectedItem={this.state.filterChangedBy && {id: this.state.filterChangedBy, title: this.state.filterChangedByName} || null}
                                />
                        </Col>
                        <Col baseSize={3} breakpointSizes={['md-6', 'sm-12']}>
                            <div className={'category-list__buttons'}>
                                <Checkbox
                                    id="isDisabled"
                                    isChecked={this.state.isDisabled}
                                    onChange={this.onCategoryIsDisabledChanged}
                                    title="Отлюченные категории"
                                />
                            </div>
                        </Col>
                        <Col baseSize={3} breakpointSizes={['md-6', 'sm-12']}>
                            <div className={'category-list__buttons'}>
                                <ButtonTextedWithIcon
                                    title="Очистить"
                                    onClick={this.onClearFilterClick}
                                    size="s"
                                />
                                <ButtonTextedWithIcon
                                    title="Применить"
                                    onClick={this.onApplyFilterClick}
                                    size="s"
                                    isDisabled={!this.state.isFilterChanged}
                                />
                            </div>
                        </Col>
                    </Row>
                </Grid>
                </div>
                <div className="category-list__top_container">
                    <div className={'paginator'}>{paginator}</div>
                    <div className={'category-list__sorting'}>
                        Сортировка по полю&nbsp;
                        <Select
                            placeholder=""
                            maxHeight={200}
                            maxWidth={150}
                            onSelect={this.onChangeSorting}
                            query={(search: string, skip: number, take: number, callback: (items: IDropdownItem[]) => void): void =>
                                this.onSortingQuery(callback)}
                            selectedItem={this.state.sorting && {id: this.state.sorting, title: this.state.sortingName} || null}
                            />
                            <ButtonBox
                                title="а->Я"
                                onClick={this.onAscendClick}
                                size="s"
                                theme="clear-neutral"
                                isDisabled={!this.state.sortDescent}
                            />
                            <ButtonBox
                                title="Я->а"
                                onClick={this.onDescendClick}
                                size="s"
                                theme="clear-neutral"
                                isDisabled={this.state.sortDescent}
                            />
                    </div>
                </div>
                <div className={'list'}>{items}</div>
                <div className="category-list__top_container">
                    <div className={'paginator'}>{paginator}</div>
                </div>
				{
					this.props.isLoading &&
					<div className="category-list__loader-container">
						<div className="category-list__loader-fade"></div>
						<div className="category-list__loader">
							<WrapPreloader isProcess={this.props.isLoading} />
						</div>
					</div>
				}
            </div>
        );
    }
}

const mapStateToProps = (state: IPortalState, ownProps: any): ICategoryListProps => {
    return {
        categoryItems: state.categoryList.items,
        pageNumber: state.categoryList.currentPageNumber,
        totalItemsCount: state.categoryList.totalItemsCount,
        isLoading: state.categoryList.isLoading,
        loadingError: state.categoryList.loadingError,
        partitionId: state.categoryList.partitionId,
        partitionCaption: state.categoryList.partitionCaption,
        subPartId: state.categoryList.subPartId,
        subPartCaption: state.categoryList.subPartCaption,
        filterCategory: state.categoryList.filterCategory,
        filterPattern: state.categoryList.filterPattern,
        filterResponse: state.categoryList.filterResponse,
        filterContext: state.categoryList.filterContext,
        filterChangedBy: state.categoryList.filterChangedBy,
        filterChangedByName: state.categoryList.filterChangedByName,
        isDisabled: state.categoryList.isDisabled,
        sorting: state.categoryList.sorting,
        sortingName: state.categoryList.sortingName,
        sortDescent: state.categoryList.sortDescent
    };
};

export const CategoryList: any = withRouter(connect(mapStateToProps)(CategoryListApp));