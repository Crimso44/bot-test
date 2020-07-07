import * as React from 'react';
import MainElement from '../elements/main-element';
import * as moment from 'moment';
import 'moment/locale/ru';
import MessageButton from "./message-button";
import {MessageFormatService} from "../services/messageFormatService";
import * as Config from "../config/config";

interface ILabelProps {
    id?: number;
    text?: string;
    date?: string;
    fromMe?: boolean;
    name?: string;
    rate?: number;
    context?: string;
    likeValue?: number;
    isLikeable: boolean;
    onButtonClick?: (category: string, context: string, text: string, isAddQuestion: boolean) => void,
    onLikeClick?: (messageId: number, vote: number) => void
}

interface ILabelState { }

export default class Message extends MainElement<ILabelProps, ILabelState> {

    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div className="chat-bot__chat-big-message clearfix">
                { this.createMessage()}
            </div>
        )
    }

    private getBotIcon = (): JSX.Element => {
        let imgName = this.props.rate === 0 ? "eve-no.png" : "eve-yes.png";

        return (
            <img src={`${Config.ImagesBase}/${imgName}`} />
            );
    }

    private createBotWaitingMessage = (): JSX.Element => {
        return (
            <div>
                <div className="chat-bot__chat-big-message-from-bot-icon">
                    {this.getBotIcon()}
                </div>
                <div className="chat-bot__chat-big-message-from-bot-content">
                    <div className="chat-bot__chat-big-message-from-bot-content-loading">
                        <img src={`${Config.ImagesBase}/dots.svg`} />
                    </div>
                </div>
            </div>
        );
    }

    private createBotMessage = (): JSX.Element => {
        return (
            <div>
                <div className="chat-bot__chat-big-message-from-bot-icon">
                    {this.getBotIcon()}
                </div>
                <div className="chat-bot__chat-big-message-from-bot-content">
                    <div className="chat-bot__chat-big-message-from-bot-header">
                        {this.props.name}&nbsp;{this.formatDateTime(this.props.date)}
                    </div>
                    <div className="chat-bot__chat-big-message-from-bot-message">
                        {MessageFormatService.parse(this.props.text, this.props.context, this.props.onButtonClick)}
                        {this.createLikeElement()}
                    </div>
                </div>
            </div>
        );
    }

    private createUserMessage = (): JSX.Element => {
        return (
            <div>
                 <div className="chat-bot__chat-big-message-from-user-content">
                    <div className="chat-bot__chat-big-message-from-user-header">
                        {this.formatDateTime(this.props.date)}
                    </div>
                    <div className="chat-bot__chat-big-message-from-user-message">
                        {this.props.text}
                    </div>
                </div>
            </div>
        );
    }

    private formatDateTime = (date: string): string => {
        moment.locale("ru");
        let m = moment(date);
        let now = moment();

        if(m.isSame(now, "day")){
            return m.format("HH:mm");
        }

        return m.format("lll");
    }

    private createMessage = (): JSX.Element => {
        return this.props.fromMe
            ? this.createUserMessage()
            : this.props.id === -1
            ? this.createBotWaitingMessage()
            : this.createBotMessage();
    }

    private onLikeClick = (vote: number): void => {

        if(!this.props.id){
            return;
        }

        if(this.props.likeValue === vote)
        {
            this.props.onLikeClick(this.props.id, 0);
            return;
        }

        this.props.onLikeClick(this.props.id, vote);
    }

    private getLikeClassName = (vote: number): string => {
        if(vote === this.props.likeValue){
            return "material-icon-font like-active";
        }

        return "material-icon-font";
    }

    private createLikeElement = () : JSX.Element => {

        if(this.props.fromMe || !this.props.isLikeable){
            return null;
        }

        return (
            <div className="chat-bot__chat-big-messages-message-like">
                <span className={this.getLikeClassName(1)} aria-hidden="true" title="Ответ полезен!"
                    onClick={() => this.onLikeClick(1)}>&#xE8DC;</span>
                <span className={this.getLikeClassName(-1)} aria-hidden="true" title="Ответ бесполезен!"
                    onClick={() => this.onLikeClick(-1)}>&#xE8DB;</span>
            </div>
            );
    }
}