import * as React from "react";
import { withRouter } from "react-router";
import { connect } from "react-redux";
import { IPortalState } from "../interfaces/IPortalState";
import { ICategoryDto } from "../interfaces/ICategoryDto";
import { IPatternDto } from "../interfaces/IPatternDto";
import { IWordDto } from "../interfaces/IWordDto";
import { ILearningDto } from "../interfaces/ILearningDto";
import { IDropdownItem } from "../interfaces/IDropdownItem";
import { FillWordFormsQuery, PatternCalculateQuery } from "../endpoints/WordQueries"
import { IEditCategoryCommand } from "../interfaces/commands/IEditCategoryCommand";
import { IDeleteCategoryCommand } from "../interfaces/commands/IDeleteCategoryCommand";
import { ICreateCategoryCommand } from "../interfaces/commands/ICreateCategoryCommand";
import { IStoreLearningRecordCommand } from "../interfaces/commands/IStoreLearningRecordCommand";
import { IDeleteLearningRecordCommand } from "../interfaces/commands/IDeleteLearningRecordCommand";
import { StoreService } from "../services/StoreService";
import { formatDateTime } from "../services/DateTimeHelpers";
import MainElement from "./main-element";
import * as Const from "../const/constants";
import * as Scenarios from "../actions/scenarios";
import ModalConfirmForm from "./modalConfirmForm";
import { DictionaryService } from "../services/DictionaryService";
import { PermissionService } from "../services/PermissionService";
import GroupSubgroup from "./group-subgroup";
import * as StringHelpers from "../services/StringHelpers";
import * as _ from 'lodash';
import Axios, { AxiosResponse } from 'axios';
import ReactTable from "react-table";
import { PatternEdit } from "./patternEdit";
import {
    TextField,
    ButtonBox,
    Switch,
    Grid,
    Row,
    Col,
    Select,
    Teatarea,
    RegistryPlate,
    Paginator,
    IRegistryPlateSpec,
    IRegistryPlateBody,
    IRegistryPlateField,
    ShortInfo,
    Checkbox,
    ButtonTextedWithIcon,
    Confirm
} from "@sbt/react-ui-components"

interface ICategoryEditProps {
    id: number;
    versionVirtId: string;
    item?: ICategoryDto;
    isLoading: boolean;
    isAdding: boolean;
    loadingError?: string;
    catPartId?: string;
    catPartCaption: string;
    catSubpartId?: string;
    catSubpartCaption: string;
    learnings: ILearningDto[];
    pages: number;
}

interface IId { id: number };

interface ICategoryEditState {
    versionVirtId: string;
    catPartId?: string;
    catPartCaption: string;
    catSubpartId?: string;
    catSubpartCaption: string;
    catName: string;
    catResponse: string;
    catSetContext: string;
    hasChanges: boolean;
    hasPatternChanges: boolean;
    patterns: IPatternDto[];
    selectedPattern?: IId;
    addingPattern?: boolean;
    changingPattern?: boolean;
    addingLearning?: boolean;
    changingLearning?: boolean;
    changingPhrase?: ILearningDto;
    isIneligible?: boolean;
    isDisabled?: boolean;
    requiredRoster?: string;
    requiredRosterName?: string;
    isML: boolean;

    gridState: any;
    pendingLoad: boolean;
}

class CategoryEditApp extends MainElement<ICategoryEditProps, ICategoryEditState>{

    constructor(props) {
        super(props);
    }

    state: ICategoryEditState = {
        versionVirtId: null,
        catPartId: null,
        catPartCaption: null,
        catSubpartId: null,
        catSubpartCaption: null,
        catName: null,
        catResponse: null,
        catSetContext: null,
        hasChanges: false,
        hasPatternChanges: false,
        isIneligible: false,
        isDisabled: false,
        patterns: [],
        isML: false,
        gridState: null,
        pendingLoad: false
    };

    sources: IDropdownItem[] = [];

    componentDidMount() {
        console.log('componentDidMount', this.props);
        if (/*!this.props.item &&*/ !this.props.isLoading)
            Scenarios.getCategoryItem(this.props.id.toString());
        if (this.props.isAdding) {
            Scenarios.addCategoryItemInit(
                this.props.catPartId, this.props.catPartCaption, this.props.catSubpartId, this.props.catSubpartCaption
            );
        }
    }

