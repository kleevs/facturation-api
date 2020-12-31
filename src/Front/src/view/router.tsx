import * as React from 'react';
import * as $ from 'jquery';
import Detail from './detail/index';
import List from './list';
import Login from './login';
import Information from './information';
import { Detail as Facture } from '../app/page/detail';
import { List as Factures} from '../app/page/list';
import { Login as LoginApp} from '../app/page/login';
import { Information as InformationApp } from '../app/page/information';
import { UI } from '../app/service/ui';
import { AjaxService } from '../app/service/ajax';
import { AuthService } from '../app/service/auth.service';
import { AuthenticationData } from '../app/service/authenticationData';
import { useObserver } from '../tool/react.extend';
import { RouterService } from '../tool/router';

var $waitingScreen = $(`<div id='waiting-screen'></div>`);
var uiController = new UI({
    blockUI: () => { $("body").append($waitingScreen); },
    unblockUI: () => { $("<div>").append($waitingScreen); }
});
var ajaxService = new AjaxService(uiController);
var authService = new AuthService(ajaxService);
var routerService = new RouterService();
routerService.listen((location) => {
    if (!_observable.userData) {
        authService.isConnected().then(_ => _observable.userData = _);
        _observable.path = location.pathname;
    } else {
        _observable.path = location.pathname;
    }
});

const _observable: { path: string; userData: AuthenticationData }  = { path: location.pathname, userData: undefined };

authService.isConnected().then(_ => _observable.userData = _).catch(() => routerService.goTo('/login'));

export default function App() {
    var observable = useObserver(_observable);
    var userData = _observable.userData;
    return <div className="">
        {(() => {
            if (userData || _observable.path === "/login") {
                switch (_observable.path) {
                    case '/login': return <Login app={new LoginApp(ajaxService, routerService)} />;
                    case '/facturations':
                    case '':
                    case '/' : return <List app={new Factures(ajaxService, userData, routerService)} />;
                    case '/facturations/new': return <Detail app={new Facture(ajaxService, routerService, userData, undefined)} />;
                    case '/account': return <Information app={new InformationApp(ajaxService, userData)} />;
                    default: {
                        let matched;
                        if (matched = (/\/facturations\/(\d+)/g).exec(_observable.path)) {
                            return <Detail app={new Facture(ajaxService, routerService, userData, +matched[1])} />;
                        }
                    }
                }
            }
        })()}
    </div>
}

