using iText.Html2pdf;
using System.IO;
using FacturationApi.Models;
using FacturationApi.Spi;
using System.Threading.Tasks;

namespace Pdf
{
    public class PdfGenerator : IPdfGenerator
    {
        private readonly IRazorRenderer _razorRenderer;

        public PdfGenerator(IRazorRenderer razorRenderer)
        {
            _razorRenderer = razorRenderer;
        }

        public async Task<byte[]> Generate(IFacturePdf facturePdf)
        {
            using (var stream = new MemoryStream())
            {
                var output = await _razorRenderer.RenderViewAsync("Template/Facture.cshtml", facturePdf);
                HtmlConverter.ConvertToPdf(output, stream);
                return stream.ToArray();
            }
        }
    }
}
