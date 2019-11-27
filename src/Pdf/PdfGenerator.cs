using iText.Html2pdf;
using System.IO;
using RazorEngine;
using RazorEngine.Templating;
using FacturationApi.Models;
using FacturationApi.Spi;

namespace Pdf
{
    public class PdfGenerator : IPdfGenerator
    {
        public byte[] Generate(IFacturePdf facturePdf)
        {
            using (var stream = new MemoryStream())
            {
                var sr = new StreamReader(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Pdf.Template.Facture.cshtml"));
                var template = sr.ReadToEnd();
                var output = Engine.Razor.RunCompile(template, "templateKey", null, facturePdf);
                HtmlConverter.ConvertToPdf(output, stream);
                return stream.ToArray();
            }
        }
    }
}
