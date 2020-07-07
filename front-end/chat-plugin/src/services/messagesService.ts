import { IAuthService, IEmployee } from "../interfaces/props/IPluginProps";
import * as Config from "../config/config";
import { VariablesQuery } from "../end-points/controllers/variables-query-controller";
import { PayloadQuery } from "../end-points/controllers/payload-query-controller";
import { MessageHistoryQuery } from "../end-points/controllers/message-history-query-controller";
import {IMessageDto} from "../interfaces/IMessageDto";
import {IMessageHistoryDto} from "../interfaces/IMessageHistoryDto";
import * as _ from "lodash";
import { MessageFormatService } from "./messageFormatService";

class MessagesServiceImpl
{
    authService: IAuthService;
    employee: IEmployee;
    messages: Array<IMessageDto>;
    previewsContext: string;
    isOpenRoster: string;
    rosterText: string;
    helloMessage: string;
    helloPromise: Promise<any>;

    constructor(){
    }

    init = (): void => {
        this.helloMessage = "...";

        this.messages = [{
            key: this.createKey(),
            id: 0,
            date: new Date().toISOString(),
            name: Config.ChatBotName,
            fromMe: false,
            text: this.helloMessage,
            isLikeable: false
        }];

        this.getDefaultHelloMessage();
    }

    getBotTopMessageId = (): number => {

        if(this.messages.length === 1)
        {
            return 0;
        }

        for(let i = 0; i < this.messages.length; i++)
        {
            let msg = this.messages[i];

            // Пропускаем сообщения пользователя и первое сообщение-приветствие с id=0.
            if(msg.fromMe || msg.id < 1)
            {
                continue;
            }

            return msg.id;
        }

        return 0;
    }

    getDefaultHelloMessage = () => {
        this.helloPromise = new Promise<any>((resolve: (value: string) => void, reject: (reason?: any) => void): void =>{
            let askQuery = new VariablesQuery<any, any>(this.authService, Config.ChatBotWebApiBase, "askByButton");
            askQuery.execute(null, { "Title": "hello", "Id": "", "Category": "hello" }).then(response => {
                this.helloMessage = response.data.Title || 
                        (`Привет, ${this.employee.firstName}!<br/>
                        Меня зовут ${Config.ChatBotName}!<br/>
                        Напиши свой вопрос о Компании или рабочих процессах, и я постараюсь ответить на него.`);
            
                for(let i = 0; i < this.messages.length; i++)
                {
                    let msg = this.messages[i];

                    // Пропускаем сообщения пользователя и первое сообщение-приветствие с id=0.
                    if(!msg.fromMe && msg.id < 1)
                    {
                        msg.text = this.helloMessage;
                        break;
                    }
                }
                resolve(this.helloMessage);
            }); 
        })
    }

    createKey = (): string => {
        return Math.random().toString(36).substr(2, 10);
    }

    add = (message: string): void => {
        this.messages.push(
            {
                key: this.createKey(),
                date: new Date().toISOString(),
                name: "",
                fromMe: true,
                text: message,
                isLikeable: false
            });
    }

    buttonSend = (text: string, context: string, category: string): Promise<IMessageDto[]> => {
        return new Promise<any>((resolve: (value: IMessageDto[]) => void, reject: (reason?: any) => void): void =>{

            this.previewsContext = category;

            let askQuery = new VariablesQuery<any, any>(this.authService, Config.ChatBotWebApiBase, "askByButton");

            askQuery.execute(null, { "Title": text, "Id": context, "Category": category, "Source": "Prt" }).then(
                response => {
                    var roster = MessageFormatService.parseRoster(response.data.Title);
                    this.isOpenRoster = roster.isOpenRoster;
                    this.rosterText = roster.rosterText;
                    this.messages.push(
                        {
                            key: this.createKey(),
                            id: response.data.Id,
                            date: new Date().toISOString(),
                            name: Config.ChatBotName,
                            fromMe: false,
                            rate: response.data.Rate,
                            text: response.data.Title || Config.DefaultBotMessage,
                            context: response.data.Context,
                            isLikeable: response.data.IsLikeable
                        });

                    this.previewsContext = response.data.Context;
                
                    resolve(this.messages);
                },
                error => {
                    this.messages.push(
                        {
                            key: this.createKey(),
                            date: new Date().toISOString(),
                            name: Config.ChatBotName,
                            fromMe: false,
                            text: Config.DefaultBotMessage,
                            isLikeable: false
                        });

                    reject(error);
                }
            );
        });
    }

