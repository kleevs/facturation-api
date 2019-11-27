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
        IEnumerable<IService> Services { get; }
        IEnumerable<IPaiement> Paiements { get; }
    }

    public interface IFactureOutput : IFacture
    {
        bool IsFinal { get; set; }
        bool IsPaye { get; set; }
        string NumeroFacture { get; set; }
        int Numero { get; }
    }

    public interface IFactureFull : IFactureOutput
    {
        IEnumerable<IFileDb> PieceJointes { get; set; }
    }

    public interface IFacturePdf : IFactureOutput
    {
        decimal MontantHT { get; set; }
        decimal MontantTva { get; set; }
        decimal MontantTTC { get; set; }
        string MyLastName { get; set; }
        string MyFirstName { get; set; }
        string MyAddress { get; set; }
        string MyPostCode { get; set; }
        string MyCity { get; set; }
        string MyPhone { get; set; }
        string MyEmail { get; set; }
        string MyNumeroTva { get; set; }
        string MySiret { get; set; }
    }
}
