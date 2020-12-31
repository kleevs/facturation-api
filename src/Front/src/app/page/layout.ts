import { AuthenticationData } from '../service/authenticationData';
import { AjaxService } from '../service/ajax';

export abstract class Layout {
    login: string;

    constructor(protected _ajaxService: AjaxService, currentUser: AuthenticationData) {
        this.login = currentUser.current.name;
    }

    logout() {
        this._ajaxService.ajax(`/accounts/logout`, { blockUI: true, method : 'GET' })
            .then(_ => location.href = '/');
    }
}