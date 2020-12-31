import { IFacture } from '../model/facture';
import { AuthenticationData } from '../service/authenticationData';
import { AjaxService } from '../service/ajax';
import { Layout } from './layout';
import { IRouter } from '../spi/router';

export class List extends Layout {

  factures: IFacture[] = [];

  constructor(_ajaxService: AjaxService, auth: AuthenticationData, private _router: IRouter) {
    super(_ajaxService, auth)
    _ajaxService.ajax<IFacture[]>(`/facturation`, { method: 'GET', blockUI: true })
    .then((response) => {
      this.factures = response.result;
    });
  }

  goTo(facture: IFacture) {
    this._router.goTo(`/facturations/${facture.id}`);
  }
}