    componentWillReceiveProps(nextProps: ICategoryEditProps) {

        //console.log('nextProps', nextProps);

        if (nextProps.item == null) return;

        let newState = {};
        newState = {...newState,
            catPartId: nextProps.item.upperPartition ? nextProps.item.upperPartition.id : null,
            catPartCaption: nextProps.item.upperPartition ? nextProps.item.upperPartition.title : null,
            catSubpartId: nextProps.item.partition ? nextProps.item.partition.id : null,
            catSubpartCaption: nextProps.item.partition ? nextProps.item.partition.title : null
        };

        if (!nextProps.isLoading && this.state.pendingLoad) {
            newState = {...newState,
                pendingLoad: false
            };
            this.setState(newState);
            Scenarios.loadLearning(this.state.gridState, this.props.item.originId);
        }

        // Помещаем item:IGroupDto в state, только если пришла новая версия данных.
        if (!nextProps.versionVirtId || this.state.versionVirtId === nextProps.versionVirtId)
            return;

        newState = {...newState,
            versionVirtId: nextProps.versionVirtId,
            catName: nextProps.item.name,
            catResponse: nextProps.item.response,
            catSetContext: nextProps.item.setContext,
            isIneligible: nextProps.item.isIneligible,
            isDisabled: nextProps.item.isDisabled,
            requiredRoster: nextProps.item.requiredRoster,
            requiredRosterName: nextProps.item.requiredRosterName,
            hasChanges: false,
            hasPatternChanges: false,
            patterns: nextProps.item.patterns ? nextProps.item.patterns : []
        };

        this.setState(newState);
    }

    isReadOnly = (): boolean => {
        return !PermissionService.Permissions.canEditChatBot;
    }

    onCancelClick = (): void => {
        Scenarios.endEditCategoryItem();
    }

    onPublishClick = (): void => {
        if (this.props.isAdding) {
            
            let payload: ICreateCategoryCommand = { 
                partitionId: this.state.catSubpartId || null,
                
                name: this.state.catName || null,
                response: this.state.catResponse || null,
                setContext: this.state.catSetContext || null,
                isIneligible: this.state.isIneligible || null,
                isDisabled: this.state.isDisabled || null,
                requiredRoster: this.state.requiredRoster || null,
 
                patterns: this.state.patterns
            };

            // Сохраняем изменения
            Scenarios.createCategoryItem(this.state.versionVirtId, payload);
        } else {

            // Проверяем, что у нас есть редактируемые данные.
            if (!this.state.versionVirtId)
                return;

            let payload: IEditCategoryCommand = { id: this.props.id };

            // В payload помещаем только те свойства, значения которых были изменены.
            if (this.state.catName !== this.props.item.name)
                payload.name = this.state.catName || null;

            if (this.state.catResponse !== this.props.item.response)
                payload.response = this.state.catResponse || null;

            if (this.state.catSetContext !== this.props.item.setContext)
                payload.setContext = this.state.catSetContext || null;

            if (this.state.catSubpartId !== this.props.item.partitionId)
                payload.partitionId = this.state.catSubpartId || null;

            if (this.state.hasPatternChanges) {
                payload.patterns = this.state.patterns;
                payload.isChangedPatterns = true;
            }

            payload.isIneligible = this.state.isIneligible;
            payload.isDisabled = this.state.isDisabled;
            payload.requiredRoster = this.state.requiredRoster;

            // Сохраняем изменения
            Scenarios.saveEditedCategoryItem(this.state.versionVirtId, payload);
        }
    }

    onDeleteConfirmation = (confirmed: boolean): void => {

        if (!confirmed)
            return;

        // Проверяем, что у нас есть редактируемые данные.
        if (!this.state.versionVirtId)
            return;

        let payload: IDeleteCategoryCommand = { id: this.props.id };

        // Сохраняем изменения
        Scenarios.deleteCategoryItem(this.state.versionVirtId, payload);
    }

    onDeleteClick = (): void => {
        var modal = StoreService.ms({ content: ModalConfirmForm, externalProps: { text: "Удалить категорию?" } });
        var handler = modal.open();
        handler.close.then(val => this.onDeleteConfirmation(val));
    }

    onDeletePatternConfirmation = (confirmed: boolean, patternId: number): void => {

        if (!confirmed)
            return;

        var pattern = this.getSelectedPattern();
        if (pattern) {
            var index = this.state.patterns.indexOf(pattern, 0);
            if (index > -1) {
                this.state.patterns.splice(index, 1);
                this.setState({
                    selectedPattern: undefined,
                    patterns: [...this.state.patterns],
                    hasPatternChanges: true
                });
            }
        }
    }


