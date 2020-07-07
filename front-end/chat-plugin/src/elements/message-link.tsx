import * as React from 'react';
import MainElement from '../elements/main-element';
import {IMessageLinkProps} from "../interfaces/props/IMessageLinkProps";
import {IMessageLinkState} from "../interfaces/states/IMessageLinkState";

export default class MessageLink extends MainElement<IMessageLinkProps, IMessageLinkState> {

    constructor(props){
        super(props);
    }

    view()
    {
        return (
            <a href='#' onClick={() => this.props.onButtonClick(this.props.category, this.props.context, this.props.text, true)}>{this.props.text}</a>
            );
    }
}