using FacturationApi.Models;
using FacturationApi.Spi;
using FacturationApi.Tools;
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
            var factureDb = _provider.FactureFull.Where(_ => _.Id == request.Id).FirstOrDefault();
            Error.ThrowIf<UnAuthorizedToSaveFactureError>((factureDb?.Paiements?.Count() ?? 0) > 0);

            foreach (var doc in request.Documents)
            {
                var file = _pieceJointeProvider.NewFile();
                file.FileName = $"/pj/id{request.Id}/{doc.FileName}";
                file.Content = doc.Content;
            }
        }

        public void Delete(int factureId, string filename)
        {
            var factureDb = _provider.FactureFull.Where(_ => _.Id == factureId).FirstOrDefault();
            Error.ThrowIf<UnAuthorizedToSaveFactureError>((factureDb?.Paiements?.Count() ?? 0) > 0);
            _pieceJointeProvider.RemoveFile($"/pj/id{factureId}/{filename}");
        }
    }
}
