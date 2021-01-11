import * as React from "react";
import { withRouter } from "react-router";
import { Link } from "react-router-dom";
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
import { IModelDto, IModelReportDto } from "../interfaces/IModelDto";
import { IModelFilterDto } from "../interfaces/IHistoryFilterDto"
import { IModelReportFilterDto } from "../interfaces/IHistoryFilterDto"
import { ILearnModelCommand } from "../interfaces/commands/ILearnModelCommand";
import { IPublishModelCommand } from "../interfaces/commands/IPublishModelCommand";
import GroupSubgroup from "./group-subgroup"
import ReactTable from "react-table";
import { formatDateTime } from "../services/DateTimeHelpers";
import * as moment from 'moment';
import {
    Grid, Row, Col,
    DateField, Checkbox,
    Confirm, Select, MultiSelect, ButtonTextedWithIcon
} from "@sbt/react-ui-components";
import { TSMethodSignature } from "babel-types";

interface IModelProps {
    models: IModelDto[];
    pages: number;
    isLoading: boolean;
    loadingError?: string;
    filter: IModelFilterDto;
}

interface IModelState {
    gridState: any;
    pendingLoad: boolean;
    addingModel?: boolean;
    filterReport: IModelReportFilterDto;
    selected: IModelDto;
    selectedReport: IModelReportDto;
    filtered: IModelReportDto[];
    minF1?: number;
    maxF1?: number;
    minF1Model?: number;
    maxF1Model?: number;
}

class ModelApp extends MainElement<IModelProps, IModelState>{
    state:IModelState = {gridState: {}, pendingLoad: false, filterReport: {}, selected: null, selectedReport: null, filtered: null};

    constructor(props) {
        super(props);

        this.onFetchData = this.onFetchData.bind(this);
    }

    componentDidMount()
    {
        //Scenarios.loadHistory({page: 0, pageSize: 20});
    }

    componentWillReceiveProps(nextProps: IModelProps) {
        if (!nextProps.isLoading && this.state.pendingLoad) {
            this.setState({pendingLoad: false});
            Scenarios.loadModels(this.state.gridState, this.props.filter);
        } else {
            let newState = {minF1Model: null, maxF1Model: null};
            if (nextProps.models && nextProps.models.length > 0) {
                let minF1 = 1.;
                let maxF1 = 0.;
                nextProps.models.forEach((d) => {
                    if (d.f1) {
                        if (minF1 > d.f1) minF1 = d.f1;
                        if (maxF1 < d.f1) maxF1 = d.f1;
                    }
                });
                if (minF1 < maxF1) {
                    newState = {minF1Model: minF1, maxF1Model: maxF1};
                }
            }
            this.setState(newState);
        }
    }

    isReadOnly = (): boolean => {
        return !PermissionService.Permissions.canEditChatBot;
    }

    onModelLearnClick = () => {
        let payload: ILearnModelCommand = { modelCommand: 'start' };
        Scenarios.doLearnModel(payload, this.state.gridState, this.props.filter);
    }

    modelPublish = (row: IModelDto) => {
        var modal = StoreService.ms({content: ModalConfirmForm, externalProps: {text: "Опубликовать модель?"}});
        var handler = modal.open();
        handler.close.then(val => {
            if (val) {
                let payload: IPublishModelCommand = { modelCommand: 'publish', modelId: row.id };
                Scenarios.doPublishModel(payload, this.state.gridState, this.props.filter);
            }
        });
    }

    columns = [
        {
            id: 'index',
            Header: '',
            Cell: (row) => (
                <span>
                    {row.original.answerDate && <span 
                        className="material-icons material-icon-font" 
                        title="Опубликовать модель" 
                        style={{color: 'green', cursor: 'pointer'}}
                        onClick={() => {this.modelPublish(row.original);}}>
                        launch</span>}
                </span>
            ),
            filterable: false,
            width: 24
        }, {
            Header: 'Дата старта',
            accessor: d => formatDateTime(d.createDate, true),
            id: 'createDate' 
        }, {
            Header: 'Дата завершения',
            accessor: d => formatDateTime(d.answerDate, true),
            id: 'answerDate' 
        }, {
            Header: 'Вопросов',
            accessor: 'markup' 
        }, {
            Header: 'Доля правильных',
            accessor: 'accuracy' 
        }, {
            Header: 'Точность',
            accessor: 'precision' 
        }, {
            Header: 'Полнота',
            accessor: 'recall' 
        }, {
            Header: 'F1',
            accessor: 'f1' 
        }
    ]

    columnsResult = [
        {
            Header: 'Категория',
            accessor: 'categoryName',
            minWidth: 400
        }, {
            Header: 'Вопросов',
            accessor: 'markup' 
        }, {
            Header: 'Точность',
            accessor: 'precision' 
        }, {
            Header: 'Полнота',
            accessor: 'recall' 
        }, {
            Header: 'F1',
            accessor: 'f1' 
        }
    ]

