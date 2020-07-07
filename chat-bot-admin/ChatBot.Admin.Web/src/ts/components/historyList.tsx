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
import { IHistoryDto } from "../interfaces/IHistoryDto"
import { IHistoryFilterDto } from "../interfaces/IHistoryFilterDto"
import { ILearningDto } from "../interfaces/ILearningDto";
import { IStoreLearningRecordCommand } from "../interfaces/commands/IStoreLearningRecordCommand";
import { IDeleteLearningRecordCommand } from "../interfaces/commands/IDeleteLearningRecordCommand";
import ReactTable from "react-table";
import { formatDateTime } from "../services/DateTimeHelpers";
import * as moment from 'moment';
import {
    DateField,
    Checkbox,
    Confirm,
    Select, MultiSelect
} from "@sbt/react-ui-components";
import { TSMethodSignature } from "babel-types";

interface IHistoryProps {
    history: IHistoryDto[];
    pages: number;
    isLoading: boolean;
    loadingError?: string;
    filter: IHistoryFilterDto;
}

interface IHistoryState {
    gridState: any;
    pendingLoad: boolean;
    addingLearning?: boolean;
    changingLearning?: boolean;
    changingPhrase?: ILearningDto;
    changingCategoryName?: string;
}

class HistoryApp extends MainElement<IHistoryProps, IHistoryState>{

    constructor(props) {
        super(props);

        this.onFetchData = this.onFetchData.bind(this);
        this.waitProps = this.waitProps.bind(this);
        this.dateFromChanged = this.dateFromChanged.bind(this);
        this.dateToChanged = this.dateToChanged.bind(this);
        this.onFilterCheckChanged = this.onFilterCheckChanged.bind(this);
    }

    componentDidMount()
    {
        //Scenarios.loadHistory({page: 0, pageSize: 20});
    }

    componentWillReceiveProps(nextProps: IHistoryProps) {
        if (!nextProps.isLoading && this.state.pendingLoad) {
            this.setState({pendingLoad: false});
            Scenarios.loadHistory(this.state.gridState, this.props.filter);
        }
    }

    isReadOnly = (): boolean => {
        return !PermissionService.Permissions.canEditChatBot;
    }

    hintedRow = (row) => 
        (<span style={{color: this.getColor(row)}} title={row.value}>{row.value}</span>);

    learningAdd = (row: IHistoryDto) => {
        this.setState({
            changingLearning: true,
            changingPhrase: {question: row.question, categoryId: row.categoryOriginId},
            changingCategoryName: row.answer
        });
    }

    learningEdit = (row: IHistoryDto) => {
        this.setState({
            changingLearning: true,
            changingPhrase: {id: row.learns[0].id, question: row.question, categoryId: row.learns[0].categoryId},
            changingCategoryName: row.learns[0].categoryName
        });
    }

    waitProps = () => {
        this.setState({pendingLoad: true});
    }

    getCategoryNames = (row:IHistoryDto): string => {
        if (!row.learns || row.learns.length == 0) return undefined;
        var res = '';
        row.learns.forEach((r) => {
            if (r.categoryName) {
                res += `${r.categoryName}; `
            }
        });
        if (res) res = res.substring(0, res.length - 2);
        return res;
    }

    columns = [
        {
            Header: 'Источник',
            accessor: 'source',
            width: 48
        }, {
            id: 'index',
            Header: '',
            Cell: (row) => (
                <span>
                    { row.original.like && <span className="material-icons material-icon-font"
                        style={{color: (row.original.like > 0 ? "green" : "red")}}>
                        {row.original.like > 0 ? 'thumb_up' : 'thumb_down'}
                    </span>}
                    <span 
                        title={ this.getCategoryNames(row.original) } 
                        style={{
                            width: '10px',
                            color: 
                                row.original.answerGood && this.getCategoryNames(row.original) ? "green" : "red"
                            }}>
                        {row.original.learns && row.original.learns.length > 0 ? 
                            this.getCategoryNames(row.original) ? 
                                !row.original.categoryOriginId || row.original.answerGood
                                    ? ' * ' : ' ? ' : ' * ' : ''}
                        {!row.original.learns || row.original.learns.length == 0 &&
                            <span 
                                className="material-icons material-icon-font" 
                                title="Добавить" 
                                style={{color: 'green', cursor: 'pointer'}}
                                onClick={() => {this.learningAdd(row.original);}}>
                                add</span>
                        }
                        {row.original.learns && row.original.learns.length == 1 &&
                            <span 
                                className="material-icons material-icon-font" 
                                title="Изменить" 
                                style={{
                                    color: 
                                        row.original.answerGood ? "green" : "red", 
                                    cursor: 'pointer'}}
                                onClick={() => {this.learningEdit(row.original);}}>
                                edit</span>
                        }
                    </span>
                </span>
            ),
            filterable: false,
            width: 64
        }, {
            Header: 'Пороги ML',
            accessor: 'mtoThresholds',
            Cell: this.hintedRow,
            width: 100
        }, {
            id: 'questionDate',
            Header: 'Дата/Время',
            accessor: d => formatDateTime(d.questionDate, true),
            filterable: false,
            width: 150
        }, {
            Header: 'Логин',
            accessor: 'userName' 
        }, {
            Header: 'Вопрос',
            accessor: 'originalQuestion',
            Cell: this.hintedRow
        }, {
            Header: 'Ответ',
            accessor: 'answer',
            Cell: this.hintedRow
        }, {
            Header: 'Текст ответа',
            accessor: 'answerText',
            Cell: this.hintedRow
        }
    ]

