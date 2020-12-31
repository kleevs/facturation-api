import * as React from 'react';
import { Detail as App } from '../../app/page/detail';
import { preventDefault } from '../../tool/mixin';
import { AuthenticationModel } from '../../app/model/authentication';

export default function (props: { app: App }) {
    if (!props.app) return '';
    
    var facture = props.app;
    var information = facture.auth.current.data.filter(_ => _.id == facture.userDataId)[0] || {} as AuthenticationModel;

    return <form className="full-width center"  onSubmit={(e) => preventDefault(e, () => facture.save())}>
    <div className="row">
        <div className="col-md-6">
            <div className="card shadow mb-4">
                <div className="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                    <h6 className="m-0 font-weight-bold text-primary">
                        Information vendeur
                    </h6>
                </div>
                <div className="card-body">
                    <p className="mb-1">{information.lastName} {information.firstName} {information.street} {information.complement}</p>
                    <p className="mb-1">{information.zipCode} {information.city} {information.country}</p>
                    <p className="mb-1">{information.phone}</p>
                    <p className="mb-1">{information.email}</p>
                    <div className="mb-1">Numéro de tva : {information.numTva}</div>
                    <div className="mb-1">Numéro de siret : {information.siret}</div>
                </div>
            </div>
        </div>

        <div className="col-md-6">
            <div className="card shadow mb-4">
                <div className="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                    <h6 className="m-0 font-weight-bold text-primary">Information client</h6>
                </div>
                <div className="card-body">
                    <div className="input-group mb-3">
                        <input disabled={facture.isReadOnly} value={facture.raisonSociale} onChange={(e) => facture.raisonSociale = e.target.value} name="raisonSociale" type="text" className="form-control" placeholder="Raison sociale" />
                    </div>

                    <div className="row">
                        <div className="col-md-6">
                            <div className="input-group mb-3">
                                <input disabled={facture.isReadOnly} value={facture.lastName} onChange={(e) => facture.lastName = e.target.value} name="lastname" type="text" className="form-control" placeholder="Nom du contact" />
                            </div>
                        </div>

                        <div className="col-md-6">
                            <div className="input-group mb-3">
                                <input disabled={facture.isReadOnly} value={facture.firstName} onChange={(e) => facture.firstName = e.target.value} name="firstname" type="text" className="form-control" placeholder="Prénom du contact" />
                            </div>
                        </div>
                    </div>

                    <div className="input-group mb-3">
                        <input disabled={facture.isReadOnly} value={facture.street} onChange={(e) => facture.street = e.target.value} name="address" type="text" className="form-control" placeholder="Adresse" />
                    </div>

                    <div className="input-group mb-3">
                        <input disabled={facture.isReadOnly} value={facture.complement} onChange={(e) => facture.complement = e.target.value} name="complement" type="text" className="form-control" placeholder="Complément d'adresse" />
                    </div>

                    <div className="row">
                        <div className="col-md-4">
                            <div className="input-group mb-3">
                                <input disabled={facture.isReadOnly} value={facture.cp} onChange={(e) => facture.cp = e.target.value} name="cp" type="text" className="form-control" placeholder="CP" />
                            </div>
                        </div>

                        <div className="col-md-4">
                            <div className="input-group mb-3">
                                <input disabled={facture.isReadOnly} value={facture.city} onChange={(e) => facture.city = e.target.value} name="city" type="text" className="form-control" placeholder="Ville" />
                            </div>
                        </div>

                        <div className="col-md-4">
                            <div className="input-group mb-3">
                                <input disabled={facture.isReadOnly} value={facture.country} onChange={(e) => facture.country = e.target.value} name="country" type="text" className="form-control" placeholder="Pays" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div className="card shadow mb-4">
        <div className="card-body">
            <div className="row">
                <div className="col-md-4">
                    <div className="input-group mb-3">
                        <div className="input-group-prepend">
                            <span className="input-group-text">Date de création</span>
                            </div>
                            <input disabled={facture.isReadOnly} value={facture.dateCreation && facture.dateCreation.toLocaleDateString() || facture.dateCreation} onChange={(e) => facture.dateCreation = e.target.value} name="dateCreation" type="text" className="form-control" placeholder="Date de création" />
                    </div>
                </div>

                <div className="col-md-4">
                    <div className="input-group mb-3">
                        <div className="input-group-prepend">
                            <span className="input-group-text">Date d'échéance</span>
                        </div>
                        <select disabled={facture.isReadOnly} value={facture.dateEcheanceOption} onChange={(e) => facture.dateEcheanceOption = e.target.value} name="dateEcheance" type="text" className="form-control">
                            {facture.dateEcheances.map(opt => {
                                return <option key={opt.id} value={opt.id}>{opt.label}</option>
                            })}
                        </select>
                    </div>
                </div>

                <div className="col-md-4">
                    <div className="input-group mb-3">
                        <div className="input-group-prepend">
                            <span className="input-group-text">Mode de paiement</span>
                        </div>
                        <select disabled={facture.isReadOnly} value={facture.paymentOption} onChange={(e) => facture.paymentOption = e.target.value} name="payment" type="text" className="form-control">
                            {facture.payments.map(opt => {
                                return <option key={opt.id} value={opt.id}>{opt.label}</option>
                            })}
                        </select>
                    </div>
                </div>
            </div>
        </div>
    </div>

    {facture.services.map((service,i) => {
        return <div className="card shadow mb-4" key={`service_${i}`}>
            <div className="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                <h6 className="m-0 font-weight-bold text-primary">Service {i+1}</h6>
                <div className="dropdown no-arrow">
                    <span onClick={() => facture.removeService(service)} className={`${facture.isReadOnly && "hidden"} text-primary clickable`}>
                        <i className="fa fa-trash"></i>
                    </span>
                </div>
            </div>
            <div className="card-body">
                <div className="row">
                    <div className="col-md-6">
                        <div className="input-group mb-3">
                            <textarea disabled={facture.isReadOnly} value={service.description} onChange={(e) => service.description = e.target.value} name={`service[${i}].description`} className="form-control"></textarea>
                        </div>
                    </div>
                    <div className="col-md-6">
                        <div className="row">
                            <div className="col-md-6">
                                <div className="input-group mb-3">
                                    <input disabled={facture.isReadOnly} value={service.price} onChange={(e) => service.price = e.target.value} name={`service[${i}].price`} type="text" className="form-control" placeholder="Prix à l'unité" />
                                </div>
                            </div>
                            <div className="col-md-6">
                                <div className="input-group mb-3">
                                    <input disabled={facture.isReadOnly} value={service.tva} onChange={(e) => service.tva = e.target.value} name={`service[${i}].tva`} type="text" className="form-control" placeholder="TVA (%)" />
                                </div>
                            </div>
                        </div>
                        <div className="row">
                            <div className="col-md-6">
                                <div className="input-group mb-3">
                                    <input disabled={facture.isReadOnly} value={service.quantity} onChange={(e) => service.quantity = e.target.value} name={`service[${i}].quantity`} type="text" className="form-control" placeholder="Quantité" />
                                </div>
                            </div>
                            <div className="col-md-6">
                                <div className="input-group mb-3">
                                    <div className="input-group-prepend">
                                        <span className="input-group-text">Unité</span>
                                    </div>
                                    <select disabled={facture.isReadOnly} value={service.unite} onChange={(e) => service.unite = e.target.value} name={`service[${i}].unite`} type="text" className="form-control">
                                        {facture.unites.map(opt => {
                                            return <option key={opt.id} value={opt.label}>{opt.label}</option>
                                        })}
                                    </select>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    })}
    

    <hr/>
    <div>
        <button type="button" onClick={() => facture.addService()} className={`${facture.isReadOnly && "hidden"} btn btn-primary full-width attention-hover`}>Ajouter un service</button>
    </div>

    <hr/>
    <button type="submit" className={`${facture.isReadOnly && "hidden"} btn btn-primary full-width attention-hover`} data-content="Enregistrer la facture">Enregistrer</button>
</form>
}