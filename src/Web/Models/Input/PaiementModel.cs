using FacturationApi.Models;
using System;

namespace Web.Models.Input
{
    public class PaiementModel : IPaiement
    {
        public int Id { get; set; }
        public int? FactureId { get; set; }
        public DateTime? DateCreation { get; set; }
        public decimal? Montant { get; set; }

        decimal? IPaiement.Value => Montant;
    }
}
