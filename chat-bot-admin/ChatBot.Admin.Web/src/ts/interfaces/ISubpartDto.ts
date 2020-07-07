
export interface ISubpartDto {
    id: string;
    parentId: string;

    title: string;
    fullTitle: string;
    parentTitle: string;

    categoryCount: number;
    categoryPublishedCount: number;
    learningCount: number;
}