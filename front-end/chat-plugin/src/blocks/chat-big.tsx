import * as React from 'react';
import * as ReactDOM from 'react-dom';
import MainElement from '../elements/main-element';
import { IChatBigProps } from "../interfaces/props/IChatBigProps";
import { IChatBigState } from "../interfaces/states/IChatBigState";
import ListBlock from "./list-block";
import * as Config from "../config/config"
import Message from '../elements/message';
import { MessagesService } from "../services/messagesService";
import { MessageFormatService } from "../services/messageFormatService";
import { EmployeeFindService } from "../services/employeeFindService";
import { IMessageDto } from "../interfaces/IMessageDto";
import * as RUIC from '@sbt/react-ui-components';
import { ISelectItem } from "../interfaces/props/IPluginProps";

export default class ChatBig extends MainElement<IChatBigProps, IChatBigState>
{
    state = {
        messages: MessagesService.messages,
        hasNewMessages: MessagesService.messages.length > 0,
        isWaitingResponse: false,
        isContextMenuOpen: false,
        isOpenRoster: "",
        rosterText: "",
        rosterContext: "",
        savedMessage: "",
        isRequiredRoster: false
    }

    userMessage: any;
    userSelect: any;
    freezeUploadHistory: boolean;

    constructor(props) {
        super(props, false);
        this.freezeUploadHistory = false;

        MessagesService.helloPromise.then(() => {
            this.setState({messages: MessagesService.messages});
        });
    }

    textKeyDown = (e) => {
        this.tryClosePopupMenu();

        if (e.keyCode === 13) // Enter
        {
            this.send();
        }
        else if (e.keyCode === 50 && e.shiftKey) // @
        {
            let input = this.getTextInput();

            if (input && !input.value) // @ - первый и единственный символ
            {
                e.preventDefault();
                this.setState({ savedMessage: input.value, isOpenRoster: "E", hasNewMessages: false, rosterText: "Выберите сотрудника" });
            }
        }
    }

    selectKeyDown = (e) => {
        this.tryClosePopupMenu();

        if (e.keyCode === 27 || (!e.target.value && e.keyCode === 8)) // escape & backspace
        {
            if (e.keyCode === 8) {
                e.preventDefault();
                e.stopPropagation();
            }
            this.setState({ isOpenRoster: "", isRequiredRoster: false, hasNewMessages: false });
            this.focusTextInput();
        }
    }

    onButtonSend = (category: string, context: string, text: string, isAddQuestion: boolean): void => {

        if (this.state.isWaitingResponse) {
            return;
        }

        if (isAddQuestion) MessagesService.add(text);

        MessagesService.setWaitResponse();
        this.setState({ messages: MessagesService.messages, hasNewMessages: true, isWaitingResponse: true });
        MessagesService.buttonSend(text, context, category)
            .then(messages => {
                MessagesService.clearWaitResponse();

                var preFillInputValue = "";
                if (messages && messages.length > 0) {
                    let lastMessage = messages[messages.length - 1];
                    preFillInputValue = MessageFormatService.getPreFillValue(lastMessage.text);
                    if (preFillInputValue) lastMessage.isLikeable = false;
                }

                var newState: any = { 
                    messages: MessagesService.messages, 
                    hasNewMessages: true, 
                    isWaitingResponse: false, 
                    savedMessage: preFillInputValue,
                    isOpenRoster: false,
                    isRequiredRoster: false
                };
                if (MessagesService.isOpenRoster) {
                    newState.isOpenRoster = MessagesService.isOpenRoster;
                    newState.rosterText = MessagesService.rosterText;
                    newState.isRequiredRoster = true;
                }
                this.setState(newState);
                this.focusTextInput();
            })
            .catch(error => {
                MessagesService.clearWaitResponse();
                this.setState({ messages: MessagesService.messages, hasNewMessages: true, isWaitingResponse: false });
                this.focusTextInput();
            });
    }

