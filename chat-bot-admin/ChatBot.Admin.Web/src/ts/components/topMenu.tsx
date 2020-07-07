import * as React from "react";
import {StoreService} from "../services/StoreService";
import MainElement from "./main-element";
import * as Const from "../const/constants";
import {
    TextField,
    ButtonBox
} from "@sbt/react-ui-components";


interface ITopMenuProps {
    activeItemId? : number;
}

interface ITopMenuState {
}

export default class TopMenu extends MainElement<ITopMenuProps, ITopMenuState>{
    //props = { activeItemId:1};
    
    static defaultProps: Partial<ITopMenuProps> = {
        activeItemId:1  
    }

    constructor(props) {
        super(props);
    }

    onCategoryListClick = (): void => {
        StoreService.history.push(Const.NavigationPathCategoryList);
    }

    onPartitionListClick = (): void => {
       StoreService.history.push(Const.NavigationPathPartitionList);
    }

    onSettingsClick = (): void => {
       StoreService.history.push(Const.NavigationPathSettings);
    }

    onHistoryClick = (): void => {
       StoreService.history.push(Const.NavigationPathHistory);
    }
 
    onLearningClick = (): void => {
        StoreService.history.push(Const.NavigationPathLearning);
    }
  
    onPatternsClick = (): void => {
        StoreService.history.push(Const.NavigationPathPatterns);
    }
  
    onModelClick = (): void => {
        StoreService.history.push(Const.NavigationPathModel);
    }
  
     view(){

        return (
            <div className="link-top-menu">
                <div key={"link-top-menu-item1"} className={"link-top-menu-item" + (this.props.activeItemId == 1 ?"-active" : "")} onClick={this.onCategoryListClick}>
                    Категории
                </div>
                <div key={"link-top-menu-item2"} className={"link-top-menu-item" + (this.props.activeItemId  == 2 ?"-active" : "")} onClick={this.onPartitionListClick}>
                    Разделы
                </div>
                <div key={"link-top-menu-item3"} className={"link-top-menu-item" + (this.props.activeItemId  == 3 ?"-active" : "")} onClick={this.onSettingsClick}>
                    Настройки
                </div>
                <div key={"link-top-menu-item4"} className={"link-top-menu-item" + (this.props.activeItemId  == 4 ?"-active" : "")} onClick={this.onHistoryClick}>
                    История
                </div>
                <div key={"link-top-menu-item5"} className={"link-top-menu-item" + (this.props.activeItemId  == 5 ?"-active" : "")} onClick={this.onPatternsClick}>
                    Паттерны
                </div>
                <div key={"link-top-menu-item6"} className={"link-top-menu-item" + (this.props.activeItemId  == 6 ?"-active" : "")} onClick={this.onLearningClick}>
                    Обучение
                </div>
                <div key={"link-top-menu-item7"} className={"link-top-menu-item" + (this.props.activeItemId  == 7 ?"-active" : "")} onClick={this.onModelClick}>
                    Модель
                </div>
            </div>
        );
    }
}