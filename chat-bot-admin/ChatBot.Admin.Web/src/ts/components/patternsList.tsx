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
import { IPatternsDto } from "../interfaces/IPatternsDto";
import { IPatternsFilterDto } from "../interfaces/IHistoryFilterDto"
import { IStorePatternCommand } from "../interfaces/commands/IStorePatternCommand";
import { IDeletePatternCommand } from "../interfaces/commands/IDeletePatternCommand";
import GroupSubgroup from "./group-subgroup"
import ReactTable from "react-table";
import { PatternEdit } from "./patternEdit";
import { FillWordFormsQuery, GetPatternQuery, PatternCalculateQuery } from "../endpoints/WordQueries"
import { IWordDto } from "../interfaces/IWordDto";
import { IPatternDto } from "../interfaces/IPatternDto";
import { formatDateTime } from "../services/DateTimeHelpers";
import * as StringHelpers from "../services/StringHelpers";
import * as moment from 'moment';
import {
    Grid, Row, Col,
    DateField, Checkbox, ButtonBox,
    Confirm, Select, MultiSelect, ButtonTextedWithIcon, ButtonIcon
} from "@sbt/react-ui-components";
import { TSMethodSignature } from "babel-types";

interface IPatternsProps {
    patternss: IPatternsDto[];
    pages: number;
    isLoading: boolean;
    loadingError?: string;
    filter: IPatternsFilterDto;
}

interface IPatternsState {
    gridState: any;
    pendingLoad: boolean;
    addingPatterns?: boolean;
    changingPatterns?: boolean;
    changingPattern?: boolean;
    changingPhrase?: IPatternsDto;
    changingCategoryName?: string;
    upperPartitionCaption?: string;
    partitionCaption?: string;
}

class PatternsApp extends MainElement<IPatternsProps, IPatternsState>{
    state:IPatternsState = {gridState: {}, pendingLoad: false};

    constructor(props) {
        super(props);

        this.onFetchData = this.onFetchData.bind(this);
    }

    componentDidMount()
    {
        //Scenarios.loadHistory({page: 0, pageSize: 20});
    }

    componentWillReceiveProps(nextProps: IPatternsProps) {
        if (!nextProps.isLoading && this.state.pendingLoad) {
            this.setState({pendingLoad: false});
            Scenarios.loadPatternss(this.state.gridState, this.props.filter);
        }
    }

    isReadOnly = (): boolean => {
        return !PermissionService.Permissions.canEditChatBot;
    }

    onAddPatternsClick = () => {
        this.setState({
            changingPatterns: true,
            changingPhrase: {
                id: 0,
                context: "", onlyContext: false, phrase: "", wordCount: 0, words: [],
                categoryId: +(this.props.filter.category ? this.props.filter.category.id : null)},
            changingCategoryName: this.props.filter.category ? this.props.filter.category.title : null
        });
    }

    patternsEdit = (row: IPatternsDto) => {
        let query = new GetPatternQuery(StoreService.auth, StoreService.servicesApi, `chatbot/pattern`);
        query.execute(row.id)
            .then((response: AxiosResponse) => {
                this.setState({
                    changingPatterns: true,
                    changingPhrase: response.data,
                    changingCategoryName: row.categoryName
                });
            })
            .catch((error: any) => {
                Scenarios.notifyError(error);
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
                        onClick={() => {this.patternsEdit(row.original);}}>
                        edit</span>
                </span>
            ),
            filterable: false,
            width: 48
        }, {
            Header: 'Паттерн',
            accessor: 'phrase' 
        }, {
            Header: 'Категория',
            filterable: false,
            accessor: 'categoryName',
            Cell: (row) => (<span title={row.value}><Link to={`/categoryedit/${row.original.categoryId}`}>{row.value}</Link></span>)
        }
    ]

    patternsDelete = (row: IPatternsDto) => {
        var modal = StoreService.ms({ content: ModalConfirmForm, externalProps: { text: `Удалить паттерн '${row.phrase.trim()}'?` } });
        var handler = modal.open();
        handler.close.then(val => this.onDeletePatternsConfirmation(val, row.id));
    }