    getFilteredReport = (data: IModelReportDto[], filter: IModelReportFilterDto):IModelReportDto[] => {
        if (!data || data.length == 0) return null;
        let res = [...data];
        if (filter.category && filter.category.length) {
            res = res.filter((x) => filter.category.some(elem => elem.id == x.categoryId));
        }
        if (filter.partitionId) {
            res = res.filter((x) => x.partitionId == filter.partitionId);
        }
        if (filter.upperPartitionId) {
            res = res.filter((x) => x.upperPartitionId == filter.upperPartitionId);
        }
        return res;
    }

    calcMinMax = (data: IModelReportDto[]):any => {
        if (!data || data.length == 0)
            return {minF1: null, maxF1: null};
        let minF1 = 1.;
        let maxF1 = 0.;
        data.forEach((d) => {
            if (d.f1) {
                if (minF1 > d.f1) minF1 = d.f1;
                if (maxF1 < d.f1) maxF1 = d.f1;
            }
        });
        if (minF1 < maxF1) return {minF1: minF1, maxF1: maxF1};
        return {minF1: null, maxF1: null};
    }

    onGetTrProps = (state, rowInfo) => {
        if (rowInfo && rowInfo.row) {
            let isSelected = !!(this.state.selected && rowInfo.original.id === this.state.selected.id); 
            let newStyle = {
                background: isSelected ? '#00afec' : 'white',
                color: isSelected ? 'white' : 'black',
                fontWeight: 'normal'
            }
            if (!isSelected && this.state.minF1Model && rowInfo.row.f1) {
                let g = Math.round(224 + 32 * (rowInfo.row.f1 - this.state.minF1Model) / (this.state.maxF1Model - this.state.minF1Model));
                let r = Math.round(224 + 32 * (rowInfo.row.f1 - this.state.maxF1Model) / (this.state.minF1Model - this.state.maxF1Model));
                newStyle.background = `rgb(${r},${g},224)`;
            }
            if (rowInfo.original.isActive) {
                newStyle.fontWeight = 'bold';
            }
          return {
            onClick: (e) => {
                let newState = this.calcMinMax(rowInfo.original.report);
                newState.selected = rowInfo.original;
                newState.selectedReport = null;
                newState.filtered = this.getFilteredReport(rowInfo.original.report, this.state.filterReport)
                this.setState(newState);
            },
            style: newStyle
          }
        } else {
          return {}
        }
    }

    onGetTrPropsReport = (state, rowInfo) => {
        if (rowInfo && rowInfo.row) {
            let isSelected = !!(this.state.selectedReport && rowInfo.original.categoryId === this.state.selectedReport.categoryId); 
            let newStyle = {
                background: isSelected ? '#00afec' : 'white',
                color: isSelected ? 'white' : 'black',
                fontWeight: 'normal'
            }
            if (!isSelected) {
                if (!this.state.minF1 || !rowInfo.row.f1) {
                    newStyle.background = '#eee';
                } else {
                    let g = Math.round(224 + 32 * (rowInfo.row.f1 - this.state.minF1) / (this.state.maxF1 - this.state.minF1));
                    let r = Math.round(224 + 32 * (rowInfo.row.f1 - this.state.maxF1) / (this.state.minF1 - this.state.maxF1));
                    newStyle.background = `rgb(${r},${g},224)`;
                }
            }
            return { 
                onClick: (e) => {
                    let newState = {selectedReport: rowInfo.original};
                    this.setState(newState);
                },
                style: newStyle 
            };
        } else {
            return {}
        }
    }

    onFetchData = (state, instance) => {
        if (this.props.isLoading) {
            this.setState({gridState: state, pendingLoad: true});
        } else {
            Scenarios.loadModels(state, this.props.filter);
            this.setState({gridState: state});
        }
    }

    getCategorys = (search: string, skip: number, take: number, 
        callback: (items: IDropdownItem[]) => void): void => {

            let query = new CategoryListQuery(StoreService.auth, StoreService.servicesApi, "chatbot/collection");
            query.execute(skip, take, {
                filterCategory: search, 
                sorting: 'cat',
                partitionId: this.state.filterReport.upperPartitionId,
                subPartId: this.state.filterReport.partitionId
            }).then((response: AxiosResponse) => {
                    let items = response.data.items.map((it) => {return {id: it.originId, title: `${it.name} - ${it.partition.title}`}});
                    callback(items);
                })
                .catch((error: any) => {
                });
        
    }

    onFilterCategorySelect = (flt_chng: any): void => {
        var flt = {...this.state.filterReport, ...flt_chng};
        let newState = {
            filterReport: flt,
            filtered: null
        };
        if (this.isHasResult()) {
            newState.filtered = this.getFilteredReport(this.state.selected.report, flt);
        }
        this.setState(newState);
    }

    onSelectPartition = (grId: string, grCaption: string, subgrId: string, subgrCaption: string) => {
        this.onFilterCategorySelect({upperPartitionId: grId, partitionId: subgrId, upperPartitionCaption: grCaption, partitionCaption: subgrCaption});
    }

    isHasResult = () => {
        return this.state.selected && this.state.selected.report && this.state.selected.report.length > 0;
    }

    openCategory = (conf) => {
        StoreService.history.push(Const.NavigationPathCategoryItemEdit.replace(":id", conf.categoryId.toString()));
    }

