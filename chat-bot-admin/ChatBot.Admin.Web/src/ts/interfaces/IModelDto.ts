export interface IModelDto {
    id: string;
    createDate: Date;
    answerDate: Date;
    markup?: number;
    accuracy?: number;
    precision?: number;
    recall?: number;
    f1?: number;
    isActive: boolean;
    report?: IModelReportDto[];
}


export interface IModelReportDto {
    id: string;
    modelLearningId: string;
    categoryId?: string;
    categoryName?: string;
    partitionId?: string;
    upperPartitionId?: string;
    markup?: number;
    accuracy?: number;
    precision?: number;
    recall?: number;
    f1?: number;
    confusionFrom?: IModelReportConfDto[];
    confusionTo?: IModelReportConfDto[];
}

export interface IModelReportConfDto {
    originId?: string;
    categoryId?: string;
    categoryName?: string;
    confusion?: number;
    questions?: string[];
}

