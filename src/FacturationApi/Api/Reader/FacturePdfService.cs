using FacturationApi.Spi;
using System.Linq;

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

        public byte[] Generate(int id)
        {
            var facture = _factureReader.FacturePdf.Where(_ => _.Id == id).FirstOrDefault();
            var user = _userReader.User.Where(_ => _.Id == facture.UserDataId).FirstOrDefault();

            facture = FactureRule.RuleFactureOutput(facture);
            facture.MontantHT = facture.Services.Sum(_ => (_.Price ?? 0) * (_.Quantity ?? 0));
            facture.MontantTva = facture.Services.Sum(_ => (_.Tva ?? 0) * (_.Price ?? 0) * (_.Quantity ?? 0) / 100);
            facture.MontantTTC = facture.MontantHT + facture.MontantTva;
            facture.MyLastName = user.LastName;
            facture.MyFirstName = user.FirstName;
            facture.MyAddress = user.Street;
            facture.MyPostCode = user.ZipCode;
            facture.MyCity = user.City;
            facture.MyPhone = user.Phone;
            facture.MyEmail = user.Email;
            facture.MyNumeroTva = user.NumTva;
            facture.MySiret = user.Siret;

            return _pdfGenerator.Generate(facture);
        }
    }
}
