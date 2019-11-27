using FacturationApi.Models;

namespace FacturationApi.Spi
{
    public interface IPdfGenerator
    {
        byte[] Generate(IFacturePdf facturePdf);
    }
}