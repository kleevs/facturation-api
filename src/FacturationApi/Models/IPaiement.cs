using System;

namespace FacturationApi.Models
{
    public interface IPaiement : IIdentifiable
    {
        int? FactureId { get; set; }
        DateTime? DateCreation { get; }
        decimal? Value { get; }
    }

    public interface IPaiementDb : IIdentifiable
    {
        int? FactureId { get; set; }
        DateTime? DateCreation { get; set; }
        decimal? Value { get; set; }
    }
}
