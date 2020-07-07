import { IWordFormDto } from "../interfaces/IWordFormDto";

export interface IWordDto {
    id: number;
    patternId: number;

    key?: string;

    wordName: string;

    wordTypeId: number;

    wordForms: IWordFormDto[];
}