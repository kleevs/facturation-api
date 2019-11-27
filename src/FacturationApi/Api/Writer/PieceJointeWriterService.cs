using FacturationApi.Models;
using FacturationApi.Spi;
using System.Linq;

namespace FacturationApi.Api
{
    public class PieceJointeWriterService
    {
        private readonly IFactureProvider _provider;
        private readonly IPieceJointeProvider _pieceJointeProvider;

        public PieceJointeWriterService(
            IFactureProvider provider,
            IPieceJointeProvider pieceJointeProvider
        )
        {
            _provider = provider;
            _pieceJointeProvider = pieceJointeProvider;
        }

        public void Save(IPieceJointe request)
        {
            var factureDb = _provider.FactureFull.Where(_ => _.Id == request.Id).Select(FactureRule.RuleFactureOutput).FirstOrDefault();

            if (!(factureDb?.IsPaye ?? false))
            {
                foreach (var doc in request.Documents)
                {
                    var file = _pieceJointeProvider.NewFile();
                    file.FileName = $"/pj/id{request.Id}/{doc.FileName}";
                    file.Content = doc.Content;
                }
            }
        }
    }
}
