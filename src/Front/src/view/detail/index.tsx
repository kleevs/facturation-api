import * as React from 'react';
import Layout from '../layout';
import DetailOnglet from './detail';
import { useObserver } from '../../tool/react.extend';
import { Detail as App } from '../../app/page/detail';
import PaiementOnglet from './paiement';
import PieceJointeOnglet from './piecejointe';
import { apiDomain } from '../../config';

export default function (props: { app: App }) {
    var facture = useObserver(props.app);
    var id = facture && facture.id;
    var numeroFacture = facture.numeroFacture;
    var services = facture && facture.services || [];
    var totalHT = services.length > 0 && services.map(service => service.price * service.quantity).reduce((accumulator, currentValue) => accumulator + currentValue) || 0;
    var totalTTC =  services.length > 0 && (totalHT + services.map(service => service.tva * service.price * service.quantity / 100).reduce((accumulator, currentValue) => accumulator + currentValue)) || 0;
    
    return <Layout app={facture}>
        <div className="container">
          <div className='row'>
              <div className="col-md-3">
                <div className="card border-left-primary shadow h-100 py-2">
                  <div className="card-body">
                    <div className="row no-gutters align-items-center">
                      {numeroFacture && <div className="col mr-2">
                        <div className="text-xs font-weight-bold text-primary text-uppercase mb-1">Facture n° {numeroFacture}</div>
                        <div className="row">
                          <div className="col-md-3">
                            <a target="_blank" href={`${apiDomain}/facturation/${id}/pdf`}>
                              <i className="fa fa-download"></i>
                            </a>
                          </div>

                          <div className="col-md-3">
                            <span onClick={() => facture.save()} className="text-primary clickable">
                              <i className="fa fa-save"></i>
                            </span>
                          </div>

                          <div className="col-md-3">
                            <span onClick={() => facture.delete()} className="text-primary clickable">
                              <i className="fa fa-trash"></i>
                            </span>
                          </div>
                        </div>
                      </div> || ''}
                    </div>
                  </div>
                </div>
              </div>
              <div className="col-md-3">
                <div className="card border-left-success shadow h-100 py-2">
                  <div className="card-body">
                    <div className="row no-gutters align-items-center">
                      <div className="col mr-2">
                        <div className="text-xs font-weight-bold text-success text-uppercase mb-1">Hors taxe</div>
                        <div className="h5 mb-0 font-weight-bold text-gray-800">{totalHT} €</div>
                      </div>
                      <div className="col-auto">
                        <i className="fas fa-dollar-sign fa-2x text-gray-300"></i>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
              <div className="col-md-3">
                <div className="card border-left-success shadow h-100 py-2">
                  <div className="card-body">
                    <div className="row no-gutters align-items-center">
                      <div className="col mr-2">
                        <div className="text-xs font-weight-bold text-success text-uppercase mb-1">Taxe tous comprise</div>
                        <div className="h5 mb-0 font-weight-bold text-gray-800">{totalTTC} €</div>
                      </div>
                      <div className="col-auto">
                        <i className="fas fa-dollar-sign fa-2x text-gray-300"></i>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
          </div>
        </div>
        <hr/>
        <div className="container">
            <nav className="nav nav-tabs">
                <a className="nav-item nav-link active" href="#detail" data-toggle="tab">Detail de la facture</a>
                {facture.id && <a className="nav-item nav-link" href="#piecejointe" data-toggle="tab">Pièces jointes</a> || ''}
                {facture.id && <a className="nav-item nav-link" href="#paiement" data-toggle="tab">Paiement</a> || ''}
            </nav>

            <div className="tab-content">
                <div className="tab-pane active" id="detail">
                   <DetailOnglet app={facture}/>
                </div>
                <div className="tab-pane" id="piecejointe">
                    <PieceJointeOnglet app={facture} />
                </div>
                <div className="tab-pane" id="paiement">
                    <PaiementOnglet app={facture} />
                </div>
            </div>
        </div>
    </Layout>
}