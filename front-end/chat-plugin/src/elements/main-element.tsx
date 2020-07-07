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

    componentWillMount() {
        if (this.props.stream) {
            this.streamReader = this.props.stream.asObservable();
        }
    }

    componentWillReceiveProps(nextProps: IMainElementProps & P) { }

    shouldComponentUpdate(nextProps: IMainElementProps & P, nextState: IMainElementState & S) {
        return true;
    }

    componentWillUpdate(nextProps: IMainElementProps & P, nextState: IMainElementState & S) { }

    componentDidUpdate(prevProps: IMainElementProps & P, prevState: IMainElementState & S) { }

    componentDidMount() { }

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
