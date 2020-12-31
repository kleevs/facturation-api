import * as React from 'react';
import { useObserver } from '../../tool/react.extend';
import { Detail } from '../../app/page/detail';

export default function (props: { app: Detail }) {
    if (!props.app) return '';
    
    var facture = props.app;
    
    return <div>
        <table className="table table-striped table-bordered table-hover">
            <thead className="thead-dark">
                <tr>
                    <th>Action</th>
                    <th>Montant</th>
                    <th>Date de paiement</th>
                </tr>
            </thead>
            <tbody>
                {facture.paiements.map((paiement, i) => {
                    return <tr key={`paiement[${i}]`}>
                        <td><button type="button" onClick={(e) => facture.removePaiement(paiement)}>Supprimer</button></td>
                        <td>{paiement.value}</td>
                        <td>{paiement.dateCreation.toLocaleDateString()}</td>
                    </tr>
                })}
                {(!facture.isPaye) && <tr>
                        <td><button type="button" onClick={(e) => facture.addPaiement()}>Creer</button></td>
                        <td><input value={facture.nouveauMontant} onChange={(e) => facture.nouveauMontant = e.target.value} type="text" className="form-control" placeholder="Montant" /></td>
                        <td><input value={facture.nouveauDateCreation} onChange={(e) => facture.nouveauDateCreation = e.target.value} type="text" className="form-control" placeholder="Date de paiement" /></td>
                    </tr>
                }
            </tbody>
        </table>
    </div>
}