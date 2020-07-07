import * as React from "react";
import { withRouter } from "react-router";
import { connect } from "react-redux";
import MainElement from "./main-element";
import ListHeader from "./categoryHeader";
import { IPortalState } from '../interfaces/IPortalState';
import { IPartitionDto } from '../interfaces/IPartitionDto';
import { ISubpartDto } from '../interfaces/ISubpartDto';
import * as Scenarios from "../actions/scenarios";
import { StoreService } from '../services/StoreService';
import TopMenu from "./topMenu";
import * as Const from "../const/constants";
import ModalConfirmForm from "./modalConfirmForm";
import {IDeletePartCommand} from "../interfaces/commands/IDeletePartCommand";
import * as GuidService from "../services/GuidService";
import { PermissionService } from "../services/PermissionService";
import {
    Grid, Row, Col,
    Checkbox, TextField,
    ButtonBox
} from "@sbt/react-ui-components";

interface ISettingsProps {
    useModel?: boolean;
    useMLThreshold?: boolean;
    useMLMultiAnswer?: boolean;
    mLThreshold?: string;
    mLMultiThreshold?: string;
}

interface ISettingsState {
    
}

class SettingsApp extends MainElement<ISettingsProps, ISettingsState>{

    constructor(props) {
        super(props);
    }

    componentDidMount()
    {
        Scenarios.loadSettings();
    }

    onUseModelClick = (value: any): void => {
        Scenarios.updateSettings({...this.props, useModel: value.checked});
    }

    onUseMLThresholdClick = (value: any): void => {
        Scenarios.updateSettings({...this.props, useMLThreshold: value.checked});
    }

    onUseMLMultiAnswerClick = (value: any): void => {
        Scenarios.updateSettings({...this.props, useMLMultiAnswer: value.checked});
    }

    onMLThresholdClick = (value: any): void => {
        Scenarios.updateSettings({...this.props, mLThreshold: value});
    }

    onMLMultiThresholdClick = (value: any): void => {
        Scenarios.updateSettings({...this.props, mLMultiThreshold: value});
    }

    onApplyClick = (value: any): void => {
        Scenarios.applySettings({...this.props});
    }

    isReadOnly = (): boolean => {
        return !PermissionService.Permissions.canEditChatBot;
    }

    view() {

        return (
            <div className={'group-list-container wide'}>
                <TopMenu activeItemId={3}/>
                <Grid>
                    <Row>
                        <Col baseSize={3}>
                            <div>&nbsp;</div>
                            <Checkbox
                                id="useModel"
                                isChecked={this.props.useModel}
                                isDisabled={this.isReadOnly()}
                                onChange={this.onUseModelClick}
                                title="Использовать ML для обработки вопросов"
                            />
                        </Col>
                    </Row>
                    <Row>
                        <Col baseSize={3}>
                            <div>&nbsp;</div>
                            <Checkbox
                                id="useMLThreshold"
                                isChecked={this.props.useMLThreshold}
                                isDisabled={this.isReadOnly()}
                                onChange={this.onUseMLThresholdClick}
                                title="Использовать ли порог для ответов модели ML"
                            />
                        </Col>
                        <Col baseSize={2}>
                            <TextField
                                id="mLThreshold"
                                value={this.props.mLThreshold}
                                isDisabled={this.isReadOnly()}
                                onChange={this.onMLThresholdClick}
                            />
                        </Col>
                    </Row>
                    <Row>
                        <Col baseSize={3}>
                            <div>&nbsp;</div>
                            <Checkbox
                                id="useMLMultiAnswer"
                                isChecked={this.props.useMLMultiAnswer}
                                isDisabled={this.isReadOnly()}
                                onChange={this.onUseMLMultiAnswerClick}
                                title="Использовать ли несколько ответов модели ML"
                            />
                        </Col>
                        <Col baseSize={2}>
                            <TextField
                                id="mLMultiThreshold"
                                value={this.props.mLMultiThreshold}
                                isDisabled={this.isReadOnly()}
                                onChange={this.onMLMultiThresholdClick}
                            />
                        </Col>
                    </Row>
                    <Row>
                        <Col baseSize={3}>
                            <div>&nbsp;</div>
                        </Col>
                    </Row>
                </Grid>
                <ButtonBox 
                    title="Сохранить"
                    onClick={this.onApplyClick}
                    size="s"
                    />
            </div>
        );
    }
}

const mapStateToProps = (state: IPortalState, ownProps: any): ISettingsProps => {
    return {
        useModel: state.settings.useModel,
        useMLThreshold: state.settings.useMLThreshold,
        useMLMultiAnswer: state.settings.useMLMultiAnswer,
        mLThreshold: state.settings.mLThreshold,
        mLMultiThreshold: state.settings.mLMultiThreshold,
    };
};

export const Settings: any = withRouter(connect(mapStateToProps)(SettingsApp));