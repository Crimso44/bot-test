import * as React from "react";
import { Route, withRouter } from "react-router";
import { connect } from "react-redux";
import { IPortalPageProps } from "../interfaces/IPortalPage";
import { IPortalState } from "../interfaces/IPortalState";
import { ISubpartDto } from "../interfaces/ISubpartDto";
import {IEditSubpartCommand} from "../interfaces/commands/IEditSubpartCommand";
import {IDeleteSubpartCommand} from "../interfaces/commands/IDeleteSubpartCommand";
import {StoreService} from "../services/StoreService";
import MainElement from "./main-element";
import * as Const from "../const/constants";
import * as Scenarios from "../actions/scenarios";
import ModalConfirmForm from "./modalConfirmForm";
import {DictionaryService} from "../services/DictionaryService";
import {
    TextField,
    IconSvg,
    SvgPathes,
    ButtonBox,
    Switch,
    Grid,
    Row,
    Col,
    Select,
    Teatarea,
    IDropdownItem
} from "@sbt/react-ui-components";

interface ISubpartEditProps {
    versionVirtId: string;
    item?: ISubpartDto;
    isLoading: boolean;
    loadingError?: string;
}

interface ISubpartEditState {
    versionVirtId: string;
    id: string;
    parentPartCaption?: string;
    parentPartId?: string;
    caption?: string;
    hasChanges: boolean;
    canDelete: boolean;
    linkCount?: number;
}

class SubpartEditApp extends MainElement<ISubpartEditProps, ISubpartEditState>{

    constructor(props) {
        super(props);
    }

    state: ISubpartEditState = {
        versionVirtId: null,
        id: undefined,
        parentPartCaption: undefined,
        parentPartId: undefined,
        caption: undefined,
        hasChanges: false,
        canDelete: false
    };

    componentDidMount()
    {
        if(!this.props.item && !this.props.isLoading)
            StoreService.history.push(Const.NavigationPathPartitionList);
    }

    componentWillReceiveProps(nextProps: ISubpartEditProps) {

        // Помещаем item:IPartDto в state, только если пришла новая версия данных.
        if(!nextProps.versionVirtId || this.state.versionVirtId === nextProps.versionVirtId)
            return;

        this.setState({
            versionVirtId: nextProps.versionVirtId,
            id: nextProps.item.id,
            caption: nextProps.item.title,
            parentPartCaption: nextProps.item.parentTitle,
            parentPartId: nextProps.item.parentId,
            hasChanges: false,
            canDelete: !nextProps.item.categoryCount,
            linkCount: nextProps.item.categoryCount
        });
    }

    onCancelClick = (): void => {
        Scenarios.endEditSubpartItem();
    }

    onPublishClick = (): void => {

        // Проверяем, что у нас есть редактируемые данные.
        if(!this.state.versionVirtId)
            return;

        let payload: IEditSubpartCommand = { id: this.state.id };

        // В payload помещаем только те свойства, значения которых были изменены.
        if(this.state.caption !== this.props.item.title)
            payload.caption = this.state.caption || null;

        if(this.state.parentPartId !== this.props.item.parentId)
            payload.parentPartId = this.state.parentPartId || null;

        // Сохраняем изменения
        Scenarios.saveEditedSubpartItem(this.state.versionVirtId, payload);
    }

    onDeleteConfirmation = (confirmed: boolean): void => {

        if(!confirmed)
            return;

        // Проверяем, что у нас есть редактируемые данные.
        if(!this.state.versionVirtId)
            return;

        let payload: IDeleteSubpartCommand = { id: this.state.id };

        // Сохраняем изменения
        Scenarios.deleteSubpartItem(this.state.versionVirtId, payload);
    }

    onDeleteClick = (): void => {

        // Проверяем, что у нас есть редактируемые данные.
        if(!this.state.versionVirtId)
            return;

        var modal = StoreService.ms({content: ModalConfirmForm, externalProps: {text: "Удалить подраздел?"}});
        var handler = modal.open();
        handler.close.then(val => this.onDeleteConfirmation(val));
    }

    onSubpartCaptionFocusOut = (value: string) => {

        if(value === this.state.caption)
            return;

        this.setState({
            caption: value === "" ? null : value,
            hasChanges: true
        });
    }

    onSubpartCaptionClear = (): void => {
        this.setState({
            caption: null,
            hasChanges: true
        });
    }

    onPartSelect = (item: IDropdownItem): void => {
        this.setState({
            parentPartId: item && item.id || undefined,
            parentPartCaption: item && item.title || undefined,
            hasChanges: true
        });
    }

    canSave = (): boolean => {
        return this.state.versionVirtId && this.state.caption && this.state.parentPartId && this.state.hasChanges;
    }

    view() {
        return (
            <div className={'subgroup-edit-container'}>
                <h3>Редактирование подраздела</h3>
                {this.state.linkCount ? <h4>Категорий в подразделе: {this.state.linkCount}</h4> : null}
                <Grid>
                    <Row>
                        <Col baseSize={12} breakpointSizes={['md-6', 'sm-12']}>
                            <div className={'first-col'}>
                                <TextField
                                    title="Отображаемое наименование раздела"
                                    hasTooltip={false}
                                    placeholder="Введите наименование"
                                    isRequired={true}
                                    isFocused={true}
                                    onFocusOut={this.onSubpartCaptionFocusOut}
                                    onClear={this.onSubpartCaptionClear}
                                    value={this.state.caption || ""}
                                    />
                            </div>
                        </Col>
                    </Row>
                    <Row>
                        <Col baseSize={12} breakpointSizes={['md-6', 'sm-12']}>
                            <div className={'first-col'}>
                            <Select
                                title="Родительский раздел"
                                placeholder="Выберите родительский раздел"
                                maxHeight={200}
                                maxWidth={600}
                                onSelect={this.onPartSelect}
                                query={DictionaryService.onPartitionQuery}
                                selectedItem={this.state.parentPartId && {id: this.state.parentPartId, title: this.state.parentPartCaption} || null}
                                isRequired={true}
                                />
                            </div>
                        </Col>
                    </Row>
                    <Row>
                        <Col baseSize={4} breakpointSizes={['md-6', 'sm-12']}>
                            <div className={'buttons-left'}>
                                <ButtonBox
                                    title="Удалить пораздел"
                                    onClick={this.onDeleteClick}
                                    size="s"
                                    theme="danger"
                                    isDisabled={!this.state.canDelete}
                                    />
                            </div>
                        </Col>

                        <Col baseSize={8} breakpointSizes={['md-6', 'sm-12']}>
                            <div className={'buttons-right'}>
                                <ButtonBox
                                    title="Закрыть"
                                    onClick={this.onCancelClick}
                                    size="s"
                                    colorTheme="#a5b4dd"
                                    />
                                <ButtonBox
                                    title="Сохранить"
                                    onClick={this.onPublishClick}
                                    size="s"
                                    isDisabled={!this.canSave()}
                                    />
                            </div>
                        </Col>
                    </Row>
                </Grid>
            </div>
        );
    }
}

const mapStateToProps = (state: IPortalState, ownProps: any): ISubpartEditProps => {
    return {
        versionVirtId: state.subPartItem.versionVirtId,
        item: state.subPartItem.item,
        isLoading: state.subPartItem.isLoading,
        loadingError: state.subPartItem.loadingError
    };
};

export const SubpartEdit: any = withRouter(connect(mapStateToProps)(SubpartEditApp));