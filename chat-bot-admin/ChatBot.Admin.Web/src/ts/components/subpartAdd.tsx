import * as React from "react";
import { withRouter } from "react-router";
import { connect } from "react-redux";
import { IPortalState } from "../interfaces/IPortalState";
import {ICreateSubpartCommand} from "../interfaces/commands/ICreateSubpartCommand";
import {StoreService} from "../services/StoreService";
import * as GuidService from "../services/GuidService";
import MainElement from "./main-element";
//import * as Const from "../const/constants";
import * as Scenarios from "../actions/scenarios";
import * as StringHelpers from "../services/StringHelpers";
import {DictionaryService} from "../services/DictionaryService";
import ModalConfirmForm from "./modalConfirmForm";
import {
    TextField,
    ButtonBox,
    Grid,
    Row,
    Col,
    Select,
    IDropdownItem
} from "@sbt/react-ui-components";

interface ISubpartAddProps {
    parentPartCaption?: string;
    parentPartId?: string;
}

interface ISubpartAddState {
    versionVirtId: string;
    parentPartCaption?: string;
    parentPartId?: string;
    caption?: string;
    hasChanges: boolean;
}

class SubpartAddApp extends MainElement<ISubpartAddProps, ISubpartAddState> {

    constructor(props) {
        super(props);
    }

    state: ISubpartAddState = {
        versionVirtId: null,
        parentPartCaption: null,
        parentPartId: null,
        caption: null,
        hasChanges: false
    };

    componentDidMount()
    {
        this.setState({
            versionVirtId: GuidService.getNewGUIDString(),
            parentPartId: this.props.parentPartId,
            parentPartCaption: this.props.parentPartCaption
        });
    }

    onPublishClick = (): void => {

        // Проверяем, что у нас есть редактируемые данные.
        if(!this.state.versionVirtId)
            return;

        let payload: ICreateSubpartCommand = {};

        if(this.state.caption)
            payload.caption = this.state.caption;

        if(this.state.parentPartId)
            payload.parentPartId = this.state.parentPartId;

        // Сохраняем изменения
        Scenarios.createSubpartItem(this.state.versionVirtId, payload);
    }

    onCancelConfirmation = (confirmed: boolean): void => {

        if(!confirmed)
            return;

        Scenarios.addSubpartItemCancel();
    }

    onCancelClick = (): void => {

        // Проверяем, что у нас есть редактируемые данные.
        if(!this.state.hasChanges)
        {
            this.onCancelConfirmation(true);
            return;
        }

        var modal = StoreService.ms({content: ModalConfirmForm, externalProps: {text: "Отменить создание подраздела?"}});
        var handler = modal.open();
        handler.close.then(val => this.onCancelConfirmation(val));
    }

    onSubpartCaptionFocusOut = (value: string) => {

        value = StringHelpers.normalize(value);

        if(value === this.state.caption)
            return;

        this.setState({
            caption: value,
            hasChanges: true
        });
    }

    onSubpartCaptionClear = (): void => {
        this.setState({
            caption: undefined,
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
        return this.state.caption && this.state.parentPartId && this.state.hasChanges;
    }

    view() {
        return (
            <div className={'subgroup-add-container'}>
                <h3>Создание подраздела</h3>
                <Grid>
                    <Row>
                        <Col baseSize={12} breakpointSizes={['md-6', 'sm-12']}>
                            <div className={'first-col'}>
                                <TextField
                                    title="Отображаемое наименование подраздела"
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
                        <Col baseSize={12} breakpointSizes={['md-6', 'sm-12']}>
                            <div className={'buttons-right'}>
                                <ButtonBox
                                    title="Отмена"
                                    onClick={this.onCancelClick}
                                    size="s"
                                    colorTheme="#a5b4dd"
                                    />
                                <ButtonBox
                                    title="Создать"
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

const mapStateToProps = (state: IPortalState, ownProps: any): ISubpartAddProps => {
    return {
        parentPartId: state.subPartItem.parentPartId,
        parentPartCaption: state.subPartItem.parentPartCaption
    };
};

export const SubpartAdd: any = withRouter(connect(mapStateToProps)(SubpartAddApp));