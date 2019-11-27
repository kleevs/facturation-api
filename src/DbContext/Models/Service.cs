using FacturationApi.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Repository.Models
{
    public partial class Service : IService
    {
        [Key]
        public int Id { get; set; }
        public int FactureId { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Tva { get; set; }
        public string Unite { get; set; }
        int? IIdentifiable.Id { get => Id == 0 ? null : (int?)Id; set => Id = value ?? 0; }
    }
}
