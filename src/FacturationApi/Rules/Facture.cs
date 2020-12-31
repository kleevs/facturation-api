using FacturationApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FacturationApi.Rules
{
    public static class FactureRules
    {
        public static decimal MontantHT(IFacture facture) 
        {
            return facture.Services.Sum(_ => (_.Price ?? 0) * (_.Quantity ?? 0));
        }

        public static decimal MontantTVA(IFacture facture)
        {
            return facture.Services.Sum(_ => (_.Tva ?? 0) * MontantHT(facture) / 100);
        }

        public static decimal MontantTTC(IFacture facture)
        {
            return MontantHT(facture) + MontantTVA(facture);
        }

        public static string NumeroFacture(IFacture facture) 
        {
            return facture.DateCreation.HasValue ?
                $"{facture.DateCreation.Value.Year}{(facture.DateCreation.Value.Month).ToString("00")}{facture.Numero.ToString("00000")}" : null;
        } 
        
        public static bool IsFinal(IFacture facture) 
        {
            return facture.Paiements != null && facture.Paiements.Count() > 0;        
        } 
        
        public static bool IsPaye(IFacture facture) 
        {
            return facture.Paiements != null && facture.Paiements.Sum(_ => _.Value) >= MontantTTC(facture);
        }
    }
}
