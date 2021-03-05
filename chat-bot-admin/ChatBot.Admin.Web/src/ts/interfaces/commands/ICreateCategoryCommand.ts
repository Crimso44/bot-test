import { IPatternDto } from "../IPatternDto";

export interface ICreateCategoryCommand {
    partitionId?: string;
    
    name: string;
    response:string;
    setContext: string;
    setMode?: string;
    isIneligible: boolean;
    isDisabled: boolean;
    requiredRoster?: string;

    patterns?: IPatternDto[];
}