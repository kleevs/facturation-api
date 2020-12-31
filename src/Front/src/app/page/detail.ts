import { IFacture } from '../model/facture';
import { AuthenticationData } from '../service/authenticationData';
import { IService } from '../model/service';
import { IPaiement } from '../model/paiement';
import { parseDate } from '../../tool/date';
import { AjaxService } from '../service/ajax';
import { success, error } from '../../tool/notify';
import { Layout } from './layout';
import { IRouter } from '../spi/router';

export class Detail extends Layout {
    id: number = undefined;
    numero: number;
    numeroFacture: string;
    userDataId: number = 0;
    currentUserDataId: number = 0;
    raisonSociale = '';
    lastName = '';
    firstName = '';
    street = '';
    complement = '';
    cp = '';
    city = '';
    country = '';
    isReadOnly = false;
    services: IService[] = [];
    dateCreation = new Date();
    dateEcheanceOption = 1;
    paymentOption = 1;
    dateEcheances = [{ id: 1, label: '45 jours + fin du mois' }];
    unites = [{ id: 1, label: 'jour' }];
    payments = [{ id: 1, label: 'Virement' }];

    // Paiement onglet
    paiements: IPaiement[] = [];
    nouveauMontant: number = 0;
    isPaye: boolean = false; 
    private _nouveauDateCreation: Date = undefined;
    private _nouveauDateCreationStr: string = undefined;


    // Piece jointe
    files: any[] = [];
    uploadedFile: string[] = [];

    get nouveauDateCreation() {
        return this._nouveauDateCreation && this._nouveauDateCreation.toLocaleDateString() || this._nouveauDateCreationStr;
    }

    set nouveauDateCreation(v: string) {
        var date = parseDate(this._nouveauDateCreationStr = v);
        this._nouveauDateCreation = date instanceof Date ? date : null;
    }

    constructor(
    _ajaxService: AjaxService, 
    private _router: IRouter, 
    public auth: AuthenticationData, 
    id: number
    ) {
        super(_ajaxService, auth);
        this.id = id;
        var auth = this.auth;
        this.currentUserDataId = auth && auth.current && auth.current.data[0] && auth.current.data[0].id;
        this.userDataId = this.userDataId || this.currentUserDataId;
        this.load();
    }

    delete() {
    this._ajaxService.ajax(`/facturation/${this.id}`, { method: 'DELETE',  blockUI: true })
        .then(_ => success("Success", "Suppression effectué"))  
        .then(_ => this._router.goTo('/facturations'))
        .catch(_ => _.result.message && error("Erreur", _.result.message));
    return false;
    }

    save() {
    var facture = {
        id: this.id,
        userDataId: this.userDataId, 
        contact: {
        raisonSociale: this.raisonSociale,
        lastName: this.lastName,
        firstName: this.firstName,
        address: {
            street: this.street,
            complement: this.complement,
            cp: this.cp,
            city: this.city,
            country: this.country
        }
        },
        services: this.services.map(_ => ({
        id: _.id,
        description: _.description,
        price: +_.price,
        unite: _.unite || "jour",
        quantity: +_.quantity,
        tva: +_.tva
        })),
        dateEcheanceOption: this.dateEcheanceOption,
        dateCreation: this.dateCreation,
        paymentOption: this.paymentOption
    };

    this._ajaxService.ajax<number>(`/facturation/${facture.id || ''}`, { blockUI: true, method : facture.id && 'POST' || 'PUT', data: facture})
    .then(_ => (success("Success", "Sauvegarde effectué"), _))
    .then(_ => { 
        if (facture.id) {
        this.load(); 
        } else {
        this._router.goTo(`/facturations/${_.result}`)
        }
      
    });
    return false;
    }

    addService() {
    this.services = this.services.concat([{
        id: 0,
        description: '',
        price: null,
        unite: '',
        quantity: null,
        tva: null
    }]);
    }

    removeService(service) {
    this.services = this.services.filter(_ => _ !== service);
    }

    addPaiement() {
    this._ajaxService.ajax(`/paiement`, { method : 'PUT', data: {
        dateCreation: this._nouveauDateCreation, 
        montant: +this.nouveauMontant,
        factureId: this.id
    }}).then(() => {
        success("Success", "Sauvegarde effectué"); 
        this.load();
    });
    }

    removePaiement(paiement: IPaiement) {
    this._ajaxService.ajax(`/paiement/${paiement.id}`, { method : 'DELETE' }).then(() => {
        success("Success", "Sauvegarde effectué"); 
        this.load();
    });
    }

    load() {
    if (this.id) { 
        this._ajaxService.ajax<IFacture>(`/facturation/${this.id}`, { method: 'GET', blockUI: true }).then((response) => {
            var facture = response.result;
          
            if (facture) {
            this.id = facture.id;
            this.numero = facture.numero;
            this.numeroFacture = facture.numeroFacture;
            this.userDataId = facture.userDataId;
            this.raisonSociale = facture.raisonSociale;
            this.lastName = facture.lastName;
            this.firstName = facture.firstName;
            this.street = facture.street;
            this.complement = facture.complement;
            this.cp = facture.cp;
            this.city = facture.city;
            this.country = facture.country;
            this.services = facture.services.map(_ => ({
                id: _.id,
                description: _.description,
                price: _.price,
                unite: _.unite,
                quantity: _.quantity,
                tva: _.tva
            })) || [],
            this.dateEcheanceOption = facture.dateEcheanceOption;
            this.dateCreation = facture.dateCreation;
            this.paymentOption = facture.paymentOption;
            this.isReadOnly = facture.isFinal;
            this.paiements = facture.paiements;
            this.isPaye = facture.isPaye;
            this.uploadedFile = facture.pieceJointes;
            }
        });
    } else {
        this.userDataId = this.currentUserDataId;
    }
    }

    addPieceJointe() {
    var data: any = { 
        id: this.id,
        documents: this.files
        };

        this._ajaxService.ajaxFormData(`/piecejointe`, { blockUI: true, method : 'PUT', data: data}).then(() => {
        this.clear();
        success("Success", "Pièce jointe ajoutée"); 
        this.load();
    });
    }

    removePieceJointe(filename: string) {
    this._ajaxService.ajaxFormData(`/facturation/${this.id}/piecejointe/${filename}`, { blockUI: true, method : 'DELETE'})
        .then(() => {
        this.clear();
        success("Success", "Pièce jointe supprimée"); 
        this.load();
        });
    }

    clear() {
    this.files = [];
    }
}