    onFetchData = (state, instance) => {
        if (this.props.isLoading) {
            this.setState({gridState: state, pendingLoad: true});
        } else {
            Scenarios.loadHistory(state, this.props.filter);
            this.setState({gridState: state});
        }
    }

    dateFromChanged = (date: any) => {
        var flt = this.props.filter;
        flt.from = this.getStringDate(date);
        Scenarios.filterChanged(flt);
        if (this.props.isLoading) 
            this.waitProps();
        else {
            Scenarios.loadHistory(this.state.gridState, flt);
        }
    }

    dateToChanged = (date: any) => {
        var flt = this.props.filter;
        flt.to = this.getStringDate(date);
        Scenarios.filterChanged(flt);
        if (this.props.isLoading) 
            this.waitProps();
        else {
            Scenarios.loadHistory(this.state.gridState, flt);
        }
    }

    onFilterCheckChanged = (flt_chng: any) => {
        var flt = {...this.props.filter, ...flt_chng};
        Scenarios.filterChanged(flt);
        if (this.props.isLoading) 
            this.waitProps();
        else {
            Scenarios.loadHistory(this.state.gridState, flt);
        }
    }

	getDateString = (date) => { 
        return date && date.isValid() ? moment(date, 'DD.MM.YYYY').format('DD.MM.YYYY') : null; 
    }

	getStringDate = (date) => { 
        return date ? moment(moment(date).format('DD.MM.YYYY'), 'DD.MM.YYYY') : null; 
    }

    getTrProps = (state, rowInfo, column) => {
        return {
          style: {
            background: rowInfo ?
                rowInfo.original.categoryOriginId ? '#eeffee' :
                !rowInfo.original.rate && !rowInfo.original.mtoThresholds ? '#ffeeee' :
                '#eeeeee' : '#eeeeee',
            fontWeight: rowInfo && rowInfo.original.learns && rowInfo.original.learns.length > 0 ? "bold" : undefined
          }
        };
    }

    getColor = (rowInfo) =>  (rowInfo ? 
        rowInfo.original.isButton ? '#888888' :
        rowInfo.original.isMto ? '#008800' :
        'black' : 'black');

    getTdProps = (state, rowInfo, column, instance) => {
        return {
          style: {
            color: this.getColor(rowInfo)
          }
        };
    }

    onSaveChangeLearning = (text: string) => {
        let payload: IStoreLearningRecordCommand = { learning: {
            id: this.state.changingPhrase ? this.state.changingPhrase.id : null,
            question: text,
            categoryId: this.state.changingPhrase.categoryId
        } };
        Scenarios.storeLearningItemFromHistory(payload, this.state.gridState, this.props.filter);

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
            query.execute(skip, take, {filterCategory: search, sorting: 'cat'})
                .then((response: AxiosResponse) => {
                    let items = response.data.items.map((it) => {return {id: it.originId, title: `${it.name} - ${it.partition.title}`}});
                    callback(items);
                })
                .catch((error: any) => {
                });
        
    }

    onCategorySelect = (item: IDropdownItem): void => {
        this.setState({
            changingPhrase: {...this.state.changingPhrase, categoryId: item ? item.id : null},
            changingCategoryName: item.title
        });
    }


