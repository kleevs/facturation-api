using FacturationApi.Models;
using System;

namespace Web.Models.Output
{
    public class PaiementModel
    {
        public int Id { get; set; }
        public DateTime? DateCreation { get; set; }
        public decimal? Value { get; set; }

        public static Func<IPaiement, PaiementModel> Map = (paiement) => new PaiementModel
        {
            Id = paiement.Id,
            DateCreation = paiement.DateCreation,
            Value = paiement.Value
        };
    }
}
