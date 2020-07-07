import * as Const from "../const/constants";


export const normalize = (value: string): string => {

    if(!value)
        return null;

    value = value.trim();

    return value === "" ? null : value;
}


export const short = (value: string): string => {

    if(!value)
        return null;

    value = value.trim();

    if(value.length <= Const.ShortStringLength)
        return value;

    return value.substr(0, Const.ShortStringLength) + "...";
}
