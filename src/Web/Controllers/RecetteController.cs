using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Excel;
using FacturationApi.Api;
using FileSystem;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class RecetteController : ControllerBase
    {
        private readonly Db.IProvider _provider;
        private readonly AppConfiguration _configuration;

        public RecetteController(Db.IProvider provider, AppConfiguration configuration)
        {
            _provider = provider;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("recette")]
        public async Task<IEnumerable<Models.Output.RecetteModel>> Index() => await Task.Run(() =>
            new RecetteService(_provider).List().Select(Models.Output.RecetteModel.Map).ToList());

        [HttpGet]
        [Route("recette.xlsx")]
        public async Task<IActionResult> Generate() => await Task.Run(() =>
            new FileContentResult(new RecetteXlsxService(new XlsxGenerator(), new RecetteService(_provider)).Generate(), "application/xlsx"));
    }
}
