import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Provider, connect } from 'react-redux';
import { Route, Router, Switch, withRouter } from "react-router";
import { store, history } from './stores/stores';
import { CategoryList } from './components/categoryList';
import { CategoryEdit } from './components/categoryEdit';
import { PartList } from './components/partList';
import { PartEdit } from './components/partEdit';
import {PartAdd} from "./components/partAdd";
import {SubpartEdit} from "./components/subpartEdit";
import {SubpartAdd} from "./components/subpartAdd";
import { Settings } from './components/settings';
import { HistoryList } from './components/historyList';
import { PatternsList } from './components/patternsList';
import { LearningList } from './components/learningList';
import { ModelList } from './components/modelList';
import { reducers } from './reducers/reducers';
import { StoreService } from "./services/StoreService";
import { PermissionService } from "./services/PermissionService";
import { IPortalState } from "./interfaces/IPortalState";
import { IAppPageProps } from "./interfaces/IPortalPage";
import { CHAT_BOT_ADMIN_HOST, CHAT_BOT_ROOT } from "./config";
import * as Const from "./const/constants";
import AuthService from './services/auth-service';
import NotificationService from './services/notification-service';
import { ModalService } from './services/modal-service';
import * as RUIC from "@sbt/react-ui-components";


class MainApp extends React.Component<any, any> {
    constructor(props) {
        super(props);

        StoreService.auth = AuthService;
        StoreService.store = this.props.store;
        StoreService.history = this.props.history;
        StoreService.baseUrl = this.props.baseUrl || Const.NavigationPathCategoryList;
        StoreService.ms = (props) => new ModalService(props);
        StoreService.notification = NotificationService;
        StoreService.dispatch = this.props.dispatch;
        StoreService.servicesApi = this.props.servicesApi || CHAT_BOT_ADMIN_HOST;
        StoreService.isDevelopment = true;

        PermissionService.Init();
    }

    render() {
        return (
                <div className='container'>
                    <div className='container-main'>
                        <Route exact path={Const.NavigationPathCategoryList} component={CategoryList} />
                        <Route exact path={Const.NavigationPathCategoryItemAdd} component={CategoryEdit} />
                        <Route exact path={Const.NavigationPathCategoryItemEdit} component={CategoryEdit} />
                        <Route exact path={Const.NavigationPathPartitionList} component={PartList} />
                        <Route exact path={Const.NavigationPathPartitionItemEdit} component={PartEdit} />
                        <Route exact path={Const.NavigationPathPartitionItemAdd} component={PartAdd} />
                        <Route exact path={Const.NavigationPathSubpartItemEdit} component={SubpartEdit} />
                        <Route exact path={Const.NavigationPathSubpartItemAdd} component={SubpartAdd} />
                        <Route exact path={Const.NavigationPathSettings} component={Settings} />
                        <Route exact path={Const.NavigationPathHistory} component={HistoryList} />
                        <Route exact path={Const.NavigationPathPatterns} component={PatternsList} />
                        <Route exact path={Const.NavigationPathLearning} component={LearningList} />
                        <Route exact path={Const.NavigationPathModel} component={ModelList} />
                        <RUIC.Toasts key={6} stream={NotificationService.getStream()} />
                    </div>
                </div>
        );
    }
}

const mapStateToProps = (state: IPortalState, ownProps: any): IAppPageProps => {
    return {
    } as IAppPageProps;
};

export const Main: any = withRouter(connect(mapStateToProps)(MainApp));
export const reducer = reducers;

class MainComp extends React.Component<any, any> {
    constructor(props) {
        super(props);
    }
    render() {
        return (
            <Provider store={store}>
                <Router history={history}>
                    <Main />
                </Router>
            </Provider>
        );
    }
}


AuthService.init()
    .then(val => {
        val && ReactDOM.render(<MainComp />, document.getElementById('app'));
    })
    .catch();


