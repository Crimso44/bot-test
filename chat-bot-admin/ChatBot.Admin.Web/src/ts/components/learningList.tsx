import * as React from "react";
import { withRouter } from "react-router";
import { connect } from "react-redux";
import MainElement from "./main-element";
import ListHeader from "./categoryHeader";
import { IPortalState } from '../interfaces/IPortalState';
import * as Scenarios from "../actions/scenarios";
import { StoreService } from '../services/StoreService';
import TopMenu from "./topMenu";
import * as Const from "../const/constants";
import ModalConfirmForm from "./modalConfirmForm";
import * as GuidService from "../services/GuidService";
import { AxiosResponse } from 'axios';
import { PermissionService } from "../services/PermissionService";
import { CategoryListQuery } from "../endpoints/CategoryListQuery";
import { IDropdownItem } from "../interfaces/IDropdownItem";
import { ICategoryDto } from "../interfaces/ICategoryDto";
import { ILearningDto } from "../interfaces/ILearningDto";
import { ILearningFilterDto } from "../interfaces/IHistoryFilterDto"
import { IStoreLearningRecordCommand } from "../interfaces/commands/IStoreLearningRecordCommand";
import { IDeleteLearningRecordCommand } from "../interfaces/commands/IDeleteLearningRecordCommand";
import { IRecalcLearningTokensCommand } from "../interfaces/commands/IRecalcLearningTokensCommand";
import { ILearnModelCommand } from "../interfaces/commands/ILearnModelCommand";
import GroupSubgroup from "./group-subgroup"
import ReactTable from "react-table";
import { Link } from "react-router-dom";
import { formatDateTime } from "../services/DateTimeHelpers";
import * as moment from 'moment';
import {
    Grid, Row, Col,
    DateField, Checkbox,
    Confirm, Select, MultiSelect, ButtonTextedWithIcon
} from "@sbt/react-ui-components";
import { TSMethodSignature } from "babel-types";

interface ILearningProps {
    learnings: ILearningDto[];
    pages: number;
    isLoading: boolean;
    loadingError?: string;
    filter: ILearningFilterDto;
}

interface ILearningState {
    gridState: any;
    pendingLoad: boolean;
    addingLearning?: boolean;
    changingLearning?: boolean;
    changingPhrase?: ILearningDto;
    changingCategoryName?: string;
    upperPartitionCaption?: string;
    partitionCaption?: string;
}

class LearningApp extends MainElement<ILearningProps, ILearningState>{
    state:ILearningState = {gridState: {}, pendingLoad: false};

    constructor(props) {
        super(props);

        this.onFetchData = this.onFetchData.bind(this);
    }

    componentDidMount()
    {
        //Scenarios.loadHistory({page: 0, pageSize: 20});
    }

    componentWillReceiveProps(nextProps: ILearningProps) {
        if (!nextProps.isLoading && this.state.pendingLoad) {
            this.setState({pendingLoad: false});
            Scenarios.loadLearnings(this.state.gridState, this.props.filter);
        }
    }

    isReadOnly = (): boolean => {
        return !PermissionService.Permissions.canEditChatBot;
    }

    onAddLearningClick = () => {
        this.setState({
            changingLearning: true,
            changingPhrase: {question: '', categoryId: this.props.filter.category ? this.props.filter.category.id : null},
            changingCategoryName: this.props.filter.category ? this.props.filter.category.title : null
        });
    }

    onSpellCheckClick = () => {
        let payload: IRecalcLearningTokensCommand = { isFullRecalc: false };
        Scenarios.doSpellCheck(payload);
    }

    learningEdit = (row: ILearningDto) => {
        this.setState({
            changingLearning: true,
            changingPhrase: {id: row.id, question: row.question, categoryId: row.categoryId},
            changingCategoryName: row.categoryName
        });
    }

    columns = [
        {
            id: 'index',
            Header: '',
            Cell: (row) => (
                <span>
                    <span 
                        className="material-icons material-icon-font" 
                        title="Изменить" 
                        style={{color: 'green', cursor: 'pointer'}}
                        onClick={() => {this.learningEdit(row.original);}}>
                        edit</span>
                    <span 
                        className="material-icons material-icon-font" 
                        title="Удалить" 
                        style={{color: 'red', cursor: 'pointer'}}
                        onClick={() => {this.learningDelete(row.original);}}>
                        delete</span>
                </span>
            ),
            filterable: false,
            width: 48
        }, {
            Header: 'Вопрос',
            accessor: 'question' 
        }, {
            Header: 'Категория',
            filterable: false,
            accessor: 'categoryName',
            Cell: (row) => (<span title={row.value}>{row.value}</span>)
        }
    ]

    learningDelete = (row: ILearningDto) => {
        var modal = StoreService.ms({ content: ModalConfirmForm, externalProps: { text: `Удалить фразу '${row.question}'?` } });
        var handler = modal.open();
        handler.close.then(val => this.onDeleteLearningConfirmation(val, row.id));
    }

    onDeleteLearningConfirmation = (confirmed: boolean, learningId: number): void => {
        if (!confirmed) return;
        let payload: IDeleteLearningRecordCommand = { learningId: learningId };
        Scenarios.deleteLearningsItem(payload, this.state.gridState, this.props.filter);
    }

    onFetchData = (state, instance) => {
        if (this.props.isLoading) {
            this.setState({gridState: state, pendingLoad: true});
        } else {
            Scenarios.loadLearnings(state, this.props.filter);
            this.setState({gridState: state});
        }
    }

