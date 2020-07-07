import { ICollectionDto } from "../interfaces/ICollectionDto";

export interface ILoadPageResponseDto<T>{
    pageNumber: number;
    data: ICollectionDto<T>;
}