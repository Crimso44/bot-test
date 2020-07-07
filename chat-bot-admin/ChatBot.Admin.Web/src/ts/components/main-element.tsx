import * as React from 'react';
import * as Rx from 'rxjs';

declare var require;
var PureRenderMixin = require('react-addons-pure-render-mixin');

export interface IMainElement<P, S> extends React.ComponentLifecycle<P, S> {
    view(props?: P, state?: S, template?: { [key: string]: JSX.Element } | JSX.Element): JSX.Element;
    stream: Rx.Subject<any>;
    streamReader: Rx.Observable<any>;
}

export interface IMainElementState { }

export interface IMainElementProps extends React.Props<MainElement<any, any>> {
    itemKey?: string | number;

    stream?: Rx.Subject<any>;

}

export default class MainElement<P, S> extends React.Component<IMainElementProps & P, IMainElementState & S> {

    stream = new Rx.Subject<any>();

    streamReader: Rx.Observable<any>;

    readonly state: Readonly<IMainElementState & S> = null;

    public static defaultProps = null;

    refs: {
        [key: string]: (Element);
    };

    context: any = {};

    constructor(props, autoSCU = true) {
        super(props);
        if (autoSCU) {
            this.shouldComponentUpdate = PureRenderMixin.shouldComponentUpdate.bind(this);
        }
    }

    componentWillReceiveProps(nextProps: IMainElementProps & P) { }

    shouldComponentUpdate(nextProps: IMainElementProps & P, nextState: IMainElementState & S) {
        return true;
    }

    componentDidUpdate(prevProps: IMainElementProps & P, prevState: IMainElementState & S) { 
        
    }

    componentDidMount() { 
        if (this.props.stream) {
            this.streamReader = this.props.stream.asObservable();
        }
    }

    componentWillUnmount() {
        if (this.stream && !!this.stream.unsubscribe) {
            this.stream.unsubscribe();
        }
        if (this.streamReader) {
            this.streamReader = null;
        }
    }
    view(props = this.props, state = this.state, template = null): JSX.Element {
        return (
            <div>
                {props.children}
            </div>
        );
    }

    render(): JSX.Element {
        return this.view();
    }
}

// пример использования template

interface IBaseListItemProps<T>{
    item?: T;
} 

interface IITem{
    name: string;
    phone: string;
} 

class BaseListItem<T> extends MainElement<IBaseListItemProps<T>, any>{
    constructor(props) {
        super(props);
    }

    view(props, state, template) {
        return (
            <div>
                <div className={'head'}>{template.head}</div>
                <div className={'body'}>{template.body}</div>
                <div className={'footer'}>{template.footer}</div>
            </div>
        );
    }
}

class ConcreteListItem extends BaseListItem<IITem> {
    constructor(props) {
        super(props);
    }

    view(props = this.props, state, template) {
        var body = (<div></div>);
        return super.view(props, state, { body: body });
    }
}