    onDeletePatternClick = (patternId: number): void => {
        var pattern = this.getSelectedPattern();
        if (pattern) {
            var modal = StoreService.ms({ content: ModalConfirmForm, externalProps: { text: "Удалить паттерн ?" } });
            var handler = modal.open();
            handler.close.then(val => this.onDeletePatternConfirmation(val, pattern.id));
        }
    }

    onCategoryTextFocusOut = (value: string) => {

        value = StringHelpers.normalize(value);

        if (value === this.state.catName)
            return;

        this.setState({
            catName: value,
            hasChanges: true
        });
    }

    getSelectedPattern = (): IPatternDto => {
        if (!this.state.selectedPattern) return null;
        let pat = this.state.patterns.filter(x => x.id == this.state.selectedPattern.id);
        if (!pat || pat.length == 0) return null;
        return pat[0];
    }


    onPatternOnlyContextChanged = (value: any) => {
        var pattern = this.getSelectedPattern();
        if (pattern) {
            pattern.onlyContext = value.checked;
            this.setState({
                selectedPattern: { id: pattern.id },
                patterns: [...this.state.patterns],
                hasPatternChanges: true
            });
        }
    }

    onCategoryResponseFocusOut = (value: string) => {

        value = StringHelpers.normalize(value);

        if (value === this.state.catResponse)
            return;

        this.setState({
            catResponse: value,
            hasChanges: true
        });
    }

    onCategorySetContextFocusOut = (value: string) => {

        value = StringHelpers.normalize(value);

        if (value === this.state.catSetContext)
            return;

        this.setState({
            catSetContext: value,
            hasChanges: true
        });
    }

    onCategoryIsIneligibleChanged = (value: any) => {
        this.setState({
            isIneligible: value.checked,
            hasChanges: true
        });
    }

    onCategoryIsDisabledChanged = (value: any) => {
        this.setState({
            isDisabled: value.checked,
            hasChanges: true
        });
    }

    onPatternContextFocusOut = (value: string) => {

        value = StringHelpers.normalize(value);
        var pattern = this.getSelectedPattern();
        if (pattern) {

            if (value === pattern.context)
                return;

            pattern.context = value;
            this.setState({
                selectedPattern: { id: pattern.id },
                patterns: [...this.state.patterns],
                hasPatternChanges: true
            });
        }
    }

    onCategoryTextClear = (): void => {
        this.setState({
            catName: null,
            hasChanges: true
        });
    }

    onCategorySetContextClear = (): void => {
        this.setState({
            catSetContext: null,
            hasChanges: true
        });
    }

    onPatternContextClear = (): void => {
        var pattern = this.getSelectedPattern();
        if (pattern) {
            pattern.context = '';
            this.setState({
                selectedPattern: { id: pattern.id },
                patterns: [...this.state.patterns],
                hasPatternChanges: true
            });
        }
    }

    onSelectGroupSubgroup = (grId, grCaption, subgrId, subgrCaption): void => {
        this.setState({
            catPartId: grId,
            catPartCaption: grCaption,
            catSubpartId: subgrId,
            catSubpartCaption: subgrCaption,
            hasChanges: true
        });
    }

    onPermittedForScopeSelect = (item: IDropdownItem): void => {
    }

    IsPartRequired = (): boolean => {
        return !!this.state.catSubpartId;
    }

    IsSubpartRequired = (): boolean => {
        return !!this.state.catPartId;
    }

    canSave = (): boolean => {
        return ((this.state.versionVirtId || this.props.isAdding)
            && this.state.catName
            && (!!this.state.catPartId == !!this.state.catSubpartId)
            && (this.state.hasChanges || this.state.hasPatternChanges || this.props.isAdding)
        );
    }


    onPatternSelect = (pattern: IPatternDto): void => {
        this.setState({
            selectedPattern: { id: pattern.id }
        });
    }

    onChangePatternClick = () => {
        this.setState({
            changingPattern: true
        })
    }

    onCloseChangePattern = () => {
        this.setState({
            changingPattern: false,
            addingPattern: false
        })
    }

