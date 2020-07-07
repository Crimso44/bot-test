import * as React from 'react';
import MainElement from '../elements/main-element';
import {IMessageButtonProps} from "../interfaces/props/IMessageButtonProps";
import {IMessageButtonState} from "../interfaces/states/IMessageButtonState";

export default class MessageButton extends MainElement<IMessageButtonProps, IMessageButtonState> {

    constructor(props){
        super(props);
    }

    view()
    {
        return (
            <button onClick={() => this.props.onButtonClick(this.props.category, this.props.context, this.props.text, true)}>{this.props.text}</button>
            );
    }
}