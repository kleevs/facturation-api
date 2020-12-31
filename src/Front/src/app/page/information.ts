import { AuthenticationData } from '../service/authenticationData';
import { AjaxService } from '../service/ajax';
import { success } from '../../tool/notify';
import { AuthenticationModel } from '../model/authentication';
import { Layout } from './layout';

export class Information  extends Layout {
  city: string;
  complement: string;
  country: string;
  firstName: string;
  lastName: string;
  street: string;
  userId: string;
  zipCode: string;
  email: string;
  numTva: string;
  siret: string;
  phone: string;

  constructor(_ajaxService: AjaxService, private _auth: AuthenticationData) {
    super(_ajaxService, _auth);
    var userInfo = _auth.current && _auth.current.data && _auth.current.data[0] || <AuthenticationModel>{};
    this.city = userInfo.city;
    this.complement = userInfo.complement;
    this.country = userInfo.country;
    this.firstName = userInfo.firstName;
    this.lastName = userInfo.lastName;
    this.street = userInfo.street;
    this.userId = userInfo.userId;
    this.zipCode = userInfo.zipCode;
    this.email = userInfo.email;
    this.numTva = userInfo.numTva;
    this.siret = userInfo.siret;
    this.phone = userInfo.phone;
  }

  save() {
    this._ajaxService.ajax(`/accounts/${this._auth.current.id}`, { 
      method: 'PUT', 
      blockUI: true, 
      data: {
        city: this.city,
        complement: this.complement,
        country: this.country,
        firstName: this.firstName,
        lastName: this.lastName,
        street: this.street,
        userId: this.userId,
        zipCode: this.zipCode,
        email: this.email,
        numTva: this.numTva,
        siret: this.siret,
        phone: this.phone
      }
    })
    .then(_ => success("Success", "Sauvegarde effectué"));
  }
}