    getPatterns = (): ShortInfo[] => {
        let items: IPatternDto[] = this.state.patterns;

        return items.map(value => {
            return <PatternEdit key={`key-${value.id}`}
                pattern={{...value}} isReadOnly={this.isReadOnly()} 
                isOpen={this.state.selectedPattern?.id == value.id}
                onWordTypeSelect={this.onWordTypeSelect}
                onPatternContextClear={this.onPatternContextClear}
                onPatternContextFocusOut={this.onPatternContextFocusOut}
                onPatternOnlyContextChanged={this.onPatternOnlyContextChanged}
                onChangePatternClick={this.onChangePatternClick}
                onDeletePatternClick={this.onDeletePatternClick}
                onPatternSelect={this.onPatternSelect}
            />
        });
    }

    onSourceSelect = (item: IDropdownItem): void => {
        if (!item) {
            this.setState({
                requiredRoster: null,
                requiredRosterName: null,
                hasChanges: true
            });
            return;
        }

        if (this.state.requiredRoster == item.id) return;

        this.setState({
            requiredRoster: item.id,
            requiredRosterName: item.title,
            hasChanges: true
        });
    }

    getSources = (
        searchStrig: string,
        skip: number,
        take: number,
        callback: (items: IDropdownItem[]) => void): void => {

        if (this.sources.length == 0) {
            DictionaryService.onSourcesQuery((items: IDropdownItem[]): void => {
                this.sources = items;
                callback(this.sources);
            });
        } else {
            callback(this.sources);
        }
    
    }


    onWordTypeSelect = (e: IDropdownItem, word: IWordDto) => {
        word.wordTypeId = e ? +e.id : -1;
        let query = new FillWordFormsQuery(StoreService.auth, StoreService.servicesApi, "chatbotwords/word/forms");
        query.execute(word)
            .then((response: AxiosResponse) => {
                var pattern = this.getSelectedPattern();
                if (pattern) {
                    var wrd = pattern.words.filter(x => x.id == word.id);
                    if (wrd && wrd.length > 0) {
                        var wordInd = pattern.words.indexOf(wrd[0]);
                        pattern.words[wordInd] = response.data;
                        this.setState({
                            selectedPattern: { id: pattern.id },
                            patterns: [...this.state.patterns],
                            hasPatternChanges: true
                        });
                    }
                }
            })
            .catch((error: any) => {
                Scenarios.notifyError(error);
            });
    }

    getPreview = (text: string): any => {
        return {
            __html:
                (text || "")
                    .replace(/xbtn/g, 'button')
                    .replace(/\<org\>/g, '<b>').replace(/\<\/org\>/g, '</b>')
                    .replace(/\<xorg /g, '<b ').replace(/\<\/xorg\>/g, '</b>')
        }
    }

    onDeleteLearningConfirmation = (confirmed: boolean, learningId: number): void => {
        if (!confirmed)
            return;
            let payload: IDeleteLearningRecordCommand = { learningId: learningId };
            Scenarios.deleteLearningItem(payload, this.state.gridState, this.props.item.originId);
        }

