import * as React from "react";
import { withRouter } from "react-router";
import { connect } from "react-redux";
import MainElement from "./main-element";
import ListHeader from "./categoryHeader";
import { IPortalState } from '../interfaces/IPortalState';
import { IPartitionDto } from '../interfaces/IPartitionDto';
import { ISubpartDto } from '../interfaces/ISubpartDto';
import * as Scenarios from "../actions/scenarios";
import { StoreService } from '../services/StoreService';
import TopMenu from "./topMenu";
import * as Const from "../const/constants";
import ModalConfirmForm from "./modalConfirmForm";
import {IDeletePartCommand} from "../interfaces/commands/IDeletePartCommand";
import * as GuidService from "../services/GuidService";
import {
    RegistryPlate,
    Paginator,
    IRegistryPlateSpec,
    IRegistryPlateBody,
    IRegistryPlateHeader,
    IRegistryPlateField,
    IRegistryPlateFieldValue,
    ButtonBox,
    ButtonIcon,
    ButtonTextedWithIcon,
    IconFont,
    IconSvg,
    SvgPathes
} from "@sbt/react-ui-components";

interface IPartListProps {
    items?: IPartitionDto[];
    subitems?: ISubpartDto[];
    isLoading?: boolean;
    loadingError?: string;
    selectedPartId?: string;
    selectedPartCaption?: string;
}

interface IPartListState {
}

class PartListApp extends MainElement<IPartListProps, IPartListState>{

    constructor(props) {
        super(props);
    }

    componentDidMount()
    {
        Scenarios.loadPartitionListPage(null);
    }

    onPartItemEditClick = (id: string): void => {
        Scenarios.editPartitionItem(id);
    }

    onPartItemDeleteClick = (id: string, name: string): void => {
        var modal = StoreService.ms({content: ModalConfirmForm, externalProps: {text: "Удалить раздел " + name + " ?"}});
        var handler = modal.open();
        handler.close.then(val => this.onDeleteConfirmation(val, id));
    }

    onDeleteConfirmation = (confirmed: boolean, id: string): void => {

        if(!confirmed)
            return;

        let payload: IDeletePartCommand = { id: id };

        // Сохраняем изменения
        Scenarios.deletePartitionItem(GuidService.getNewGUIDString(), payload);
    }

    onSubpartItemDeleteClick = (id: string, name: string): void => {
        var modal = StoreService.ms({content: ModalConfirmForm, externalProps: {text: "Удалить подраздел " + name + " ?"}});
        var handler = modal.open();
        handler.close.then(val => this.onDeleteSubConfirmation(val, id));
    }

    onDeleteSubConfirmation = (confirmed: boolean, id: string): void => {

        if(!confirmed)
            return;

        let payload: IDeletePartCommand = { id: id };

        // Сохраняем изменения
        Scenarios.deleteSubpartItem(GuidService.getNewGUIDString(), payload);
    }

    onSubpartItemClick = (id: string): void => {
        Scenarios.editSubpartItem(id);
    }

    onPartItemClick = (id: string, caption: string): void => {
        Scenarios.loadSubpartListPage(null, id, caption);
    }
    onViewCategoryClick = (partId: string, partCaption: string, subPartId: string, subPartCaption: string): void => {
        Scenarios.viewCategoryList(partId,partCaption,subPartId,subPartCaption);
    }

    buildRegistryPlateItems = () : RegistryPlate[] => {

        let items: IPartitionDto[] = this.props.items;

        return items.map(value => {

            let leftBodyFields: IRegistryPlateField[] = [
                {
                    caption: null,
                    value: value.title || "-"
                }
            ];

            let body: IRegistryPlateBody = {
                left: leftBodyFields,
                right: null
            };

            
            let footer = 
            <div className="subgroup-footer">
            <div>
                <ButtonTextedWithIcon 
                    size="s"
                    onClick={(e: any) => this.onPartItemClick(value.id, value.title)}
                    title={'Подразделы ('+value.subpartitionCount+')'}
                    theme={'clear-default'}/>
                <ButtonTextedWithIcon 
                    size="s"
                    title={'Категории ('+value.categoryCount+')'}
                    isDisabled={value.categoryCount == 0}
                    onClick={(e: any) => this.onViewCategoryClick(value.id, value.title, null, null)}
                    theme={'clear-default'}/>
                {value.learningCount ? <ButtonTextedWithIcon 
                    size="s"
                    title={'Выборка ML ('+value.learningCount+')'}
                    isDisabled={true}
                    theme={'clear-default'}/> : undefined}
            </div>
            {(!value.subpartitionCount && !value.categoryCount) && <ButtonTextedWithIcon 
                    iconCode="delete"    
                    size="s"
                    title="Удалить"
                    onClick={(e: any) => this.onPartItemDeleteClick(value.id, value.title)}
                    theme={'clear-danger'}/>
            }
            </div>
            ;

            let spec: IRegistryPlateSpec = {
                body: body,
                footer: footer
            };

            
            return (
                <RegistryPlate
                    key={value.id}
                    spec={spec}
                    isSelected={(this.props.selectedPartId == value.id)}
                    canSelectable={(this.props.selectedPartId == value.id)}
                    onClick={(e: any) => this.onPartItemEditClick(value.id)}
                    id={value.id} />
            );
        });
    }

