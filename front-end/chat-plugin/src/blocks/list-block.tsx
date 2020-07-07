import * as React from 'react';
import * as ReactDOM from 'react-dom';
import MainElement from '../elements/main-element';
import { ScrollService } from "../services/scrollService";
import { IListBlockProps } from "../interfaces/props/IListBlockProps";
import { IListBlockState } from "../interfaces/states/IListBlockState";

const PerfectScrollbar = require('perfect-scrollbar');


declare var require;

export default class ListBlock extends MainElement<IListBlockProps, IListBlockState>{

    ps: any = null;
    scrollService: ScrollService;
    element: HTMLElement;
    scrollTail: number;

    constructor(props) {
        super(props, false);
        this.scrollService = new ScrollService();
    }

    refs: {
        [key: string]: (Element);
        list: HTMLDivElement
    };

    componentDidMount() {
        this.element = ReactDOM.findDOMNode<HTMLElement>(this.refs.list);
        this.element.addEventListener("wheel", this.onWheelEvent);
        this.ps = new PerfectScrollbar(this.element);
        this.scrollService.scrollDown(this.element);
    }

    componentWillUpdate() {
        this.scrollTail = Math.max(this.element.scrollHeight, this.element.clientHeight) - this.element.scrollTop;
    }

    componentDidUpdate() {
        this.ps && this.ps.update();

        if (this.props.shouldScrollDown) {
            this.scrollService.scrollAnimate(this.element, this.element.scrollTop, this.element.scrollHeight - this.element.clientHeight);
        }
        else {
            this.scrollService.scrollTo(this.element, Math.max(this.element.scrollHeight, this.element.clientHeight) - this.scrollTail);
        }
    }

    componentWillUnmount() {
        this.element.removeEventListener("wheel", this.onWheelEvent);
        this.ps && this.ps.destroy();
        this.ps = null;
        super.componentWillUnmount();
    }


    onWheelEvent = (event: WheelEvent): void => {
        if (event.deltaY >= 0) {
            return;
        }

        if (this.element.scrollTop === 0 && this.props.needHistory) {
            this.props.needHistory();
        }
    }

    view(): JSX.Element {
        return (
            <div ref="list" className="chat-bot__chat-big-messages-list">
                <div>
                    {this.props.children}
                </div>
            </div>
        );
    }
}