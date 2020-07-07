import * as React from "react";
import { Route, withRouter } from "react-router";
import { connect } from "react-redux";
import { IPortalPageProps } from "../interfaces/IPortalPage";
import { IPortalState } from "../interfaces/IPortalState";
import { IPartitionDto } from "../interfaces/IPartitionDto";
import {IEditPartCommand} from "../interfaces/commands/IEditPartCommand";
import {IDeletePartCommand} from "../interfaces/commands/IDeletePartCommand";
import {StoreService} from "../services/StoreService";
import MainElement from "./main-element";
import * as Const from "../const/constants";
import * as Scenarios from "../actions/scenarios";
import ModalConfirmForm from "./modalConfirmForm";
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

interface IPartEditProps {
    versionVirtId: string;
    item?: IPartitionDto;
    isLoading: boolean;
    loadingError?: string;
}

interface IPartEditState {
    versionVirtId: string;
    id: string;
    caption?: string;
    hasChanges: boolean;
    canDelete: boolean;
    categoryCount?: number;
    subPartCount?: number;
}

class PartEditApp extends MainElement<IPartEditProps, IPartEditState>{

    constructor(props) {
        super(props);
    }

    state: IPartEditState = {
        versionVirtId: null,
        id: undefined,
        caption: undefined,
        hasChanges: false,
        canDelete: false
    };

    componentDidMount()
    {
        if(!this.props.item && !this.props.isLoading)
            StoreService.history.push(Const.NavigationPathPartitionList);
    }

    componentWillReceiveProps(nextProps: IPartEditProps) {

        // Помещаем item:IPartitionDto в state, только если пришла новая версия данных.
        if(!nextProps.versionVirtId || this.state.versionVirtId === nextProps.versionVirtId)
            return;

        this.setState({
            versionVirtId: nextProps.versionVirtId,
            id: nextProps.item.id,
            caption: nextProps.item.title,
            hasChanges: false,
            canDelete: !nextProps.item.categoryCount && !(nextProps.item.subpartitions && nextProps.item.subpartitions.length),
            categoryCount: nextProps.item.categoryCount,
            subPartCount: nextProps.item.subpartitions ? nextProps.item.subpartitions.length : 0
        });
    }

    onCancelClick = (): void => {
        Scenarios.endEditPartitionItem();
    }

    onPublishClick = (): void => {

        // Проверяем, что у нас есть редактируемые данные.
        if(!this.state.versionVirtId)
            return;

        let payload: IEditPartCommand = { id: this.state.id };

        // В payload помещаем только те свойства, значения которых были изменены.
        if(this.state.caption !== this.props.item.title)
            payload.caption = this.state.caption || null;

        // Сохраняем изменения
        Scenarios.saveEditedPartitionItem(this.state.versionVirtId, payload);
    }

    onDeleteConfirmation = (confirmed: boolean): void => {

        if(!confirmed)
            return;

        // Проверяем, что у нас есть редактируемые данные.
        if(!this.state.versionVirtId)
            return;

        let payload: IDeletePartCommand = { id: this.state.id };

        // Сохраняем изменения
        Scenarios.deletePartitionItem(this.state.versionVirtId, payload);
    }

    onDeleteClick = (): void => {

        // Проверяем, что у нас есть редактируемые данные.
        if(!this.state.versionVirtId)
            return;

        var modal = StoreService.ms({content: ModalConfirmForm, externalProps: {text: "Удалить раздел?"}});
        var handler = modal.open();
        handler.close.then(val => this.onDeleteConfirmation(val));
    }

    onPartCaptionFocusOut = (value: string) => {

        if(value === this.state.caption)
            return;

        this.setState({
            caption: value === "" ? null : value,
            hasChanges: true
        });
    }

    onPartCaptionClear = (): void => {
        this.setState({
            caption: null,
            hasChanges: true
        });
    }

    canSave = (): boolean => {
        return this.state.versionVirtId && this.state.caption && this.state.hasChanges;
    }

    view() {
        return (
            <div className={'group-edit-container'}>
                <h3>Редактирование раздела</h3>
                {this.state.subPartCount ? <h4>Подразделов у раздела: {this.state.subPartCount}</h4> : null}
                {this.state.categoryCount ? <h4>Категорий в разделе: {this.state.categoryCount}</h4> : null}
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
                                    onFocusOut={this.onPartCaptionFocusOut}
                                    onClear={this.onPartCaptionClear}
                                    value={this.state.caption || ""}
                                    />
                            </div>
                        </Col>
                    </Row>
                    <Row>
                        <Col baseSize={4} breakpointSizes={['md-6', 'sm-12']}>
                            <div className={'buttons-left'}>
                                <ButtonBox
                                    title="Удалить раздел"
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

const mapStateToProps = (state: IPortalState, ownProps: any): IPartEditProps => {
    return {
        versionVirtId: state.partitionItem.versionVirtId,
        item: state.partitionItem.item,
        isLoading: state.partitionItem.isLoading,
        loadingError: state.partitionItem.loadingError
    };
};

export const PartEdit: any = withRouter(connect(mapStateToProps)(PartEditApp));