    onLikeSend = (messageId: number, vote: number): void => {
        MessagesService.likeSend(messageId, vote);
        if (vote < 0) {
            this.onButtonSend("dislike", "dislike", "", false);
        } else {
            this.setState({ messages: MessagesService.messages, hasNewMessages: false });
            this.focusTextInput();
        }
    }

    onMenuClick = (e: React.MouseEvent<HTMLElement>): void => {
        this.setState({ isContextMenuOpen: !this.state.isContextMenuOpen, hasNewMessages: false });
        e.stopPropagation();
    }

    tryClosePopupMenu = (): void => {
        if (this.state.isContextMenuOpen) {
            this.setState({ isContextMenuOpen: false });
        }
    }

    writeToHelpDesk = (): void => {
        window.location.href = "mailto:chatbotfeedback.sbt@sberbank.ru";
    }

    getTextInput = (): HTMLTextAreaElement => {
        return ReactDOM.findDOMNode<HTMLTextAreaElement>(this.userMessage);
    }

    send = () => {

        let input = this.getTextInput();

        if (!input.value || this.state.isWaitingResponse) {
            return;
        }

        let messageText = input.value;
        MessagesService.add(messageText);
        MessagesService.setWaitResponse();
        this.setState({ savedMessage: "", messages: MessagesService.messages, isWaitingResponse: true });
        input.value = "";
        MessagesService.textSend(messageText, MessagesService.previewsContext)
            .then(messages => {
                MessagesService.clearWaitResponse();
                var newState: any = { 
                    messages: MessagesService.messages, 
                    hasNewMessages: true, 
                    isWaitingResponse: false,
                    isOpenRoster: false,
                    isRequiredRoster: false
                };
                if (MessagesService.isOpenRoster) {
                    newState.isOpenRoster = MessagesService.isOpenRoster;
                    newState.rosterText = MessagesService.rosterText;
                    newState.isRequiredRoster = true;
                }
                this.setState(newState);
                this.focusTextInput();
            })
            .catch(error => {
                MessagesService.clearWaitResponse();
                this.setState({ messages: MessagesService.messages, hasNewMessages: true, isWaitingResponse: false });
                this.focusTextInput();
            });
    }


    showSelect = () => {
        let input = this.getTextInput();
        this.setState({ savedMessage: input.value, isOpenRoster: "E", hasNewMessages: false, rosterText: "Выберите сотрудника" });
    }

    createMessage = (message: IMessageDto): JSX.Element => {
        return (
            <Message key={message.key}
                id={message.id}
                date={message.date}
                name={message.name}
                fromMe={message.fromMe}
                text={message.text}
                rate={message.rate}
                context={message.context}
                likeValue={message.likeValue}
                isLikeable={message.isLikeable}
                onButtonClick={this.onButtonSend}
                onLikeClick={this.onLikeSend} />
        );
    }

    createPopupMenu = (): JSX.Element => {
        return (
            <div className="chat-bot__chat-big-popup-menu">
                <div className="chat-bot__chat-big-popup-menu-item" onClick={this.writeToHelpDesk}>
                    <img className="chat-bot__chat-big-popup-menu-item-icon"
                        src={`${Config.ImagesBase}/mail.png`} />
                    <span className="chat-bot__chat-big-popup-menu-item-text">Обратная связь</span>
                </div>
            </div>
        );
    }

    handleInitSelect(el) {
        if (el) {
            el.inputInitComplete = (input: any) => {
                //console.log(2, input);
                if (input) {
                    input.addEventListener('keydown', (e) => this.selectKeyDown(e));
                    input.focus();
                }
            }

            if (el.handleClick) {
                //console.log(1);
                el.handleClick();
            }
        }
    }

    handleInitInput(el) {

        this.userMessage = el;
        let input = this.getTextInput();

        if (input) {
            input.value = this.state.savedMessage;
            input.addEventListener('keydown', (e) => this.textKeyDown(e));
            this.focusTextInput();
        }
    }