    onSaveChangeLearning = (text: string) => {
        let payload: IStoreLearningRecordCommand = { learning: {
            id: this.state.changingPhrase ? this.state.changingPhrase.id : null,
            question: text,
            categoryId: this.props.item.originId
        } };
        Scenarios.storeLearningItem(payload, this.state.gridState, this.props.item.originId);

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

    onSaveChangePattern = (text: string) => {
        var pattern: IPatternDto = null;
        if (this.state.changingPattern) {
            pattern = this.getSelectedPattern();
            pattern.phrase = text;
        } else if(this.state.addingPattern) {
            pattern = {id: 0, categoryId: this.props.id, context:'', onlyContext:false, phrase: text, wordCount: 0, words: null};
        }
        if (pattern) {
            let query = new PatternCalculateQuery(StoreService.auth, StoreService.servicesApi, "chatbotwords/pattern/calculate");
            query.execute(pattern)
                .then((response: AxiosResponse) => {
                    if (this.state.changingPattern) {
                        pattern = this.getSelectedPattern();
                    } else if(this.state.addingPattern) {
                        pattern = {id: 0, categoryId: this.props.id, context:'', onlyContext:false, phrase: '', wordCount: 0, words: null};
                        this.state.patterns.push(pattern);
                        this.setState({patterns: this.state.patterns});
                    }
                    if (pattern) {
                        pattern.id = response.data.id;
                        if (!pattern.id) {
                            var minId = -1;
                            this.state.patterns.forEach((pat: IPatternDto) => {
                                if (minId >= pat.id) minId = pat.id - 1;
                            });
                            pattern.id = minId;
                        }
                        pattern.phrase = response.data.phrase;
                        pattern.wordCount = response.data.wordCount;
                        pattern.words = response.data.words;
                        var cnt = 0;
                        pattern.words.forEach((word: IWordDto) => {
                            word.wordTypeId = word.wordTypeId == null ? -1 : word.wordTypeId;
                            word.id = word.id ? word.id : --cnt;
                        });
                        this.setState({
                            selectedPattern: { id: pattern.id },
                            changingPattern: false,
                            addingPattern: false,
                            patterns: [...this.state.patterns],
                            hasPatternChanges: true
                        });
                    }
                })
                .catch((error: any) => {
                    Scenarios.notifyError(error);
                });
        }
    }

    onAddPatternClick = () => {
        this.setState({
            addingPattern: true
        });
    }

    onAddLearningClick = () => {
        this.setState({
            addingLearning: true,
            changingPhrase: null
        });
    }

    onPatternToLearnClick = () => {
        var modal = StoreService.ms({ content: ModalConfirmForm, externalProps: { text: "Перенести паттерны в обучение?" } });
        var handler = modal.open();
        handler.close.then(val => this.onPatternToLearnConfirmation(val));
    }

    onPatternToLearnConfirmation = (confirmed: boolean) => {
        if (confirmed) {
            Scenarios.patternToLearn(this.state.versionVirtId,  { categoryId: this.props.id } );
        }
    }

    onMakeCopyClick = () => {
        let payload: ICategoryDto = {
            id: 0,
            name: this.state.catName,
            response: this.state.catResponse,
            setContext: this.state.catSetContext,
            isIneligible: this.state.isIneligible,
            isDisabled: this.state.isDisabled,
            requiredRoster: this.state.requiredRoster,
            requiredRosterName: this.state.requiredRosterName,
            partitionId: this.state.catSubpartId,
            partition: this.state.catSubpartId ? { id: this.state.catSubpartId, title: this.state.catSubpartCaption } : null,
            upperPartition: this.state.catPartId ? { id: this.state.catPartId, title: this.state.catPartCaption } : null,
            patterns: this.state.patterns
        }
        Scenarios.makeCopy(payload);
    }

    clickML = (val) => {
        this.setState({isML: val});
    }


    learningEdit = (row: ILearningDto) => {
        this.setState({
            changingLearning: true,
            changingPhrase: row
        })
    }

    learningDelete = (row: ILearningDto) => {
        var modal = StoreService.ms({ content: ModalConfirmForm, externalProps: { text: `Удалить фразу '${row.question}'?` } });
        var handler = modal.open();
        handler.close.then(val => this.onDeleteLearningConfirmation(val, row.id));
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
        }
    ]

    onFetchData = (state, instance) => {
        if (this.props.isLoading) {
            this.setState({gridState: state, pendingLoad: true});
        } else {
            Scenarios.loadLearning(state, this.props.item.originId);
            this.setState({gridState: state});
        }
    }

    getLearning = () => {
        return (
            <ReactTable
                data={this.props.learnings}
                loading={this.props.isLoading}
                pages={this.props.pages}
                onFetchData={this.onFetchData}
                columns={this.columns}
                manual
                filterable 
                defaultSorted={[
                    {
                        id: "question",
                        desc: false
                    }
                ]}
                defaultPageSize={20}
            />
        );
    }

    gotoRoot = () => {
        StoreService.history.push(Const.NavigationPathCategoryList);
    }

