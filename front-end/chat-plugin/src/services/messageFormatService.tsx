import * as React from 'react';
import MessageButton from "../elements/message-button";
import MessageLink from "../elements/message-link";

export interface IParseRoster {
    isOpenRoster?: string;
    rosterText?: string;
}

class MessageFormatServiceImpl {

    parse = (text: string, context: string, onButtonClick: (category: string, context: string, text: string, isAddQuestion: boolean) => void): JSX.Element => {

        let nodes: Array<JSX.Element> = [];
        let parser = new DOMParser();
        let doc = parser.parseFromString(text, "text/html");
        let childNodes = doc.body.childNodes;

        for(let i = 0; i < childNodes.length; i++)
        {
            let childNode = childNodes[i];

            if(childNode.localName === "xbtn")
            {
                nodes.push(
                    <MessageButton
                        key={i.toString()}
                        category={childNode.attributes["category"].nodeValue}
                        context={context}
                        text={childNode.textContent}
                        onButtonClick={onButtonClick}/>
                );

                continue;
            }

            if(childNode.localName === "xlnk")
            {
                nodes.push(
                    <MessageLink
                        key={i.toString()}
                        category={childNode.attributes["category"].nodeValue}
                        context={childNode.attributes["context"].nodeValue}
                        text={childNode.textContent}
                        onButtonClick={onButtonClick}/>
                );

                continue;
            }

            if(childNode.localName === "xpre")
            {
                continue;
            }
    

            if(childNode.localName === "xrst")
            {
                continue;
            }
    

            nodes.push(React.createElement(
                childNode.localName || "span",
                this.buildPropsFromAttributes(childNode, i.toString()),
                this.getTextContent(childNode.textContent))
            );
        }

        return (
            <div>{nodes}</div>
        );
    }

    getPreFillValue = (text: string): string => {

        let nodes: Array<JSX.Element> = [];
        let parser = new DOMParser();
        let doc = parser.parseFromString(text, "text/html");
        let childNodes = doc.body.childNodes;

        for(let i = 0; i < childNodes.length; i++)
        {
            let childNode = childNodes[i];

            if(childNode.localName === "xpre")
            {
                return childNode.textContent;
            }
    
        }

        return "";
    }



    parseRoster = (text: string): IParseRoster => {

        var res: IParseRoster = {
            isOpenRoster: null,
            rosterText: null
        };

        if (!text) return res;

        let parser = new DOMParser();
        let doc = parser.parseFromString(text, "text/html");
        let childNodes = doc.body.childNodes;

        for(let i = 0; i < childNodes.length; i++)
        {
            let childNode = childNodes[i];

            if(childNode.localName === "xrst")
            {
                res.isOpenRoster = childNode.attributes['type'].nodeValue;
                doc.body.removeChild(childNode);
                res.rosterText = doc.body.textContent;
            }
        }

        return res;
    }

    private getTextContent = (content: string): string => {
        return (content && content.length > 0) ? content : undefined;
    }

    private buildPropsFromAttributes = (node: Node, key: string): any => {

        var obj = { key: key };

        if(node.attributes)
        {
            for(let i = 0; i < node.attributes.length; i++)
            {
                let attr = node.attributes[i];
                obj[this.getAttrName(attr)] = attr.nodeValue;
            }
        }

        return obj;
    }

    private getAttrName = (attr: Attr): any => {

        if(attr.nodeName === "class")
            return "className";

        return attr.nodeName;
    }
}

export const MessageFormatService = new MessageFormatServiceImpl();