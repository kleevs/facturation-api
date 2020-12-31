using FacturationApi.Models;
using FacturationApi.Spi;
using System.Collections.Generic;
using FacturationApi.Linq2;

namespace FacturationApi.Api
{
    public class FactureService
    {
        private readonly IFactureProvider _factureReader;

        public FactureService(IFactureProvider factureReader)
        {
            _factureReader = factureReader;
        }

        public IEnumerable<IFactureOutput> List() => _factureReader.FactureOutput.Select(facture => 
        {
            facture.DateEcheanceOption = facture.DateEcheance.HasValue && facture.DateCreation.HasValue &&
                (facture.DateEcheance.Value - facture.DateCreation.Value).Days >= 45 ? 1 : 0;

            return facture;
        });
    }

    public class FactureDetailService
    {
        private readonly IFactureProvider _factureReader;
        private readonly IPieceJointeProvider _fileManager;

        public FactureDetailService(IFactureProvider factureReader, IPieceJointeProvider fileManager)
        {
            _factureReader = factureReader;
            _fileManager = fileManager;
        }

        public IEnumerable<IFactureFull> GetById(int id) =>
            _factureReader.FactureFull.Where(_ => _.Id == id)
            .Select(facture =>
            {
                facture.DateEcheanceOption = facture.DateEcheance.HasValue && facture.DateCreation.HasValue &&
                (facture.DateEcheance.Value - facture.DateCreation.Value).Days >= 45 ? 1 : 0;

                facture.PieceJointes = _fileManager.Files($"/pj/id{id}").ToList();
                return facture;
            });

        public IEnumerable<IFileDb> GetPieceJointes(int id, string filename) => _fileManager.Files($"/pj/id{id}/{filename}");
    }
}
