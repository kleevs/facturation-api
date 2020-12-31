import { IService } from './service';
import { IPaiement } from './paiement';

export interface IFacture {
    id: number;
    numero: number;
    numeroFacture: string;
    userDataId: number;
    raisonSociale: string;
    dateCreation: Date;
    dateEcheance: Date;
    dateEcheanceOption: number;
    paymentOption: number;
    lastName: string;
    firstName: string;
    street: string;
    complement: string;
    cp: string;
    country: string;
    city: string;
    services: IService[];
    paiements: IPaiement[];
    isFinal: boolean;
    isPaye: boolean;
    pieceJointes: string[];
}