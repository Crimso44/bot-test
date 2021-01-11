import * as React from "react";
import MainElement from "./main-element";
import {
    ButtonTextedWithIcon,
    ButtonBox,
    IconFont,
    IconSvg,
    SvgPathes,
    Grid,
    Row,
    Col
} from "@sbt/react-ui-components"

interface ICategoryHeaderProps {

    header: string;
    buttonCaption: string;
    readOnly: boolean;
    isChanged?: boolean;
    onClick: () => void;
    onPublishClick: () => void;
    onUnpublishClick: () => void;
    onXlsClick: () => void;
}

interface ICategoryHeaderState {
}

export default class CategoryHeader extends MainElement<ICategoryHeaderProps, ICategoryHeaderState>{

    constructor(props) {
        super(props);
    }

    view(){

        const iconPlus = (
            <IconSvg
              pathEl={SvgPathes.plus.path}
              viewBoxX={SvgPathes.plus.viewBoxX}
              viewBoxY={SvgPathes.plus.viewBoxY}
              width={14}
              height={14}
            />
          );

        return (
            <div className={'list-header'}>
                <div className={'category-list__buttons'}>
                    {!this.props.readOnly && <ButtonBox
                        title={this.props.buttonCaption}
                        icon={iconPlus}
                        onClick={this.props.onClick}
                        size="s"
                        />}
                    <div> 
                        <ButtonBox title="Выгрузить" size="s" onClick={this.props.onXlsClick} colorTheme="#a5b4dd" />&nbsp;&nbsp;&nbsp;&nbsp;
                        {!this.props.readOnly && <ButtonBox title="Опубликовать" size="s" onClick={this.props.onPublishClick} isDisabled={!this.props.isChanged} />}&nbsp;&nbsp;&nbsp;&nbsp;
                        {!this.props.readOnly && <ButtonBox title="Удалить последние изменения" size="s" onClick={this.props.onUnpublishClick} isDisabled={!this.props.isChanged} theme="danger" />}
                    </div>
                </div>
            </div>
        );
    }
}