using FacturationApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Models
{
    public partial class Facture : Identifiable, IFacture, IFactureOutput, IFacturePdf, IFactureFull, IFactureDb
    {
        public Facture()
        {
            Services = new List<Service>();
        }

        [Key]
        public override int Id { get; set; }
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
        [NotMapped] public bool IsFinal { get; set; }
        [NotMapped] public bool IsPaye { get; set; }
        [NotMapped] public string NumeroFacture { get; set; }
        [NotMapped] public decimal MontantHT { get; set; }
        [NotMapped] public decimal MontantTva { get; set; }
        [NotMapped] public decimal MontantTTC { get; set; }
        [NotMapped] public string MyLastName { get; set; }
        [NotMapped] public string MyFirstName { get; set; }
        [NotMapped] public string MyAddress { get; set; }
        [NotMapped] public string MyPostCode { get; set; }
        [NotMapped] public string MyCity { get; set; }
        [NotMapped] public string MyPhone { get; set; }
        [NotMapped] public string MyEmail { get; set; }
        [NotMapped] public string MyNumeroTva { get; set; }
        [NotMapped] public string MySiret { get; set; }
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