    buildRegistryPlateSubItems = () : RegistryPlate[] => {
        let items: ISubpartDto[] = this.props.subitems;
        
        return items.map(value => {

            /*let header: IRegistryPlateHeader = {
                title: value.caption
            };*/

            let leftBodyFields: IRegistryPlateField[] = [
                {
                    caption: null, //"Раздел:",
                    value: value.title //value.parentGroupCaption
                }
            ];

            let body: IRegistryPlateBody = {
                left: leftBodyFields
               
            };

            let footer = 
               <div className="subgroup-footer">
                <ButtonTextedWithIcon 
                    size="s"
                    title={'Категории ('+value.categoryCount+')'}
                    isDisabled={value.categoryCount == 0}
                    onClick={(e: any) => this.onViewCategoryClick(null, null, value.id, value.title)}
                    theme={'clear-default'}/>
            
                {value.learningCount ? <ButtonTextedWithIcon 
                    size="s"
                    title={'Выборка ML ('+value.learningCount+')'}
                    isDisabled={true}
                    theme={'clear-default'}/> : undefined}

                {!value.categoryCount && <ButtonTextedWithIcon 
                    iconCode="delete"    
                    size="s"
                    title="Удалить"
                    onClick={(e: any) => this.onSubpartItemDeleteClick(value.id, value.title)}
                    theme={'clear-danger'}/>
                }
            </div>
            ;
            let spec: IRegistryPlateSpec = {
                //header: header,
                body: body,
                footer: footer
            };

            return (
                <RegistryPlate key={value.id}
                    spec={spec}
                    onClick={(e: any) => this.onSubpartItemClick(value.id)}
                    id={value.id} />
            );
        });
    }

    onAddPartClick = (): void => {
        Scenarios.addPartitionItem();
    }

    onAddSubpartClick = (): void => {
        Scenarios.addSubpartItem(this.props.selectedPartId, this.props.selectedPartCaption);
    }
    

    view() {
        var items = this.props.items && this.buildRegistryPlateItems() || null;
        var subitems = this.props.subitems && this.buildRegistryPlateSubItems() || null;

        return (
            <div className={'group-list-container wide'}>
                <TopMenu activeItemId={2}/>
                <div className={'group-list-list'}>
                    <div className={'group-list-button'}>
                        <ButtonTextedWithIcon
                            title="Добавить раздел"
                            onClick={this.onAddPartClick}
                            size="s"
                            />
                        <ButtonTextedWithIcon
                            title="Добавить подраздел"
                            onClick={this.onAddSubpartClick}
                            size="s"
                            />
                    </div>
                    <div className={'list'}>{items}</div>
                </div>
                <div className={'subgroup-list-list'}>
                    <div className={'group-list-select-group'}>
                    {this.props.selectedPartCaption}
                    </div>
                    <div className={'list'}>{subitems}</div>
                </div>
            </div>
        );
    }
}

const mapStateToProps = (state: IPortalState, ownProps: any): IPartListProps => {
    return {
        items: state.partitionList.items,
        subitems: state.partitionList.subitems,
        isLoading: state.partitionList.isLoading,
        loadingError: state.partitionList.loadingError,
        selectedPartId: state.partitionList.selectedPartId,
        selectedPartCaption: state.partitionList.selectedPartCaption
    };
};

export const PartList: any = withRouter(connect(mapStateToProps)(PartListApp));