    onDeletePatternsConfirmation = (confirmed: boolean, patternsId: number): void => {
        if (!confirmed) return;
        let payload: IDeletePatternCommand = { patternId: patternsId };
        Scenarios.deletePatternsItem(payload, this.state.gridState, this.props.filter);
        this.setState({
            changingPatterns: false,
            addingPatterns: false
        });
    }

    onFetchData = (state, instance) => {
        if (this.props.isLoading) {
            this.setState({gridState: state, pendingLoad: true});
        } else {
            Scenarios.loadPatternss(state, this.props.filter);
            this.setState({gridState: state});
        }
    }

    onCloseChangePatterns = () => {
        this.setState({
            changingPatterns: false,
            addingPatterns: false
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
                let items = response.data.items.map((it) => {return {
                        id: it.id, 
                        title: `${it.name} - ${(it.partition ? it.partition.title : '<нет>')}`}
                    });
                callback(items);
            })
            .catch((error: any) => {
                console.log('error getCategorys', error);
            });
        
    }

    onCategorySelect = (item: IDropdownItem): void => {
        this.setState({
            changingPhrase: {...this.state.changingPhrase, categoryId: +(item ? item.id : null)},
            changingCategoryName: item ? item.title : ''
        });
    }

    onFilterCategorySelect = (flt_chng: any): void => {
        var flt = {...this.props.filter, ...flt_chng};
        Scenarios.filterPatternsChanged(flt);
        if (this.props.isLoading) 
            this.setState({pendingLoad: true});
        else
            Scenarios.loadPatternss(this.state.gridState, flt);
    }

    onSelectPartition = (grId: string, grCaption: string, subgrId: string, subgrCaption: string) => {
        this.setState({upperPartitionCaption: grCaption, partitionCaption: subgrCaption});
        this.onFilterCategorySelect({upperPartitionId: grId, partitionId: subgrId});
    }

    onCloseEdit = () => {
        this.setState({changingPatterns: false, addingPatterns: false});
    }


    onWordTypeSelect = (e: IDropdownItem, word: IWordDto) => {
        word.wordTypeId = e ? +e.id : -1;
        let query = new FillWordFormsQuery(StoreService.auth, StoreService.servicesApi, "chatbotwords/word/forms");
        query.execute(word)
            .then((response: AxiosResponse) => {
                var pattern = this.state.changingPhrase;
                if (pattern) {
                    var wrd = pattern.words.filter(x => x.id == word.id);
                    if (wrd && wrd.length > 0) {
                        var wordInd = pattern.words.indexOf(wrd[0]);
                        pattern.words[wordInd] = response.data;
                        this.setState({
                            changingPhrase: {...pattern}
                        });
                    }
                }
            })
            .catch((error: any) => {
                Scenarios.notifyError(error);
            });
    }

    onPatternContextClear = (): void => {
        var pattern = this.state.changingPhrase;
        if (pattern) {
            pattern.context = '';
            this.setState({
                changingPhrase: {...pattern}
            });
        }
    }

    onPatternContextFocusOut = (value: string) => {

        value = StringHelpers.normalize(value);
        var pattern = this.state.changingPhrase;
        if (pattern) {

            if (value === pattern.context)
                return;

            pattern.context = value;
            this.setState({
                changingPhrase: {...pattern}
            });
        }
    }


    onPatternOnlyContextChanged = (value: any) => {
        var pattern = this.state.changingPhrase;
        if (pattern) {
            pattern.onlyContext = value.checked;
            this.setState({
                changingPhrase: {...pattern}
            });
        }
    }

    onPatternSelect = () => {
    }

    onChangePatternClick = () => {
        this.setState({
            changingPattern: true
        })
    }

    onCloseChangePattern = () => {
        this.setState({
            changingPattern: false
        })
    }

