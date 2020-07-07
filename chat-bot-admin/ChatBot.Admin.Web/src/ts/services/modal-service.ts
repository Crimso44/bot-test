import * as React from 'react';
import * as ReactDOM from 'react-dom';
import * as _ from 'lodash';

var createReactClass = require('create-react-class');

const CONTAINER_CLASS_NAME = 'modal-container';
const FADE_CLASS_NAME = 'modal-back';
const MODAL_CLASS_NAME = 'modal modal_modal-form';

interface IModal<E> {
    handler: JSX.Element;
    close: Promise<any>;
    error: Promise<any>;
    setState(state: IModalState<E>): void;
    setExternal(external: E): void;
}

export interface IModalProperties<E = any> {
    content?: any;
    externalProps?: E;
    showFade?: boolean;
    onEvent?: Function;
    containerProps?: {
        className?: string;
        style?: any;
    }
    fadeProps?: {
        className?: string;
        style?: any;
    }
    modalProps?: {
        className?: string;
        style?: any;
    }
}

export interface IModalState<E = any> {
    externalProps?: E,
    style?: any,
    className?: string,
    fadeClassName?: string,
    fadeStyle?: any,
    modalClassName?: string,
    modalStyle?: any,
    showFade?: boolean
}

const defaultProperties: IModalProperties = {
    showFade: true
}

export interface IModalService<E = any> {
    open(properties?: IModalProperties<E>): IModal<E>;
    close(): void;
    createDefaultModalContainer(): React.ClassicComponentClass<any>;
}

export class ModalService<E = any> {

    private _properties: IModalProperties<E>;
    private _modal: Element;
    constructor(properties?: IModalProperties<E>) {
        this._properties = _.assign({}, defaultProperties, properties);
    }

    private createDefaultModalContainer(properties?: IModalProperties<E>): React.ClassicComponentClass<any> {
        let container = React.createElement<any, any>('div');
        let containerBack = React.createElement<any, any>('div');
        return createReactClass({
            render: function () {
                return React.cloneElement(container, {
                    className: this.props.className,
                    style: this.props.style
                },
                    [
                        this.props.showFade && React.cloneElement(containerBack, {
                            key: 'fade',
                            className: this.props.fadeClassName,
                            style: this.props.fadeStyle,
                            onClick: (e) => {
                                e.stopPropagation();
                                this.props.onClose && this.props.onClose(false);
                            }
                        }),
                        React.cloneElement(this.props.children, {
                            key: 'modal',
                            className: this.props.modalClassName,
                            style: this.props.modalStyle
                        })
                    ]);
            }
        })
    }

    open(properties: IModalProperties<E> = this._properties): IModal<E> {
        if (!this._modal) {
            let className = `${CONTAINER_CLASS_NAME} ${properties && properties.containerProps && properties.containerProps.className || ''}`;
            let style = properties && properties.containerProps && properties.containerProps.style;
            let fadeClassName = `${FADE_CLASS_NAME} ${properties && properties.fadeProps && properties.fadeProps.className || ''}`;
            let fadeStyle = properties && properties.fadeProps && properties.fadeProps.style;
            let modalClassName = `${MODAL_CLASS_NAME} ${properties && properties.modalProps && properties.modalProps.className || ''}`;
            let modalStyle = properties && properties.modalProps && properties.modalProps.style;
            let container = React.createElement(this.createDefaultModalContainer(properties));
            this._modal = document.createElement("div");
            let content = properties.content;
            let handler, setExternal, setState;

            let modalPromise = new Promise((resolve, reject) => {
                let Modal = createReactClass({
                    getInitialState: function () {
                        return {
                            externalProps: properties.externalProps,
                            style: style,
                            className: className,
                            fadeClassName: fadeClassName,
                            fadeStyle: fadeStyle,
                            modalClassName: modalClassName,
                            modalStyle: modalStyle,
                            showFade: properties.showFade
                        };
                    },
                    onClose: (val) => { resolve(val), this.close(); },
                    onError: (val) => { reject(val), this.close(); },
                    onEvent: (val) => properties.onEvent && properties.onEvent(val),
                    render: function () {
                        let content = React.createElement(properties.content, {
                            externalProps: this.state.externalProps,
                            onClose: this.onClose,
                            onError: this.onError,
                            onEvent: this.onEvent,
                            className: this.state.modalClassName,
                            style: this.state.modalStyle
                        });
                        return React.cloneElement(container,
                            {
                                externalProps: properties.externalProps,
                                className: this.state.className,
                                style: this.state.style,
                                fadeClassName: this.state.fadeClassName,
                                fadeStyle: this.state.fadeStyle,
                                modalClassName: this.state.modalClassName,
                                modalStyle: this.state.modalStyle,
                                showFade: this.state.showFade,
                                onClose: this.onClose,
                            },
                            content
                        );
                    }
                });
                handler = ReactDOM.render<any, any>(React.createElement(Modal), this._modal);
                setExternal = (externalProps: E) => handler.setState({ externalProps: externalProps });
                setState = (state: IModalState<E>) => handler.setState(state);
                document.body.appendChild(this._modal);
            });

            return { handler: handler, close: modalPromise, error: modalPromise, setState: setState, setExternal: setExternal };
        }
    }

    close() {
        if (this._modal) {
            let res = ReactDOM.unmountComponentAtNode(this._modal);
            if (res) {
                this._modal.remove();
            }
        }
    }
}