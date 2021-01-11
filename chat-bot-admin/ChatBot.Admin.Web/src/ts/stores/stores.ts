import * as Redux from 'redux';
import { createBrowserHistory } from 'history';
import { routerMiddleware, routerReducer } from 'react-router-redux';
import { reducers } from '../reducers/reducers';
import { IPortalPageProps } from "../interfaces/IPortalPage";
import { IAppState } from "../interfaces/IPortalState";

export const history = createBrowserHistory({ basename: '' })
const middleware = routerMiddleware(history)

export const store = Redux.createStore<IAppState, any, any, any>(
    reducers,
    Redux.applyMiddleware(middleware)
) as any;
