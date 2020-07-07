import * as React from 'react';
import MainElement from '../elements/main-element';
import { IChatMediumProps } from "../interfaces/props/IChatMediumProps";
import { IChatMediumState } from "../interfaces/states/IChatMediumState";
import * as Config from "../config/config";
import { MessagesService } from "../services/messagesService";
export default class ChatMedium extends MainElement<IChatMediumProps, IChatMediumState>
{
    state = { 
        alohaVisible: false, 
        eveNum: Math.floor(Math.random() * 7) + 1 
    };

    constructor(props)
    {
        super(props);
    }

    private getAlohaElement = (): JSX.Element => {

        if(!this.state.alohaVisible)
        {
            return null;
        }

        return (
            <div className="chat-bot__chat-medium-message">
                <div dangerouslySetInnerHTML={{__html: MessagesService.helloMessage}}></div>
            </div>
        );
    }

    private showAloha = (visible: boolean): void => {
        this.setState({ alohaVisible: visible });
    }

    view()
    {
        return (
            <div className="chat-bot__chat-medium"
                onMouseEnter={() => this.showAloha(true)}
                onMouseLeave={() => this.showAloha(false)}
                onClick={this.props.onClick}>
                <img src={`${Config.ImagesBase}/eve${''/*this.state.eveNum*/}.png`} className="chat-bot__chat-medium-icon"/>
                {this.getAlohaElement()}
            </div>
        );
    }
}