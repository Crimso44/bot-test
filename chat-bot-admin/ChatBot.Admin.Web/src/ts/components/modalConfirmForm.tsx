import * as React from "react";
import { ButtonBox} from "@sbt/react-ui-components";

interface IModalConfirmFormProps {
    externalProps: any;
    onClose: (val: any) => void;
    onError: (val: any) => void;
    onEvent: (val: any) => void;
}

interface IModalConfirmFormState {
}

export default class ModalConfirmForm extends React.Component<IModalConfirmFormProps, IModalConfirmFormState> {

    constructor(props) {
      super(props);
    }

    render() {

      return (
          <div className={'confirm-form-container'}>
            <div className={'caption'}>{this.props.externalProps.text}</div>
            <div className={'buttons'}>
                <div>
                    <ButtonBox
                        title="Да"
                        onClick={() => this.props.onClose(true)}
                        size="s"
                        />
                </div>
                <div>
                    <ButtonBox
                        title="Отмена"
                        onClick={() => this.props.onClose(false)}
                        size="s"
                        colorTheme="#a5b4dd"
                        />
                </div>
            </div>
          </div>
        );
    }
  }