    view() {

        return (
            <div className={'group-list-container wide'}>
                <TopMenu activeItemId={4}/>
                <div className="history-filters">
                    <div className="history-filters-cell">
                        <div>Интервал</div>
                        <div className="link-admin__period">
                            <div className='link-admin__period-date'>
                                <DateField value={this.getDateString(this.props.filter.from)} onSelect={this.dateFromChanged}/>
                            </div>
                            <div className="link-admin__period-separator">-</div>
                            <div className='link-admin__period-date'>
                                <DateField value={this.getDateString(this.props.filter.to)} onSelect={this.dateToChanged}/>
                            </div>
                        </div>
                    </div>
                    <div className="history-filters-cell">
                        <div className="history-answered-ml"><Checkbox id='cb-answered-ml' isChecked={this.props.filter.isAnsweredMl} 
                            onChange={(value: any) => this.onFilterCheckChanged({isAnsweredMl: value.checked})} title='Отвеченные ML'></Checkbox></div>
                        <div className="history-answered-eve"><Checkbox id='cb-answered-eve' isChecked={this.props.filter.isAnsweredEve} 
                            onChange={(value: any) => this.onFilterCheckChanged({isAnsweredEve: value.checked})} title='Отвеченные алгоритмом'></Checkbox></div>
                        <div className="history-answered-button"><Checkbox id='cb-answered-button' isChecked={this.props.filter.isAnsweredButton} 
                            onChange={(value: any) => this.onFilterCheckChanged({isAnsweredButton: value.checked})} title='Отвеченные по кнопке'></Checkbox></div>
                        <div className="history-answered-no"><Checkbox id='cb-answered-no' isChecked={this.props.filter.isAnsweredNo} 
                            onChange={(value: any) => this.onFilterCheckChanged({isAnsweredNo: value.checked})} title='Не отвеченные'></Checkbox></div>
                        <div className="history-answered-other"><Checkbox id='cb-answered-other' isChecked={this.props.filter.isAnsweredOther} 
                            onChange={(value: any) => this.onFilterCheckChanged({isAnsweredOther: value.checked})} title='Прочие'></Checkbox></div>
                    </div>
                    <div className="history-filters-cell">
                        <div className="history-liked"><Checkbox id='cb-liked' isChecked={this.props.filter.isLikeYes} 
                            onChange={(value: any) => this.onFilterCheckChanged({isLikeYes: value.checked})} title='С лайками'></Checkbox></div>
                        <div className="history-disliked"><Checkbox id='cb-disliked' isChecked={this.props.filter.isDisLike} 
                            onChange={(value: any) => this.onFilterCheckChanged({isDisLike: value.checked})} title='С дизлайками'></Checkbox></div>
                        <div className="history-not-liked"><Checkbox id='cb-not-liked' isChecked={this.props.filter.isLikeNo} 
                            onChange={(value: any) => this.onFilterCheckChanged({isLikeNo: value.checked})} title='Без лайков'></Checkbox></div>
                    </div>
                    <div className="history-filters-cell">
                        <div className="history-ml-yes"><Checkbox id='cb-ml-yes' isChecked={this.props.filter.isMlYes} 
                            onChange={(value: any) => this.onFilterCheckChanged({isMlYes: value.checked})} title='(*) В выборке без ответа'></Checkbox></div>
                        <div className="history-ml-answer"><Checkbox id='cb-ml-answer' isChecked={this.props.filter.isMlAnswer} 
                            onChange={(value: any) => this.onFilterCheckChanged({isMlAnswer: value.checked})} title='(*) В выборке с ответом'></Checkbox></div>
                        <div className="history-ml-wrong"><Checkbox id='cb-ml-wrong' isChecked={this.props.filter.isMlWrong} 
                            onChange={(value: any) => this.onFilterCheckChanged({isMlWrong: value.checked})} title='(?) Ответ не такой как в выборке'></Checkbox></div>
                        <div className="history-ml-no"><Checkbox id='cb-ml-no' isChecked={this.props.filter.isMlNo} 
                            onChange={(value: any) => this.onFilterCheckChanged({isMlNo: value.checked})} title='Не в выборке'></Checkbox></div>
                    </div>
                </div>
                <ReactTable
                    data={this.props.history}
                    pages={this.props.pages}
                    loading={this.props.isLoading}
                    columns={this.columns}
                    manual
                    filterable 
                    onFetchData={this.onFetchData}
                    getTrProps={this.getTrProps}
                    getTdProps={this.getTdProps}
                    defaultSorted={[
                        {
                            id: "questionDate",
                            desc: true
                        }
                        ]}
                    defaultPageSize={20}
                />
                {this.state && (this.state.changingLearning || this.state.addingLearning) &&
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
                                selectedItem={this.state.changingPhrase.categoryId ? 
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

const mapStateToProps = (state: IPortalState, ownProps: any): IHistoryProps => {
    return {
        history: state.history.history,
        pages: state.history.pages,
        isLoading: state.history.isLoading,
        loadingError: state.history.loadingError,
        filter: state.history.filter
    };
};

export const HistoryList: any = withRouter(connect(mapStateToProps)(HistoryApp));