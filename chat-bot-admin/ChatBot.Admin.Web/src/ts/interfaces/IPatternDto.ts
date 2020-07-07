import { IWordDto } from "../interfaces/IWordDto";

export interface IPatternDto {
    id: number;
    categoryId: number;

    context: string;
    onlyContext: boolean;

    phrase: string;

    wordCount: number;

    words: IWordDto[];
}