    likeSend = (messageId: number, vote: number): Promise<any> => {
        this.updateLikeValue(messageId, vote);

        return new Promise<any>((resolve: (messageId: number) => void, reject: (reason?: any) => void): void =>{

            let askQuery = new PayloadQuery(this.authService, Config.ChatBotWebApiBase, "setLike");

            askQuery.execute({ "Title": vote.toString(), "Id": messageId }).then(
                response => {
                    resolve(messageId);
                },
                error => {
                    reject(error);
                }
            );
        });
    }

    textSend = (message: string, context: string): Promise<IMessageDto[]> => {
        return this.sendToChatbotService(message, context);
    }

    uploadHistoryMessages = (beforeId: number, count?: number): Promise<IMessageDto[]> => {
        return new Promise<any>((resolve: (value: IMessageDto[]) => void, reject: (reason?: any) => void): void =>{

            let askQuery = new MessageHistoryQuery(this.authService, Config.ChatBotWebApiBase, "history");

            askQuery.execute(beforeId, count).then(
                response => {
                    let history = response.data as IMessageHistoryDto[];

                    history.forEach((value: IMessageHistoryDto) => {
                        let botMessage = this.getBotMessage(value);
                        this.messages.splice(0, 0, botMessage);
                        let userMessage = this.getUserMessage(value);
                        if (userMessage.text)
                            this.messages.splice(0, 0, userMessage);
                    });

                    resolve(this.messages);
                },
                error => {
                    reject(error);
                }
            );
        });
    }

    setWaitResponse = (): void => {
        this.messages.push(
            {
                key: this.createKey(),
                id: -1,
                date: undefined,
                name: undefined,
                fromMe: false,
                text: undefined,
                rate: 1,
                isLikeable: false
            });
    }

    clearWaitResponse = (): void => {
        _.remove(this.messages, function(m) { return m.id === -1; });
    }

    private sendToChatbotService = (message: string, context: string): Promise<IMessageDto[]> =>
    {
        return new Promise<any>((resolve: (value: IMessageDto[]) => void, reject: (reason?: any) => void): void =>
        {
            this.previewsContext = context;
            let askQuery = new VariablesQuery<any, any>(this.authService, Config.ChatBotWebApiBase, "ask");

            askQuery.execute(null, { "Title": message, "Id": context, "Source": "Prt" }).then(
                response => {
                    var roster = MessageFormatService.parseRoster(response.data.Title);
                    this.isOpenRoster = roster.isOpenRoster;
                    this.rosterText = roster.rosterText;
                    this.messages.push(
                        {
                            key: this.createKey(),
                            id: response.data.Id,
                            date: new Date().toISOString(),
                            name: Config.ChatBotName,
                            fromMe: false,
                            rate: response.data.Rate,
                            text: response.data.Title || Config.DefaultBotMessage,
                            context: response.data.Context,
                            isLikeable: response.data.IsLikeable
                        });

                    this.previewsContext = response.data.Context;
                    resolve(this.messages);
                },
                error => {
                    this.messages.push(
                        {
                            key: this.createKey(),
                            date: new Date().toISOString(),
                            name: Config.ChatBotName,
                            fromMe: false,
                            text: Config.DefaultBotMessage,
                            isLikeable: false
                        });

                    reject(error);
                }
            );
        });
    }

    private updateLikeValue = (messageId: number, vote: number): void => {
        let message = _.find(this.messages, function(m) { return m.id === messageId; });

        if(!message)
        {
            return;
        }

        message.likeValue = (vote === message.likeValue) ? 0 : vote;
    }

    private getUserMessage = (value: IMessageHistoryDto) : IMessageDto => {

        return {
            key: this.createKey(),
            date: value.QuestionDate,
            name: "",
            fromMe: true,
            text: value.Question && (value.Question[0] == '[' && value.Question[value.Question.length-1] == ']' ||
                (value.OriginalQuestion && value.OriginalQuestion[0] == '(' && value.OriginalQuestion[value.OriginalQuestion.length-1] == ')')) ?
                value.Question : value.OriginalQuestion,
            isLikeable: false
        };
    }

    private getBotMessage = (value: IMessageHistoryDto) : IMessageDto => {

        return {
            key: this.createKey(),
            id: value.Id,
            date: value.QuestionDate,
            name: Config.ChatBotName,
            fromMe: false,
            rate: value.Rate,
            text: value.AnswerText,
            context: value.Context,
            likeValue: value.Like,
            isLikeable: value.Answer && !this.hasAnswerTextXbutton(value)
        }
    }

    private hasAnswerTextXbutton = (value: IMessageHistoryDto): boolean => {
        return !!value.AnswerType || value.Answer == "dislike";
    }
}

export const MessagesService = new MessagesServiceImpl();