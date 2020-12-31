using FacturationApi.Models;
using System.Collections.Generic;

namespace FacturationApi.Spi
{
    public interface IXlsxGenerator
    {
        byte[] Generate(IEnumerable<IRecette> recette);
    }
}