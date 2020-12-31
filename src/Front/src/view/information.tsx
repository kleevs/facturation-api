import * as React from 'react';
import Layout from './layout';
import { Information as App } from '../app/page/information';
import { useObserver } from '../tool/react.extend';
import { preventDefault } from '../tool/mixin';

export default function (props: { app: App }) {
    var info = useObserver(props.app);

    return <Layout app={info}>
        <form className="container-fluid" onSubmit={(e) => preventDefault(e, () => info.save())}>
            <div className="row">
                <div className="col-md-6 form-group">
                    <label htmlFor="lastname">Nom</label>
                    <input className="form-control" value={info.lastName} onChange={(e) => info.lastName = e.target.value} id="lastname" />
                </div>

                <div className="col-md-6 form-group">
                    <label htmlFor="firstname">Prénom</label>
                    <input className="form-control" value={info.firstName} onChange={(e) => info.firstName = e.target.value} id="firstname" />
                </div>

                <div className="col-md-6 form-group">
                    <label htmlFor="street">Adresse</label>
                    <input className="form-control" value={info.street} onChange={(e) => info.street = e.target.value} id="street" />
                </div>

                <div className="col-md-6 form-group">
                    <label htmlFor="complement">Complément</label>
                    <input className="form-control" value={info.complement} onChange={(e) => info.complement = e.target.value} id="complement" />
                </div>

                <div className="col-md-2 form-group">
                    <label htmlFor="zipCode">Code postal</label>
                    <input className="form-control" value={info.zipCode} onChange={(e) => info.zipCode = e.target.value} id="zipCode" />
                </div>

                <div className="col-md-4 form-group">
                    <label htmlFor="city">Ville</label>
                    <input className="form-control" value={info.city} onChange={(e) => info.city = e.target.value} id="city" />
                </div>

                <div className="col-md-6 form-group">
                    <label htmlFor="country">Pays</label>
                    <input className="form-control" value={info.country} onChange={(e) => info.country = e.target.value} id="country" />
                </div>

                <div className="col-md-6 form-group">
                    <label htmlFor="phone">Tel</label>
                    <input className="form-control" value={info.phone} onChange={(e) => info.phone = e.target.value} id="phone" />
                </div>

                <div className="col-md-6 form-group">
                    <label htmlFor="email">Email</label>
                    <input className="form-control" value={info.email} onChange={(e) => info.email = e.target.value} id="email" />
                </div>

                <div className="col-md-6 form-group">
                    <label htmlFor="siret">Siret</label>
                    <input className="form-control" value={info.siret} onChange={(e) => info.siret = e.target.value} id="siret" />
                </div>

                <div className="col-md-6 form-group">
                    <label htmlFor="numTva">Num tva</label>
                    <input className="form-control" value={info.numTva} onChange={(e) => info.numTva = e.target.value} id="numTva" />
                </div>

                <button type="submit" className={`btn btn-primary full-width attention-hover`} data-content="Enregistrer">Enregistrer</button>
            </div>
        </form>
    </Layout>
}
