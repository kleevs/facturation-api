using FacturationApi.Models;
using FacturationApi.Spi;
using System.Collections.Generic;

namespace Excel
{
    public class XlsxGenerator : IXlsxGenerator
    {
        public byte[] Generate(IEnumerable<IRecette> recette)
        {
            var xlsxFile = new Xlsx
            {
                Sheets = new List<Sheet> { new Sheet() }
            };
            return new XlsxBuilder(xlsxFile).Build();
        }
    }
}
