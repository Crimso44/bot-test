import * as React from 'react';
import ChatContainer from './blocks/chat-container';
import { IPluginProps } from './interfaces/props/IPluginProps';
import { MessagesService } from "./services/messagesService";
import { EmployeeFindService } from "./services/employeeFindService";

interface IState {
    init: boolean;
 }

export class Main extends React.Component<IPluginProps, IState> {

    state = {init: false};

    constructor(props) {
        super(props);
        MessagesService.authService = this.props.auth;
        EmployeeFindService.authService = this.props.auth;
        this.props.auth.getPortalEmployee().then(employee => {
            MessagesService.employee = employee;
            MessagesService.init();
            this.setState({ init: true });
        })
    }

    render() {
        return (
            this.state.init && <ChatContainer location={this.props.location}/> || null
        );
    }
}