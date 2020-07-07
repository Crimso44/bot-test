
export interface IMessageDto {
    key: string;
    id?:number;
    date: string;
    name: string;
    fromMe: boolean;
    rate?: number;
    text: string;
    context?: string;
    likeValue?: number;
    isLikeable?: boolean;
}