    onSaveChangePattern = (text: string) => {
        var pattern: IPatternDto = null;
        if (this.state.changingPattern) {
            pattern = this.state.changingPhrase;
            pattern.phrase = text.trim();
            if (!pattern.categoryId) pattern.categoryId = 0;
        }
        if (pattern) {
            let query = new PatternCalculateQuery(StoreService.auth, StoreService.servicesApi, "chatbotwords/pattern/calculate");
            query.execute(pattern)
                .then((response: AxiosResponse) => {
                    if (this.state.changingPattern) {
                        pattern = this.state.changingPhrase;
                    }
                    if (pattern) {
                        pattern.id = response.data.id;
                        pattern.phrase = response.data.phrase.trim();
                        pattern.wordCount = response.data.wordCount;
                        pattern.words = response.data.words;
                        var cnt = 0;
                        pattern.words.forEach((word: IWordDto) => {
                            word.wordTypeId = word.wordTypeId == null ? -1 : word.wordTypeId;
                            word.id = word.id ? word.id : --cnt;
                        });
                        this.setState({
                            changingPhrase: {...pattern},
                            changingPattern: false
                        });
                    }
                })
                .catch((error: any) => {
                    Scenarios.notifyError(error);
                });
        }
    }

    onSavePatternClick = () => {
        let payload: IStorePatternCommand = { pattern: this.state.changingPhrase };
        Scenarios.storePatternsItemFromPatterns(payload, this.state.gridState, this.props.filter);

        this.setState({
            changingPatterns: false,
            addingPatterns: false
        })

    }

    onDeletePatternClick = () => {
        this.patternsDelete(this.state.changingPhrase);
    }

    view() {
        return (
            <div className={'group-list-container wide'}>
                <TopMenu activeItemId={5}/>
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
                        </Col>
                        <Col baseSize={2} breakpointSizes={['md-6', 'sm-12']}>
                            <div>&nbsp;</div>
                            <ButtonTextedWithIcon
                                title="Добавить фразу"
                                onClick={this.onAddPatternsClick}
                                size="s"
                            />
                        </Col>
                    </Row>
                </Grid>
                </div>
                <ReactTable
                    data={this.props.patternss}
                    pages={this.props.pages}
                    loading={this.props.isLoading}
                    columns={this.columns}
                    manual
                    filterable 
                    onFetchData={this.onFetchData}
                    defaultSorted={[{
                            id: "phrase",
                            desc: false
                        }]}
                    defaultPageSize={20}
                />
                {(this.state.changingPatterns || this.state.addingPatterns) &&
                    <div className="pattern-edit-dialog">
                        <div className="pattern-dialog-select">
                            Категория:
                            <div style={{'float': 'right'}}>
                                <ButtonIcon
                                    iconCode="clear"
                                    onClick={this.onCloseEdit}
                                    size={24}
                                    color="#000000"
                                />
                            </div>
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
                        <PatternEdit 
                            pattern={this.state.changingPhrase} isReadOnly={this.isReadOnly()} 
                            isOpen={true}
                            onWordTypeSelect={this.onWordTypeSelect}
                            onPatternContextClear={this.onPatternContextClear}
                            onPatternContextFocusOut={this.onPatternContextFocusOut}
                            onPatternOnlyContextChanged={this.onPatternOnlyContextChanged}
                            onChangePatternClick={this.onChangePatternClick}
                            onDeletePatternClick={this.onDeletePatternClick}
                            onPatternSelect={this.onPatternSelect}
                        />
                        <div className="uavp-confirm__footer">
                            <ButtonBox
                                title={'Сохранить'}
                                onClick={this.onSavePatternClick}
                                isDisabled={!this.state.changingPhrase.categoryId || !this.state.changingPhrase.phrase.trim()}
                                theme="default"
                                size="s"
                            />
                        </div>
                    </div>
                }
                {(this.state.changingPattern) &&
                    <div className="pattern-dialog">
                        <Confirm
                            title="Пожалуйста, введите паттерн"
                            buttonLabel="Отправить"
                            onSubmit={this.onSaveChangePattern}
                            onClose={this.onCloseChangePattern}
                            placeholder="Пожалуйста, введите паттерн"
                            theme="default"
                            message={this.state.changingPattern ? this.state.changingPhrase.phrase.trim() : null}
                            preSetValue={this.state.changingPattern ? this.state.changingPhrase.phrase.trim() : null}
                        />
                    </div>
                }

            </div>
        );
    }
}

const mapStateToProps = (state: IPortalState, ownProps: any): IPatternsProps => {
    return {
        patternss: state.patterns.patterns,
        pages: state.patterns.pages,
        isLoading: state.patterns.isLoading,
        loadingError: state.patterns.loadingError,
        filter: state.patterns.filter
    };
};

export const PatternsList: any = withRouter(connect(mapStateToProps)(PatternsApp));