import { IPatternDto } from "../interfaces/IPatternDto";

export interface IPatternsDto extends IPatternDto {
    partitionId?: string;
    categoryName?: string;
}