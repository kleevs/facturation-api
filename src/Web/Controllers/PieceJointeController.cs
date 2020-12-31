using System.Linq;
using System.Threading.Tasks;
using Db;
using FacturationApi.Api;
using FileSystem;
using Microsoft.AspNetCore.Mvc;
using Web.Tools;

namespace Web.Controllers
{
    public class PieceJointeController : ControllerBase
    {
        private readonly IProvider _provider;
        private readonly AppConfiguration _configuration;
        private readonly FileManager _fileManager;

        public PieceJointeController(Db.IProvider provider, AppConfiguration configuration)
        {
            _provider = provider;
            _configuration = configuration;
            _fileManager = new FileManager(_configuration.ConnectionStringFtp);
        }

        [HttpPut]
        [Route("piecejointe")]
        public async Task Create([FromForm]Models.Input.PieceJointeModel pieceJointe)
        {
            new PieceJointeWriterService(_provider, _fileManager)
                .Save(pieceJointe);
            await _fileManager.SaveChangesAsync();
        }

        [HttpDelete]
        [Route("facturation/{id}/piecejointe/{filename}")]
        public async Task Delete(int id, string filename)
        {
            new PieceJointeWriterService(_provider, _fileManager)
                .Delete(id, filename);
            await _fileManager.SaveChangesAsync();
        }

        [HttpGet]
        [Route("facturation/{id}/piecejointe/{filename}")]
        public async Task<IActionResult> Download(int id, string filename) => await Task.Run(() =>
            new FileContentResult(new FactureDetailService(_provider, _fileManager).GetPieceJointes(id, filename)
                .Select(_ => _.Content).FirstOrDefault(), new MimeType().GetMimeType(filename)));
    }
}
