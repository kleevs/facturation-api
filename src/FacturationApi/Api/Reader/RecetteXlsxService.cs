using FacturationApi.Models;
using FacturationApi.Spi;
using System.Collections.Generic;
using System.Linq;

namespace FacturationApi.Api
{
    public class RecetteXlsxService
    {
        private readonly IXlsxGenerator _xlsxGenerator;
        private readonly RecetteService _recetteService;

        public RecetteXlsxService(IXlsxGenerator xlsxGenerator, RecetteService recetteService)
        {
            _xlsxGenerator = xlsxGenerator;
            _recetteService = recetteService;
        }

        public byte[] Generate() 
        {
            return _xlsxGenerator.Generate(_recetteService.List());
        }
    }
}
