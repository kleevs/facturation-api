using FacturationApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Web.Models.Output
{
    public class FactureModel
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public int UserDataId { get; set; }
        public string RaisonSociale { get; set; }
        public DateTime? DateCreation { get; set; }
        public DateTime? DateEcheance { get; set; }
        public int? DateEcheanceOption { get; set; }
        public int? PaymentOption { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Street { get; set; }
        public string Complement { get; set; }
        public string Cp { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public bool IsFinal { get; set; }
        public bool IsPaye { get; set; }
        public string NumeroFacture { get; set; }
        public IEnumerable<ServiceModel> Services { get; set; }
        public IEnumerable<PaiementModel> Paiements { get; set; }
        public IEnumerable<string> PieceJointes { get; set; }

        public static Func<IFactureOutput, FactureModel> Map = (facture) => new FactureModel
        {
            Id = facture.Id,
            Numero = facture.Numero,
            NumeroFacture = facture.NumeroFacture,
            UserDataId = facture.UserDataId,
            RaisonSociale = facture.RaisonSociale,
            DateCreation = facture.DateCreation,
            DateEcheance = facture.DateEcheance,
            DateEcheanceOption = facture.DateEcheanceOption,
            PaymentOption = facture.PaymentOption,
            LastName = facture.LastName,
            FirstName = facture.FirstName,
            Street = facture.Street,
            Complement = facture.Complement,
            Cp = facture.ZipCode,
            Country = facture.Country,
            City = facture.City,
            IsFinal = facture.IsFinal,
            IsPaye = facture.IsPaye,
            Services = facture.Services.Select(ServiceModel.Map),
            Paiements = facture.Paiements.Select(PaiementModel.Map)
        };

        public static Func<IFactureFull, FactureModel> MapFull = (facture) => new FactureModel
        {
            Id = facture.Id,
            Numero = facture.Numero,
            NumeroFacture = facture.NumeroFacture,
            UserDataId = facture.UserDataId,
            RaisonSociale = facture.RaisonSociale,
            DateCreation = facture.DateCreation,
            DateEcheance = facture.DateEcheance,
            DateEcheanceOption = facture.DateEcheanceOption,
            PaymentOption = facture.PaymentOption,
            LastName = facture.LastName,
            FirstName = facture.FirstName,
            Street = facture.Street,
            Complement = facture.Complement,
            Cp = facture.ZipCode,
            Country = facture.Country,
            City = facture.City,
            IsFinal = facture.IsFinal,
            IsPaye = facture.IsPaye,
            Services = facture.Services.Select(ServiceModel.Map),
            Paiements = facture.Paiements.Select(PaiementModel.Map),
            PieceJointes = facture.PieceJointes?.Select(_ => _.FileName)
        };
    }
}
