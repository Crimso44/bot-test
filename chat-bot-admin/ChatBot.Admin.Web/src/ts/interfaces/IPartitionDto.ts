
export interface IPartitionDto {
    id: string;

    title: string;
    fullTitle?: string;

    subpartitions?: IPartitionDto[];

    subpartitionCount?: number;
    categoryCount?: number;
    categoryPublishedCount?: number;
    learningCount?: number;
}