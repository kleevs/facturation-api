using System.Threading.Tasks;
using FacturationApi.Api;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class PaiementController : ControllerBase
    {
        private readonly Db.Provider _provider;

        public PaiementController(Db.Provider provider)
        {
            _provider = provider;
        }

        [HttpPut]
        [Route("paiement")]
        public async Task Create([FromBody]Models.Input.PaiementModel paiement) => 
            await _provider.SaveChangesAsync(new PaiementWriterService(_provider).Save(paiement));

        [HttpDelete]
        [Route("paiement/{id}")]
        public async Task Delete(int id)
        {
            new PaiementWriterService(_provider).Delete(id);
            await _provider.SaveChangesAsync();
        }
    }
}
