import * as React from 'react';
import { render } from 'react-dom';
import {
    TextField,
    ButtonBox,
    Switch,
    Grid,
    Row,
    Col,
    Select,
    Teatarea,
    RegistryPlate,
    Paginator,
    IRegistryPlateSpec,
    IRegistryPlateBody,
    IRegistryPlateField,
    ShortInfo,
    Checkbox,
    ButtonTextedWithIcon,
    Confirm
} from "@sbt/react-ui-components";

import { IPatternDto } from "../interfaces/IPatternDto";
import { IWordDto } from "../interfaces/IWordDto";
import { IDropdownItem } from "../interfaces/IDropdownItem";

interface IPatternEditProps {
    pattern: IPatternDto;
    isOpen: boolean;
    isReadOnly: boolean;

    onWordTypeSelect(e: IDropdownItem, word: IWordDto);
    onPatternContextClear(): void;
    onPatternContextFocusOut(value: string);
    onPatternModeClear(): void;
    onPatternModeFocusOut(value: string);
    onPatternOnlyContextChanged(value: any);
    onChangePatternClick();
    onDeletePatternClick(patternId: number): void;
    onPatternSelect(pattern: IPatternDto): void;
  }

interface IPatternEditState {}
  

export class PatternEdit extends React.PureComponent<IPatternEditProps, IPatternEditState> {

    wordTypes: IDropdownItem[] = [
        { id: '0', title: 'глагол' },
        { id: '1', title: 'существительное' },
        { id: '2', title: 'прилагательное' },
        { id: '5', title: 'притяжательное местоимение' },
        { id: '6', title: 'притяжательное возвратное' }
    ];

    getWordForms = (word: IWordDto): string => {
        var res = "";
        if (word.wordForms) {
            word.wordForms.forEach(x => res += x.form + " ");
        }
        return res;
    }

    getWordTypes = (
        searchStrig: string,
        skip: number,
        take: number,
        callback: (items: IDropdownItem[]) => void): void => {
        callback(this.wordTypes);
    }

    getSelectedWordTypeItem = (word: IWordDto): IDropdownItem => {
        var type = this.wordTypes.filter(x => x.id == word.wordTypeId.toString());
        if (type.length == 0) return { id: "-1", title: "-" };
        return type[0];
    }

    getWords(pattern) {
        if (!pattern) return null;

        let items: IWordDto[] = pattern.words;

        var res = [];

        items.forEach(value => {
            res.push(<Row key={"wordName" + value.id}>
                <Col baseSize={2} breakpointSizes={['md-6', 'sm-12']}>
                    <div className={"word-name-text"}>{value.wordName}</div>
                </Col>
                <Col baseSize={3} breakpointSizes={['md-6', 'sm-12']}>
                    <Select
                        maxHeight={200}
                        maxWidth={230}
                        isRequired={false}
                        onSelect={(e: IDropdownItem) => { this.props.onWordTypeSelect(e, value); }}
                        query={this.getWordTypes}
                        isDisabled={this.props.isReadOnly}
                        selectedItem={this.getSelectedWordTypeItem(value)}
                    />
                </Col>
                <Col baseSize={7} breakpointSizes={['md-6', 'sm-12']}>
                    {this.getWordForms(value)}
                </Col>
            </Row>);
        });

        res.push(
            <Row key="context">
                <Col baseSize={2} breakpointSizes={['md-6', 'sm-12']}>
                    <div className={"pattern-context-text"}>Режим</div>
                </Col>
                <Col baseSize={2} breakpointSizes={['md-6', 'sm-12']}>
                    <div className={"pattern-context-row"}>
                        <TextField
                            hasTooltip={false}
                            placeholder="Введите номер режима"
                            isRequired={false}
                            onClear={this.props.onPatternModeClear}
                            onFocusOut={this.props.onPatternModeFocusOut}
                            isDisabled={this.props.isReadOnly}
                            value={pattern.mode || ""}
                        />
                    </div>
                </Col>
                <Col baseSize={2} breakpointSizes={['md-6', 'sm-12']}>
                    <div className={"pattern-context-text"}>Контекст</div>
                </Col>
                <Col baseSize={3} breakpointSizes={['md-6', 'sm-12']}>
                    <div className={"pattern-context-row"}>
                        <TextField
                            hasTooltip={false}
                            placeholder="Введите контекст"
                            isRequired={false}
                            onClear={this.props.onPatternContextClear}
                            onFocusOut={this.props.onPatternContextFocusOut}
                            isDisabled={this.props.isReadOnly}
                            value={pattern.context || ""}
                        />
                    </div>
                </Col>
                <Col baseSize={3} breakpointSizes={['md-6', 'sm-12']}>
                    <div className={"checkbox-only-context"}>
                        <Checkbox
                            id="onlyContext"
                            isChecked={pattern.onlyContext}
                            isDisabled={this.props.isReadOnly}
                            onChange={this.props.onPatternOnlyContextChanged}
                            title="Только в контексте"
                        />
                    </div>
                </Col>
            </Row>
        );

        return (<div className="category-edit__pattern__active"><Grid>{res}</Grid></div>);
        
    }





    render() {
        let leftBodyFields: IRegistryPlateField[] = [
            {
                caption: null,
                value: this.props.pattern.phrase
            }
        ];

        let body: IRegistryPlateBody = {
            left: leftBodyFields,
            right:
                <div className="category-edit__pattern__button">

                    {this.props.isOpen && !this.props.isReadOnly &&
                        <ButtonTextedWithIcon
                            size="s"
                            title={'Изменить'}
                            isDisabled={false}
                            onClick={this.props.onChangePatternClick}
                            theme={'clear-default'} />
                    }

                    {this.props.isOpen && !this.props.isReadOnly &&
                        <ButtonTextedWithIcon
                            iconCode="delete"
                            size="s"
                            title="Удалить"
                            onClick={(e: any) => this.props.onDeletePatternClick(this.props.pattern.id)}
                            theme={'clear-danger'} />
                    }
                </div>
        };

        let spec: IRegistryPlateSpec = {
            body: body
        };

        var res = (
            <div key={'pattern-'+this.props.pattern.id} className={(this.props.isOpen && !this.props.isReadOnly) ? "category-edit__pattern__active" : ""}>
                <RegistryPlate
                    key={this.props.pattern.id}
                    spec={spec}
                    onClick={() => { this.props.onPatternSelect(this.props.pattern) }}>
                </RegistryPlate>
                {this.props.isOpen &&
                    this.getWords(this.props.pattern)
                }
            </div>);

        return res;
    }
}