    onSaveChangeLearning = (text: string) => {
        let payload: IStoreLearningRecordCommand = { learning: {
            id: this.state.changingPhrase ? this.state.changingPhrase.id : null,
            question: text,
            categoryId: this.state.changingPhrase.categoryId
        } };
        Scenarios.storeLearningItemFromLearnings(payload, this.state.gridState, this.props.filter);

        this.setState({
            changingLearning: false,
            addingLearning: false
        })
    }

    onCloseChangeLearning = () => {
        this.setState({
            changingLearning: false,
            addingLearning: false
        })
    }

    getCategorys = (search: string, skip: number, take: number, 
        callback: (items: IDropdownItem[]) => void): void => {

            let query = new CategoryListQuery(StoreService.auth, StoreService.servicesApi, "chatbot/collection");
            query.execute(skip, take, {
                filterCategory: search, 
                sorting: 'cat',
                partitionId: this.props.filter.upperPartitionId,
                subPartId: this.props.filter.partitionId
            }).then((response: AxiosResponse) => {
                    let items = response.data.items.map((it) => {return {id: it.originId, title: `${it.name} - ${it.partition.title}`}});
                    callback(items);
                })
                .catch((error: any) => {
                });
        
    }

    onCategorySelect = (item: IDropdownItem): void => {
        this.setState({
            changingPhrase: {...this.state.changingPhrase, categoryId: item ? item.id : null},
            changingCategoryName: item ? item.title : ''
        });
    }

    onFilterCategorySelect = (flt_chng: any): void => {
        var flt = {...this.props.filter, ...flt_chng};
        Scenarios.filterLearningChanged(flt);
        if (this.props.isLoading) 
            this.setState({pendingLoad: true});
        else
            Scenarios.loadLearnings(this.state.gridState, flt);
    }

    onSelectPartition = (grId: string, grCaption: string, subgrId: string, subgrCaption: string) => {
        this.setState({upperPartitionCaption: grCaption, partitionCaption: subgrCaption});
        this.onFilterCategorySelect({upperPartitionId: grId, partitionId: subgrId});
    }

    view() {

        return (
            <div className={'group-list-container wide'}>
                <TopMenu activeItemId={6}/>
                <div className="history-filters">
                    <Grid>
                        <GroupSubgroup 
                                onClick={(grId, grCaption, subgrId, subgrCaption) => this.onSelectPartition(grId, grCaption, subgrId, subgrCaption)} 
                                PartId={this.props.filter.upperPartitionId} 
                                SubpartId={this.props.filter.partitionId} 
                                PartCaption={this.state.upperPartitionCaption} 
                                SubpartCaption={this.state.partitionCaption}/>
                    <Row>
                        <Col baseSize={6} breakpointSizes={['md-6', 'sm-12']}>
                            <div>Категория</div>
                            <MultiSelect
                                    maxHeight={200}
                                    isRequired={false}
                                    onSelect={(e: IDropdownItem[]) => { this.onFilterCategorySelect({category: e}); }}
                                    query={(search: string, skip: number, take: number, callback: (items: IDropdownItem[]) => void): void =>
                                        this.getCategorys(search, skip, take, callback)}
                                    isDisabled={false}
                                    selectedItems={this.props.filter.category}
                                />
                        </Col>
                        <Col baseSize={2} breakpointSizes={['md-6', 'sm-12']}>
                        </Col>
                        <Col baseSize={2} breakpointSizes={['md-6', 'sm-12']}>
                            <div>&nbsp;</div>
                            <ButtonTextedWithIcon
                                title="SpellCheck"
                                onClick={this.onSpellCheckClick}
                                size="s"
                            />
                        </Col>
                        <Col baseSize={2} breakpointSizes={['md-6', 'sm-12']}>
                            <div>&nbsp;</div>
                            <ButtonTextedWithIcon
                                title="Добавить фразу"
                                onClick={this.onAddLearningClick}
                                size="s"
                            />
                        </Col>
                    </Row>
                </Grid>
                </div>
                <ReactTable
                    data={this.props.learnings}
                    pages={this.props.pages}
                    loading={this.props.isLoading}
                    columns={this.columns}
                    manual
                    filterable 
                    onFetchData={this.onFetchData}
                    defaultSorted={[{
                            id: "question",
                            desc: false
                        }]}
                    defaultPageSize={20}
                />
                {(this.state.changingLearning || this.state.addingLearning) &&
                    <div className="pattern-dialog">
                        <div className="history-dialog-select">
                            Категория:
                            <Select
                                maxHeight={200}
                                isRequired={false}
                                onSelect={(e: IDropdownItem) => { this.onCategorySelect(e); }}
                                query={(search: string, skip: number, take: number, callback: (items: IDropdownItem[]) => void): void =>
                                    this.getCategorys(search, skip, take, callback)}
                                isDisabled={false}
                                selectedItem={this.state.changingPhrase && this.state.changingPhrase.categoryId ? 
                                    {id: this.state.changingPhrase.categoryId, title: this.state.changingCategoryName} : null}
                            />
                        </div>
                        <Confirm
                            title="Пожалуйста, введите фразу"
                            buttonLabel="Отправить"
                            onSubmit={this.onSaveChangeLearning}
                            onClose={this.onCloseChangeLearning}
                            placeholder="Пожалуйста, введите фразу"
                            theme="default"
                            message={this.state.changingLearning ? this.state.changingPhrase.question : null}
                            preSetValue={this.state.changingLearning ? this.state.changingPhrase.question : null}
                        />
                    </div>
                }

            </div>
        );
    }
}

const mapStateToProps = (state: IPortalState, ownProps: any): ILearningProps => {
    return {
        learnings: state.learning.learnings,
        pages: state.learning.pages,
        isLoading: state.learning.isLoading,
        loadingError: state.learning.loadingError,
        filter: state.learning.filter
    };
};

export const LearningList: any = withRouter(connect(mapStateToProps)(LearningApp));