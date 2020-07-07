
export interface IMessageHistoryDto {
    Id: number,
    QuestionDate : string,
    SigmaLogin : string,
    Question : string,
    OriginalQuestion : string,
    Answer : string,
    AnswerText : string,
    AnswerType : string,
    Rate? : number,
    SetContext : string,
    Context : string,
    IsButton : boolean,
    Like? : number
}