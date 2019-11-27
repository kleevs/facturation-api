using FacturationApi.Models;
using System.Linq;

namespace FacturationApi.Api
{
    static class FactureRule
    {
        private static T RuleFacture<T>(T facture) where T : IFacture
        {
            if (facture != null)
            {
                if (facture.DateEcheance.HasValue && facture.DateCreation.HasValue)
                {
                    var diff = facture.DateEcheance.Value - facture.DateCreation.Value;
                    if (diff.Days >= 45)
                    {
                        facture.DateEcheanceOption = 1;
                    }
                }
            }

            return facture;
        }

        public static T RuleFactureOutput<T>(T facture) where T : IFactureOutput
        {
            if (facture != null)
            {
                facture = RuleFacture(facture);

                if (facture.DateCreation.HasValue)
                {
                    facture.NumeroFacture = $"{facture.DateCreation.Value.Year}{(facture.DateCreation.Value.Month + 1).ToString("00")}{facture.Numero.ToString("00000")}";
                }

                var montantTtc = facture.Services.Sum(_ => (_.Price * _.Quantity) * (100 + _.Tva) / 100);

                facture.IsFinal = facture.Paiements != null && facture.Paiements.Count() > 0;
                facture.IsPaye = facture.Paiements != null && facture.Paiements.Sum(_ => _.Value) >= montantTtc;
            }

            return facture;
        }
    }
}
