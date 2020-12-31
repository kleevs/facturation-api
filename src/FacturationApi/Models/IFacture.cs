using System;
using System.Collections.Generic;

namespace FacturationApi.Models
{
    public interface IFactureDb : IIdentifiable
    {
        int UserDataId { get; set; }
        DateTime? DateCreation { get; set; }
        DateTime? DateEcheance { get; set; }
        int? DateEcheanceOption { get; set; }
        int? PaymentOption { get; set; }
        string RaisonSociale { get; set; }
        string LastName { get; set; }
        string FirstName { get; set; }
        string Street { get; set; }
        string Complement { get; set; }
        string ZipCode { get; set; }
        string Country { get; set; }
        string City { get; set; }
        int Numero { get; set; }
        IEnumerable<IService> Services { get; }
        IEnumerable<IPaiement> Paiements { get; }

        IService NewService { get; }
        void RemoveService(IService service);
        void AddService(IService service);
    }

    public interface IFacture : IIdentifiable
    {
        int UserDataId { get; }
        DateTime? DateCreation { get; }
        DateTime? DateEcheance { get; set; }
        int? DateEcheanceOption { get; set; }
        int? PaymentOption { get; }
        string RaisonSociale { get; }
        string LastName { get; }
        string FirstName { get; }
        string Street { get; }
        string Complement { get; }
        string ZipCode { get; }
        string Country { get; }
        string City { get; }
        int Numero { get; }
        IEnumerable<IService> Services { get; }
        IEnumerable<IPaiement> Paiements { get; }
    }

    public interface IFactureOutput : IFacture
    {
        bool IsFinal { get; }
        bool IsPaye { get; }
        string NumeroFacture { get; }
    }

    public interface IFactureFull : IFactureOutput
    {
        IEnumerable<IFileDb> PieceJointes { get; set; }
    }

    public interface IFacturePdf : IFactureOutput
    {
        decimal MontantHT { get; }
        decimal MontantTva { get; }
        decimal MontantTTC { get; }
        string MyLastName { get; }
        string MyFirstName { get; }
        string MyAddress { get; }
        string MyPostCode { get; }
        string MyCity { get; }
        string MyPhone { get; }
        string MyEmail { get; }
        string MyNumeroTva { get; }
        string MySiret { get; }
    }
}
