import { AjaxService } from './ajax';
import { AuthenticationData } from './authenticationData';

export class AuthService {
    constructor(private _ajaxService: AjaxService) {}
    
    isConnected() : Promise<AuthenticationData> {
        return this._ajaxService.ajax<any>('/accounts', { method: 'GET', blockUI: true })
          .then(_ => {
                return new AuthenticationData(_.result); 
            });
    }
}