    view() {
        return (
            <div className={'group-list-container wide'}>
                {/*<div className={'category-edit'}>*/}
                <div className="category-edit__head">
                    <div className="link-top-menu">
                        <div key={"link-top-menu-item1"} className={"link-top-menu-item"} onClick={this.gotoRoot} >
                                Категории
                        </div>
                        <div key={"link-top-menu-item2"} className={"link-top-menu-item-active"}>
                                {this.props.isAdding ? "Добавление категории" : "Редактирование категории"}
                        </div>
                    </div>
                    {this.props.item && !this.props.isAdding && 
                    <div className="category-edit__about">
                        <div>Изменено &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; {formatDateTime(this.props.item.changedOn)} {this.props.item.changedByName}</div>
                        {this.props.item.publishedOn && <div>Опубликовано {formatDateTime(this.props.item.publishedOn)}</div>}
                    </div>
                    }
                </div>

          

                <div className="category-edit__buttons">
                    <Grid>
                    <Row>
                        <Col baseSize={6} breakpointSizes={['md-6', 'sm-12']}>
                        <div className={'buttons-left'}>
                            {!this.props.isAdding && !this.isReadOnly() && <ButtonBox
                                    title="Удалить категорию"
                                    onClick={this.onDeleteClick}
                                    size="s"
                                    theme="danger"
                                />
                            }
                            {!this.props.isAdding && !this.isReadOnly() && <ButtonBox
                                    title="Создать копию"
                                    onClick={this.onMakeCopyClick}
                                    size="s"
                                />}
                            </div>
                        </Col>
                        <Col baseSize={6} breakpointSizes={['md-6', 'sm-12']}>
                            <div className={'buttons-right'}>
                                <ButtonBox
                                    title="Закрыть"
                                    onClick={this.onCancelClick}
                                    size="s"
                                    colorTheme="#a5b4dd"
                                />
                                {!this.isReadOnly() && <ButtonBox
                                    title="Сохранить"
                                    onClick={this.onPublishClick}
                                    isDisabled={!this.canSave()}
                                    size="s"
                                />}
                            </div>
                        </Col>
                    </Row>
                    </Grid>
                </div>
                <div className="category-edit__filter">
                <Grid>
                    <Row>
                        <Col baseSize={6} breakpointSizes={['md-6', 'sm-12']}>
                            <div className={'first-col'}>
                                <TextField
                                    title="Наименование категории"
                                    hasTooltip={false}
                                    placeholder="Введите наименование"
                                    isRequired={true}
                                    isDisabled={this.isReadOnly()}
                                    onClear={this.onCategoryTextClear}
                                    onFocusOut={this.onCategoryTextFocusOut}
                                    value={this.state.catName || ""}
                                />
                            </div>
                        </Col>
                        <Col baseSize={6} breakpointSizes={['md-6', 'sm-12']}>
                            <div className={'first-col category-edit__filter_fix-left'}>
                                <Select
                                    title="Обязательный элемент"
                                    maxHeight={200}
                                    isRequired={false}
                                    onSelect={(e: IDropdownItem) => { this.onSourceSelect(e); }}
                                    query={this.getSources}
                                    isDisabled={this.isReadOnly()}
                                    selectedItem={this.state.requiredRoster && {id: this.state.requiredRoster, title: this.state.requiredRosterName} || null}
                                />
                            </div>
                        </Col>
                    </Row>
                    <GroupSubgroup
                        onClick={(grId, grCaption, subgrId, subgrCaption) => this.onSelectGroupSubgroup(grId, grCaption, subgrId, subgrCaption)}
                        PartId={this.state.catPartId}
                        SubpartId={this.state.catSubpartId}
                        PartCaption={this.state.catPartCaption}
                        SubpartCaption={this.state.catSubpartCaption}
                        IsRequiredPart={this.IsPartRequired()}
                        IsDisabled={this.isReadOnly()}
                        IsRequiredSubpart={this.IsSubpartRequired()}
                    />
                    <Row>
                        <Col baseSize={6} breakpointSizes={['md-6', 'sm-12']}>
                            <div className={'first-col'}>
                                <Teatarea
                                    className={'uavp-textarea__high'}
                                    title="Ответ Евы"
                                    placeholder="Введите ответ"
                                    value={this.state.catResponse || ""}
                                    isDisabled={this.isReadOnly()}
                                    onFocusOut={this.onCategoryResponseFocusOut}
                                />
                            </div>
                        </Col>
                        <Col baseSize={6} breakpointSizes={['md-6', 'sm-12']}>
                            <div className={'first-col'}>
                                <div className="uavp-textarea">
                                    <label className="uavp-textarea__label">Предпросмотр</label>
                                    <div className="preview" dangerouslySetInnerHTML={this.getPreview(this.state.catResponse)} />
                                </div>
                            </div>
                        </Col>
                    </Row>
                    <Row>
                        <Col baseSize={6} breakpointSizes={['md-3', 'sm-6']}>
                            <div className={'first-col'}>
                                <TextField
                                    title="Установить контекст"
                                    hasTooltip={false}
                                    placeholder="Введите контекст"
                                    isRequired={false}
                                    onClear={this.onCategorySetContextClear}
                                    isDisabled={this.isReadOnly()}
                                    onFocusOut={this.onCategorySetContextFocusOut}
                                    value={this.state.catSetContext || ""}
                                />
                            </div>
                        </Col>
                        <Col baseSize={3} breakpointSizes={['md-3', 'sm-6']}>
                            <div className={'category-edit__filter_checkbox'}>
                                <Checkbox
                                    id="isIneligible"
                                    isChecked={this.state.isIneligible}
                                    isDisabled={this.isReadOnly()}
                                    onChange={this.onCategoryIsIneligibleChanged}
                                    title="Нежелательные слова"
                                />
                            </div>
                        </Col>
                        <Col baseSize={3} breakpointSizes={['md-3', 'sm-6']}>
                            <div className={'category-edit__filter_checkbox'}>
                                <Checkbox
                                    id="isDisabled"
                                    isChecked={this.state.isDisabled}
                                    isDisabled={this.isReadOnly()}
                                    onChange={this.onCategoryIsDisabledChanged}
                                    title="Отключить категорию"
                                />
                            </div>
                        </Col>
                    </Row>
                </Grid>
                </div>
                <div className="link-top-menu category-edit__head">
                    <div className="link-top-menu category-edit__switch">
                        <div className={"link-top-menu-item"+(this.state.isML ? "-active" : "")} onClick={() => (this.clickML(true))}>
                            Обучение
                        </div>
                        <div className={"link-top-menu-item"+(this.state.isML ? "" : "-active")} onClick={() => (this.clickML(false))}>
                            Паттерны
                        </div>
                    </div>
                    {!this.state.isML && <div className="category-edit__about">
                        Слова <b>(в скобках)</b> должны обязательно присутствовать в вопросе.<br/>
                        Слова <b>&lt;в угловых скобках&gt;</b> должны обязательно присутствовать в вопросе в указанном порядке.
                    </div>}
                    {!this.state.isML && this.props.id && <ButtonTextedWithIcon
                        title="Добавить паттерны в обучение"
                        onClick={this.onPatternToLearnClick}
                        size="s"
                    />}
                    {!this.state.isML && <ButtonTextedWithIcon
                        title="Добавить паттерн"
                        onClick={this.onAddPatternClick}
                        size="s"
                    />}
                    {this.state.isML && <ButtonTextedWithIcon
                        title="Добавить фразу"
                        onClick={this.onAddLearningClick}
                        size="s"
                    />}
                </div>

                {!this.state.isML && <div className="category-edit__pattern">
                    {this.getPatterns()}
                </div>}

                {this.state.isML && <div className="category-edit__pattern">
                    {this.getLearning()}
                </div>}
                <div className="link-top-menu category-edit__head">
                    <div className="link-top-menu category-edit__switch">
                    </div>
                    {!this.state.isML && this.state.patterns && this.state.patterns.length && <ButtonTextedWithIcon
                        title="Добавить паттерн"
                        onClick={this.onAddPatternClick}
                        size="s"
                    /> || null}
                </div>

                {(this.state.changingPattern || this.state.addingPattern) &&
                    <div className="pattern-dialog">
                        <Confirm
                            title="Пожалуйста, введите паттерн"
                            buttonLabel="Отправить"
                            onSubmit={this.onSaveChangePattern}
                            onClose={this.onCloseChangePattern}
                            placeholder="Пожалуйста, введите паттерн"
                            theme="default"
                            message={this.state.changingPattern ? this.getSelectedPattern().phrase : null}
                            preSetValue={this.state.changingPattern ? this.getSelectedPattern().phrase : null}
                        />
                    </div>
                }
                {(this.state.changingLearning || this.state.addingLearning) &&
                    <div className="pattern-dialog">
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

const mapStateToProps = (state: IPortalState, ownProps: any): ICategoryEditProps => {
    return {
        id: ownProps.match.params.id,
        versionVirtId: state.categoryItem.versionVirtId,
        item: state.categoryItem.item,
        isLoading: state.categoryItem.isLoading,
        isAdding: state.categoryItem.isAdding,
        loadingError: state.categoryItem.loadingError,
        catPartId: state.categoryItem.partitionId,
        catPartCaption: state.categoryItem.partitionCaption,
        catSubpartId: state.categoryItem.subPartId,
        catSubpartCaption: state.categoryItem.subPartCaption,
        learnings: state.categoryItem.learnings,
        pages: state.categoryItem.pages,
    };
};

export const CategoryEdit: any = withRouter(connect(mapStateToProps)(CategoryEditApp));