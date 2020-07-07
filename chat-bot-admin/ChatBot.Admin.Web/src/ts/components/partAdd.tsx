import * as React from "react";
import { withRouter } from "react-router";
import { connect } from "react-redux";
import { IPortalState } from "../interfaces/IPortalState";
import {ICreatePartCommand} from "../interfaces/commands/ICreatePartCommand";
import {StoreService} from "../services/StoreService";
import * as GuidService from "../services/GuidService";
import MainElement from "./main-element";
import * as Const from "../const/constants";
import * as Scenarios from "../actions/scenarios";
import ModalConfirmForm from "./modalConfirmForm";
import * as StringHelpers from "../services/StringHelpers";
import {
    TextField,
    ButtonBox,
    Grid,
    Row,
    Col
} from "@sbt/react-ui-components";

interface IPartAddProps {
}

interface IPartAddState {
    versionVirtId: string;
    caption?: string;
    hasChanges: boolean;
}

class PartAddApp extends MainElement<IPartAddProps, IPartAddState>{

    constructor(props) {
        super(props);
    }

    state: IPartAddState = {
        versionVirtId: null,
        caption: null,
        hasChanges: false
    };

    componentDidMount()
    {
        this.setState({
            versionVirtId: GuidService.getNewGUIDString()
        });
    }

    onPublishClick = (): void => {

        // Проверяем, что у нас есть редактируемые данные.
        if(!this.state.versionVirtId)
            return;

        if(this.state.caption === undefined || this.state.caption === "")
            return;

        let payload: ICreatePartCommand = { caption: this.state.caption };

        // Сохраняем изменения
        Scenarios.createPartitionItem(this.state.versionVirtId, payload);
    }

    onCancelConfirmation = (confirmed: boolean): void => {

        if(!confirmed)
            return;

        Scenarios.addPartitionItemCancel();
    }

    onCancelClick = (): void => {

        // Проверяем, что у нас есть редактируемые данные.
        if(!this.state.hasChanges)
        {
            this.onCancelConfirmation(true);
            return;
        }

        var modal = StoreService.ms({content: ModalConfirmForm, externalProps: {text: "Отменить создание раздела?"}});
        var handler = modal.open();
        handler.close.then(val => this.onCancelConfirmation(val));
    }

    onPartCaptionFocusOut = (value: string): void => {

        value = StringHelpers.normalize(value);

        if(value === this.state.caption)
            return;

        this.setState({
            caption: value,
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
        return this.state.caption && this.state.hasChanges;
    }

    view() {
        return (
            <div className={'group-add-container'}>
                <h3>Создание раздела</h3>
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


const mapStateToProps = (state: IPortalState, ownProps: any): IPartAddProps => {
    return {
    };
};

export const PartAdd: any = withRouter(connect(mapStateToProps)(PartAddApp));