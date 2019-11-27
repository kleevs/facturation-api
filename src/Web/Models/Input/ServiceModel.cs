using FacturationApi.Models;

namespace Web.Models.Input
{
    public class ServiceModel : IService
    {
        public int? Id { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Tva { get; set; }
        public string Unite { get; set; }
    }
}
