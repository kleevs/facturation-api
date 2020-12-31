import { success } from '../../tool/notify';
import { AjaxService } from '../service/ajax';
import { IRouter } from '../spi/router';

export class Login {

  email: string;
  password: string;
  
  constructor(private _ajaxService: AjaxService, private _router: IRouter) {
  }

  signin() {
    this._ajaxService.ajax('/accounts/login', { 
      method: 'POST',
      blockUI: true,
      data: {
        login: this.email,
        password: this.password
      }
    })
    .then(_ => success("Connexion réussi"))
    .then(_ => this._router.goTo('/'));
  }
}