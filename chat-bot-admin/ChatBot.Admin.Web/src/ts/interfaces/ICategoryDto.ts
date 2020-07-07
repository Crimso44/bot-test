import { IPartitionDto } from "../interfaces/IPartitionDto";
import { IPatternDto } from "../interfaces/IPatternDto";
import { ILearningDto } from "../interfaces/ILearningDto";

export interface ICategoryDto {
    id: number;
    name: string;

    response:string;

    setContext: string;

    isDefault?: boolean;
    isTest?: boolean;
    isDisabled?: boolean;
    isChanged?: boolean;
    isAdded?: boolean;
    isIneligible?: boolean;
    requiredRoster?: string;
    requiredRosterName?: string;
    parentId?: number;

    partitionId?: string;
    partition?: IPartitionDto;
    upperPartition?: IPartitionDto;
    originId?: string;

    changedOn?: string;
    changedBy?: string;
    changedByName?: string;
    publishedOn?: string;

    patterns: IPatternDto[];
    //learnings: ILearningDto[];

    learningCount?: number;
}