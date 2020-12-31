using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Db;
using FacturationApi.Api;
using FileSystem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Pdf;
using Web.Tools;

namespace Web.Controllers
{
    public class FacturationController : ControllerBase
    {
        private readonly Db.IProvider _provider;
        private readonly AppConfiguration _configuration;
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;

        public FacturationController(
            IProvider provider, 
            AppConfiguration configuration, 
            IRazorViewEngine razorViewEngine, 
            ITempDataProvider tempDataProvider, 
            IServiceProvider serviceProvider
        )
        {
            _provider = provider;
            _configuration = configuration;
            _razorViewEngine = razorViewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }

        [HttpGet]
        [Route("facturation")]
        public async Task<IEnumerable<Models.Output.FactureModel>> Index() => await Task.Run(() =>
            new FactureService(_provider).List().Select(Models.Output.FactureModel.Map).ToList());

        [HttpGet]
        [Route("facturation/{id}")]
        public async Task<Models.Output.FactureModel> Index(int id) =>
            await Task.Run(() => new FactureDetailService(_provider, new FileManager(_configuration.ConnectionStringFtp))
            .GetById(id).Select(Models.Output.FactureModel.MapFull).FirstOrDefault());

        [HttpGet]
        [Route("facturation/{id}/pdf")]
        public async Task<IActionResult> Pdf(int id) =>
            new FileContentResult(await new FacturePdfService(_provider, _provider, new Pdf.PdfGenerator(new RazorRenderer(_razorViewEngine, _tempDataProvider, _serviceProvider)))
                .Generate(id), "application/pdf");

        [HttpPut]
        [Route("facturation")]
        public async Task<int> Create([FromBody]Models.Input.FactureModel facture) =>
            (await _provider.SaveChangesAsync(new FactureWriterService(_provider).Save(facture))).Id;

        [HttpPost]
        [Route("facturation/{id}")]
        public async Task Update([FromBody]Models.Input.FactureModel facture) =>
            await _provider.SaveChangesAsync(new FactureWriterService(_provider).Save(facture));

        [HttpDelete]
        [Route("facturation/{id}")]
        public async Task Delete(int id) =>
            await _provider.SaveChangesAsync(new FactureWriterService(_provider).Delete(id));
    }
}
