using FacturationApi.Models;
using System.Threading.Tasks;

namespace FacturationApi.Spi
{
    public interface IPdfGenerator
    {
        Task<byte[]> Generate(IFacturePdf facturePdf);
    }
}