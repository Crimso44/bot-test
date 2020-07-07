import { IMessageDto } from "../IMessageDto";

export interface IChatBigState {
    messages: Array<IMessageDto>;
    hasNewMessages: boolean;
    isWaitingResponse: boolean;
    isContextMenuOpen: boolean;
    isOpenRoster: string;
    savedMessage: string;
    rosterText: string;
    rosterContext: string;
    isRequiredRoster: boolean;
}