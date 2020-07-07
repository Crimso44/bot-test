import { ILearningDto } from "./ILearningDto";

export interface IHistoryDto {
    id: number;
    questionDate: Date;
    sigmaLogin?: string;
    userName?: string;
    question?: string;
    originalQuestion?: string;
    answer?: string;
    answerText?: string;
    answerType: string;
    rate?: number;
    setContext?: string;
    context?: string;
    isButton?: boolean;
    like?: number;
    categoryOriginId?: string;
    isMto?: boolean;
    mtoThresholds?: string;
    answerGood?: boolean;
    source?: string;

    learns?: ILearningDto[];
}