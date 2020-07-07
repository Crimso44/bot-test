import * as React from "react";
import MainElement from "./main-element";
import {DictionaryService} from "../services/DictionaryService";
import {ISubpartDropdownItem} from "../interfaces/ISubpartDropdownItem";
import {
    Grid,
    Row,
    Col,
    Select,
    IDropdownItem
} from "@sbt/react-ui-components";


interface IPartSubpartProps {
    onClick?: Function;
    PartId?: string;
    PartCaption?: string;
    SubpartId?: string;
    SubpartCaption?: string;
    IsRequiredPart?: boolean;
    IsRequiredSubpart ?: boolean;
    IsDisabled?: boolean;
}

interface IPartSubpartState {
}
export default class PartSubpart extends MainElement<IPartSubpartProps, IPartSubpartState>{


componentWillReceiveProps(nextProps: IPartSubpartProps) {
}
    state : IPartSubpartState = {
    };
    
    static defaultProps: Partial<IPartSubpartProps> = {
    }

    constructor(props) {
        super(props);
        this.onPartSelect = this.onPartSelect.bind(this);
        this.onSubpartSelect = this.onSubpartSelect.bind(this);
    }

    onPartSelect = (item: IDropdownItem): void => {
        this.props.onClick && this.props.onClick(
            item && item.id || null,
            item && item.title || null, 
            null, 
            null);
    }

    onSubpartSelect = (item: ISubpartDropdownItem): void => {
        var newState = {
            SubpartId: item && (item as IDropdownItem).id || null,
            SubpartCaption: item && item.source.title || null,
            PartId: this.props.PartId,
            PartCaption: this.props.PartCaption
        };
        if (item && (item as IDropdownItem).id) {
            newState.PartId = item.source && item.source.parentId || null;
            newState.PartCaption = item.source && item.source.parentTitle || null;
        }
        this.props.onClick && this.props.onClick(
            newState.PartId,
            newState.PartCaption, 
            newState.SubpartId, 
            newState.SubpartCaption);
    }
    view(){
        return (
            <Row>
                <Col baseSize={6} breakpointSizes={['md-6', 'sm-12']}>
                    <Select
                        title="Раздел"
                        placeholder="Выберите раздел"
                        maxHeight={200}
                        isRequired={this.props.IsRequiredPart}
                        onSelect={this.onPartSelect}
                        isDisabled={this.props.IsDisabled}
                        query={DictionaryService.onPartitionQuery}
                        selectedItem={this.props.PartId && {id: this.props.PartId, title: this.props.PartCaption} || null}
                        />
                </Col>
                <Col baseSize={6} breakpointSizes={['md-6', 'sm-12']}>
                    <Select
                        title="Подраздел"
                        placeholder="Выберите подраздел"
                        maxHeight={200}
                        isRequired={this.props.IsRequiredSubpart}
                        onSelect={this.onSubpartSelect}
                        isDisabled={this.props.IsDisabled}
                        query={(search: string, skip: number, take: number, callback: (items: IDropdownItem[]) => void): void =>
                            DictionaryService.onSubpartitionQuery(search, this.props.PartId, skip, take, callback)}
                        selectedItem={this.props.SubpartId && {id: this.props.SubpartId, title: this.props.SubpartCaption} || null}
                        />
                </Col>
            </Row>
        );
    }
}