using FacturationApi.Models;
using FacturationApi.Spi;
using System.Collections.Generic;
using System.Linq;

namespace FacturationApi.Api
{
    public class RecetteService
    {
        private readonly IFactureProvider _factureReader;

        public RecetteService(IFactureProvider factureReader)
        {
            _factureReader = factureReader;
        }

        public IEnumerable<IRecette> List() 
        {
            var factures = _factureReader.FactureFull.ToList();
            var recetteRows = factures.SelectMany(_ => _.Services.Select(s => new RecetteRow 
            {
                Date = _.Paiements.Select(p => p.DateCreation ?? System.DateTime.MinValue).Max(),
                NumeroFacture = _.NumeroFacture,
                Client = _.RaisonSociale ?? $"{_.LastName} {_.FirstName}" ,
                Nature = "Service",
                MontantHT = (s.Price ?? 0) * (s.Quantity ?? 0),
                Tva = s.Tva ?? 0,
                ModeEncaissement = "Virement",
            }));
            return recetteRows.Select(p => p.Date.Year).Distinct().Select(year => new Recette 
            {
                Year = year,
                Rows = recetteRows.Where(_ => _.Date.Year == year)
            });
        }
    }
}
