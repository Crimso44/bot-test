import * as React from 'react';
import MainElement from '../elements/main-element';
import { IChatContainerProps } from "../interfaces/props/IChatContainerProps";
import { IChatContainerState } from "../interfaces/states/IChatContainerState";
import ChatMedium from "./chat-medium";
import ChatBig from "./chat-big";
import * as UrlService from "../services/urlService";

const defaultState: IChatContainerState = {
    mode: 'medium'
}

export default class ChatContainer extends MainElement<IChatContainerProps, IChatContainerState>{
    state = defaultState;

    constructor(props) {
        super(props);
    }

    componentDidMount(){
        this.tryShowBig();
    }

    tryShowBig = (): void => {
        var map = UrlService.parseQueryString(this.props.location.search);

        if(map.openChat && map.openChat === "true")
            this.showBig();
    }

    showMedium = (): void => {
        this.setState({ mode: 'medium' });
    }

    showBig = (): void => {
        this.setState({ mode: 'big' });
    }

    view() {

        var chat = null;

        switch(this.state.mode)
        {
            case 'big':
                chat = <ChatBig
                            onClick={this.showMedium} />;
                break;
            default:
                chat = <ChatMedium
                            onClick={this.showBig} />;
                break;
        }

        return chat;
    }
}