    createInput = (): JSX.Element => {
        if (this.state.isOpenRoster) {
            return (
                <div className="chat-bot__chat-big-input">
                    <RUIC.Select
                        ref={(el) => this.handleInitSelect(el)}
                        placeholder = {this.state.rosterText} //"Выберите сотрудника"
                        caption = {this.state.rosterText}
                        onSelect={this.handleSelectEmployee}
                        maxHeight={140}
                        maxWidth={388}
                        query={(q, skip, take, c) => { EmployeeFindService.find(q, skip, take, this.state.isOpenRoster).then(items => c(items)) }}
                    />
                </div>
            );
        }

        return (
            <div className="chat-bot__chat-big-input">
                <img src={`${Config.ImagesBase}/alternate_email.svg`} className="chat-bot__chat-big-input-dog"  title="Узнать о сотруднике"
                    onClick={this.showSelect} />
                <input name="message-to-send2" id="message-to-send2" className="with-dog"
                    placeholder="Введите сообщение..."
                    rows={1}
                    ref={(el) => this.handleInitInput(el)}
                    autoFocus
                    disabled={this.state.isWaitingResponse}></input>
                <img src={`${Config.ImagesBase}/send.svg`} className="chat-bot__chat-big-input-send" title="Отправить вопрос"
                    onClick={this.send} />
            </div>
        );
    }

    private onNeedHistory = (): void => {

        if (this.freezeUploadHistory) {
            return;
        }

        this.freezeUploadHistory = true;
        let lastMessageId = MessagesService.getBotTopMessageId();

        MessagesService.uploadHistoryMessages(lastMessageId > 0 ? lastMessageId : 100000000)
            .then(messages => {
                this.setState({ messages: MessagesService.messages, hasNewMessages: false, isWaitingResponse: false });
                this.freezeUploadHistory = false;
            })
            .catch(error => {
                this.freezeUploadHistory = false;
            });
    }

    private focusTextInput = (): void => {

        if (!this.userMessage) {
            return;
        }
        //console.log(3,this.userMessage);
        this.userMessage.focus();
    }

    private handleSelectEmployee = (item?: ISelectItem): void => {

        let message = item ? `[${item.source}:${item.id}|${item.title}] ` : ``;

        if (this.state.isRequiredRoster) {
            var fullMessage = MessagesService.previewsContext + " " + message;
            this.onButtonSend(fullMessage, fullMessage, message, true);
        } else {
            this.setState({ savedMessage: message, isOpenRoster: "", isRequiredRoster: false });
            this.focusTextInput();
        }
    }

    view() {
        let messages = this.state.messages.length && this.state.messages.map(this.createMessage) || null;
        let popupmenu = this.state.isContextMenuOpen && this.createPopupMenu() || null;
        let input = this.createInput();

        return (
            <div className="chat-bot__chat-big" onClick={this.tryClosePopupMenu}>
                <div className="chat-bot__chat-big-top"></div>
                <div className="chat-bot__chat-big-header">
                    <span className="chat-bot__chat-big-header-caption">{Config.ChatWithBotName}</span>
                    <img src={`${Config.ImagesBase}/ic_close.svg`} className="chat-bot__chat-big-header-close"
                        onClick={this.props.onClick} />
                    <img src={`${Config.ImagesBase}/ic-close-copy.svg`} className="chat-bot__chat-big-header-ic-close-copy"
                        onClick={(e: React.MouseEvent<HTMLElement>) => this.onMenuClick(e)} />
                </div>
                <div className="chat-bot__chat-big-messages" /*style={{backgroundImage: `url(${Config.ImagesBase}/eve-back.png)`, backgroundRepeat: 'repeat'}}*/>
                    <ListBlock shouldScrollDown={this.state.hasNewMessages} needHistory={this.onNeedHistory}>
                        {messages}
                    </ListBlock>
                </div>
                {input}
                {popupmenu}
            </div>
        );
    }
}