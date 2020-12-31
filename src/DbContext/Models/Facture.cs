using FacturationApi.Models;
using FacturationApi.Rules;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Models
{
    public partial class Facture : IFacture, IFactureOutput, IFacturePdf, IFactureFull, IFactureDb
    {
        public Facture()
        {
            Services = new List<Service>();
        }

        [Key]
        public int Id { get; set; }
        public int UserDataId { get; set; }
        public int Numero { get; set; }
        public string RaisonSociale { get; set; }
        public DateTime? DateCreation { get; set; }
        public DateTime? DateEcheance { get; set; }
        public int? PaymentOption { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Street { get; set; }
        public string Complement { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public List<Service> Services { get; set; }
        public IEnumerable<Paiement> Paiements { get; set; }
        [ForeignKey("UserDataId")]
        public UtilisateurData UserData { get; set; }

        [NotMapped] public int? DateEcheanceOption { get; set; }
        [NotMapped] public bool IsFinal => FactureRules.IsFinal(this);
        [NotMapped] public bool IsPaye => FactureRules.IsPaye(this);
        [NotMapped] public string NumeroFacture => FactureRules.NumeroFacture(this);
        [NotMapped] public decimal MontantHT => FactureRules.MontantHT(this);
        [NotMapped] public decimal MontantTva => FactureRules.MontantTVA(this);
        [NotMapped] public decimal MontantTTC => FactureRules.MontantTTC(this);
        [NotMapped] public string MyLastName => UserData?.LastName;
        [NotMapped] public string MyFirstName => UserData?.FirstName;
        [NotMapped] public string MyAddress => UserData?.Street;
        [NotMapped] public string MyPostCode => UserData?.ZipCode;
        [NotMapped] public string MyCity => UserData?.City;
        [NotMapped] public string MyPhone => UserData?.Phone;
        [NotMapped] public string MyEmail => UserData?.Email;
        [NotMapped] public string MyNumeroTva => UserData?.NumTva;
        [NotMapped] public string MySiret => UserData?.Siret;
        [NotMapped] public IEnumerable<IFileDb> PieceJointes { get; set; }

        IService IFactureDb.NewService => new Service();

        public void AddService(IService service)
        {
            Services.Add(service as Service);
        }

        public void RemoveService(IService service)
        {
            Services.Remove(service as Service);
        }

        IEnumerable<IService> IFacture.Services => Services;
        IEnumerable<IService> IFactureDb.Services => Services;
        IEnumerable<IPaiement> IFacture.Paiements => Paiements;
        IEnumerable<IPaiement> IFactureDb.Paiements { get => Paiements; }
    }
}
