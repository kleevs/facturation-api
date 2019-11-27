using FacturationApi.Models;
using System;

namespace Web.Models.Output
{
    public class ServiceModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Tva { get; set; }
        public string Unite { get; set; }

        public static Func<IService, ServiceModel> Map = (service) => new ServiceModel
        {
            Id = service.Id.Value,
            Description = service.Description,
            Price = service.Price,
            Quantity = service.Quantity,
            Tva = service.Tva,
            Unite = service.Unite
        };
    }
}
