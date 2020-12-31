using FacturationApi.Spi;
using System.Linq;
using System.Threading.Tasks;

namespace FacturationApi.Api
{
    public class FacturePdfService
    {
        private readonly IFactureProvider _factureReader;
        private readonly IUserProvider _userReader;
        private readonly IPdfGenerator _pdfGenerator;

        public FacturePdfService(
            IFactureProvider factureReader,
            IUserProvider userReader,
            IPdfGenerator pdfGenerator
        )
        {
            _factureReader = factureReader;
            _userReader = userReader;
            _pdfGenerator = pdfGenerator;
        }

        public Task<byte[]> Generate(int id)
        {
            var facture = _factureReader.FacturePdf.Where(_ => _.Id == id).FirstOrDefault();
            var user = _userReader.User.Where(_ => _.Id == facture.UserDataId).FirstOrDefault();

            facture.DateEcheanceOption = facture.DateEcheance.HasValue && facture.DateCreation.HasValue && 
                (facture.DateEcheance.Value - facture.DateCreation.Value).Days >= 45 ? 1 : 0;

            return _pdfGenerator.Generate(facture);
        }
    }
}
