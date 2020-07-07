import { IPatternDto } from "../IPatternDto";

export interface IEditCategoryCommand {
    id : number;
    partitionId?: string;
    name?: string;
    response?:string;
    setContext?: string;
    patterns?: IPatternDto[];
    isChangedPatterns?: boolean;
    isIneligible?: boolean;
    isDisabled?: boolean;
    requiredRoster?: string;
}