    filterCaseInsensitive = (filter, row) => {
        const id = filter.pivotId || filter.id;
        if (row[id] !== null){
            return (
                row[id] !== undefined ?
                    String(row[id].toLowerCase()).startsWith(filter.value.toLowerCase())
                    :
                    true
            );
        }
    }

    view() {

        return (
            <div className={'group-list-container wide'}>
                <TopMenu activeItemId={7}/>
                <div className="history-filters">
                    <Grid>
                    <Row>
                        <Col baseSize={2} breakpointSizes={['md-6', 'sm-12']}>
                            <div>&nbsp;</div>
                            <ButtonTextedWithIcon
                                title="Запустить обучение модели"
                                onClick={this.onModelLearnClick}
                                size="s"
                            />
                        </Col>
                    </Row>
                </Grid>
                </div>
                <ReactTable
                    data={this.props.models}
                    pages={this.props.pages}
                    loading={this.props.isLoading}
                    columns={this.columns}
                    getTrProps={this.onGetTrProps}
                    manual
                    onFetchData={this.onFetchData}
                    defaultSorted={[{
                            id: "createDate",
                            desc: true
                        }]}
                    defaultPageSize={5}
                />
                {this. isHasResult() && <div className="history-filters">
                    <Grid>
                        <Row><Col>&nbsp;</Col></Row>
                        {this.state.selectedReport && <Row>
                            <Col baseSize={3} breakpointSizes={['md-6', 'sm-12']}>
                                Размытие ИЗ:
                            </Col>
                            <Col baseSize={9} breakpointSizes={['md-6', 'sm-12']}>
                                <Row>
                                    {this.state.selectedReport.confusionFrom.map((conf, ind) => {
                                        let hint = ''; 
                                        conf.questions.forEach((q) => hint += `\n${q}`);
                                        return <Col key={`confusion-from-${ind}`} baseSize={1}>
                                            <span title={'<' + (conf.categoryName || conf.originId || 'Нет')+ '>' + hint} 
                                                style={{'background': ind ? 'rgb(255, 224, 224)' : 'rgb(224, 255, 224)', 'textAlign': 'center' }}>
                                                {conf.categoryId ?
                                                    <a target="_blank" href={`${Const.NavigationPathCategoryItemEdit.replace(":id", conf.categoryId.toString())}`}>
                                                        {conf.confusion}
                                                    </a> :
                                                    conf.confusion
                                                }
                                            </span>
                                        </Col>}
                                    )}
                                </Row>
                            </Col>
                        </Row>}
                        {this.state.selectedReport && <Row>
                            <Col baseSize={3} breakpointSizes={['md-6', 'sm-12']}>
                                Размытие В:
                            </Col>
                            <Col baseSize={9} breakpointSizes={['md-6', 'sm-12']}>
                            <Row>
                                    {this.state.selectedReport.confusionTo.map((conf, ind) => {
                                        let hint = ''; 
                                        conf.questions.forEach((q) => hint += `\n${q}`);
                                        return <Col key={`confusion-to-${ind}`} baseSize={1}>
                                            <span title={'<' + (conf.categoryName || conf.originId || 'Нет')+ '>' + hint} 
                                                style={{'background': ind ? 'rgb(255, 224, 224)' : 'rgb(224, 255, 224)', 'textAlign': 'center' }}>
                                                {conf.categoryId ?
                                                    <a target="_blank" href={`${Const.NavigationPathCategoryItemEdit.replace(":id", conf.categoryId.toString())}`}>
                                                        {conf.confusion}
                                                    </a> :
                                                    conf.confusion
                                                }
                                            </span>
                                        </Col>}
                                    )}
                                </Row>
                            </Col>
                        </Row>}
                        <Row><Col>&nbsp;</Col></Row>
                        <GroupSubgroup 
                                onClick={(grId, grCaption, subgrId, subgrCaption) => this.onSelectPartition(grId, grCaption, subgrId, subgrCaption)} 
                                PartId={this.state.filterReport.upperPartitionId} 
                                SubpartId={this.state.filterReport.partitionId} 
                                PartCaption={this.state.filterReport.upperPartitionCaption} 
                                SubpartCaption={this.state.filterReport.partitionCaption}/>
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
                                        selectedItems={this.state.filterReport.category}
                                    />
                            </Col>
                        </Row>
                    </Grid>
                </div>}
                {this. isHasResult() && 
                    <ReactTable
                        data={this.state.filtered}
                        columns={this.columnsResult}
                        getTrProps={this.onGetTrPropsReport}
                        filterable
                        defaultSorted={[{
                                id: "categoryName",
                                desc: false
                            }]}
                        defaultPageSize={10}
                        defaultFilterMethod={this.filterCaseInsensitive}
                        />
                }
            </div>
        );
    }
}

const mapStateToProps = (state: IPortalState, ownProps: any): IModelProps => {
    return {
        models: state.model.models,
        pages: state.model.pages,
        isLoading: state.model.isLoading,
        loadingError: state.model.loadingError,
        filter: state.model.filter
    };
};

export const ModelList: any = withRouter(connect(mapStateToProps)(ModelApp));