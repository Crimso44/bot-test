import * as React from "react";
import { ButtonBox} from "@sbt/react-ui-components";

interface IModalInfoFormProps {
    externalProps: any;
    onClose: (val: any) => void;
    onError: (val: any) => void;
    onEvent: (val: any) => void;
}

interface IModalInfoFormState {
}

export default class ModalInfoForm extends React.Component<IModalInfoFormProps, IModalInfoFormState> {

    constructor(props) {
      super(props);
    }

    render() {

      return (
          <div className={'info-form-container'}>
            <div className={'caption'}>{this.props.externalProps.text}</div>
            <div className={'buttons'}>
                <div>
                    <ButtonBox
                        title="Ok"
                        onClick={() => this.props.onClose(true)}
                        size="s"
                        />
                </div>
            </div>
          </div>
        );
    }
  }
