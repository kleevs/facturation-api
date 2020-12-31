import * as React from 'react';
import Layout from './layout';
import { List as App } from '../app/page/list';
import { useObserver } from '../tool/react.extend';

export default function (props: { app: App }) {
    var app = useObserver(props.app);

    return <Layout app={app}>
        <div className="card shadow mb-4">
            <div className="card-header py-3">
                <h6 className="m-0 font-weight-bold text-primary">Factures enregistrées</h6>
            </div>
            <div className="card-body p-0">
                <table className="table table-striped table-bordered table-hover m-0">
                    <thead className="thead-dark">
                        <tr>
                            <th>Nom</th>
                            <th>Date de création</th>
                            <th>Date d'échéance</th>
                            <th>Montant ttc</th>
                            <th>Payé le</th>
                        </tr>
                    </thead>
                    <tbody>
                        {app.factures.map((facture, i) => {
                            return <tr key={i} className="clickable" onClick={() => app.goTo(facture)}>
                                <td>{facture.raisonSociale} - {facture.lastName} {facture.firstName}</td>
                                <td>{facture.dateCreation && facture.dateCreation.toLocaleDateString()}</td>
                                <td>{facture.dateEcheance && facture.dateEcheance.toLocaleDateString()}</td>
                                <td>{facture.services && facture.services.length > 0 && (
                                    facture.services.map(service => (service.tva + 100) * service.price * service.quantity / 100)
                                        .reduce((accumulator, currentValue) => accumulator + currentValue)
                                ) || 0}</td>
                                <td>{facture.paiements && facture.paiements.length > 0 && (
                                    new Date(Math.max.apply(null, facture.paiements.map(_ => _.dateCreation))).toLocaleDateString()
                                )}</td>
                            </tr>
                        })}
                    </tbody>
                </table>            
            </div>
        </div>
    </Layout>
}
