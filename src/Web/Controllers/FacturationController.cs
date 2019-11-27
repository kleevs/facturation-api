using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FacturationApi.Api;
using FileSystem;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class FacturationController : ControllerBase
    {
        private readonly Db.Provider _provider;
        private readonly AppConfiguration _configuration;

        public FacturationController(Db.Provider provider, AppConfiguration configuration)
        {
            _provider = provider;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IEnumerable<Models.Output.FactureModel>> Index() => await Task.Run(() =>
            new FactureService(_provider).List().Select(Models.Output.FactureModel.Map).ToList());

        [HttpGet]
        [Route("facturation/{id}")]
        public async Task<Models.Output.FactureModel> Index(int id) =>
            await Task.Run(() => new FactureDetailService(_provider, new FileManager(
                _configuration.FtpHost,
                _configuration.FtpPort,
                _configuration.FtpUser,
                _configuration.FtpPassword
            )).GetById(id).Select(Models.Output.FactureModel.MapFull).FirstOrDefault());

        [HttpGet]
        [Route("facturation/{id}/pdf")]
        public async Task<IActionResult> Pdf(int id) => await Task.Run(() =>
            new FileContentResult(new FacturePdfService(_provider, _provider, new Pdf.PdfGenerator()).Generate(id), "application/pdf"));

        [HttpPut]
        [Route("facturation")]
        public async Task Create([FromBody]Models.Input.FactureModel facture) =>
            await _provider.SaveChangesAsync(new FactureWriterService(_provider).Save(facture));

        [HttpPost]
        [Route("facturation/{id}")]
        public async Task Update([FromBody]Models.Input.FactureModel facture) =>
            await _provider.SaveChangesAsync(new FactureWriterService(_provider).Save(facture));
    }
}
