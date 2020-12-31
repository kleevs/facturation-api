import * as React from 'react';
import { useState } from 'react';
import { useObserver } from '../../tool/react.extend';
import { Detail } from '../../app/page/detail';
import { preventDefault } from '../../tool/mixin';
import { apiDomain } from '../../tool/ajax';

export default function (props: { app: Detail }) {
    if (!props.app) return '';
    
    var facture = props.app;

    return <form className="full-width center"  onSubmit={(e) => preventDefault(e, () => facture.addPieceJointe())}>
        <div className="card shadow mb-4">
            <div className="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                {!facture.isPaye && <div className="input-group mb-3">
                    <input type="file" value={facture.files.map(_ => _.name).join(";")} onChange={(e) => facture.files = Array.from(e.target.files)} multiple/>
                </div> || ''}
                <div>
                    {facture.files.map((file, i) => {
                        return <div key={`pj_${i}`} className="row">
                            <div className="col-md-6">Nom du fichier</div>
                            <div className="col-md-6">
                                <div className="input-group mb-3">
                                    <input name="firstname" value={file.name} onChange={(e) => file.name = e.target.value} type="text" className="form-control" placeholder="Nom du fichier" />
                                </div>
                            </div>
                        </div>
                    })}
                </div>
            </div>
            <div className="card-body">
                {facture.uploadedFile && facture.uploadedFile.map((file, i) => {
                    return <div key={`pj_${i}`}>
                        <a target="_blank" href={`${apiDomain}/facturation/${facture.id}/piecejointe/${file}`}>{file}</a>
                        <span className="clickable text-primary" onClick={() => facture.removePieceJointe(file)}><i className="fa fa-trash"></i></span>
                    </div>
                })}
            </div>
        </div>

        <div className={`${!facture.files || facture.files.length <= 0 ? "hidden" : ""} row mb-3`}>
            <div className="col-md-6">
                <button className={`btn btn-primary full-width attention-hover`} type="submit">Upload</button>
            </div>
            <div className="col-md-6">
                <button className={`btn btn-secondary full-width attention-hover`} type="button" onClick={() => facture.clear()}>Annuler</button>
            </div>
        </div>
    </form>
}