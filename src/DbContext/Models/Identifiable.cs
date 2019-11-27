using FacturationApi.Models;

namespace Repository.Models
{
    public abstract class Identifiable : IIdentifiable
    {
        public abstract int Id { get; set; }
        int? IIdentifiable.Id { get => Id == 0 ? null : (int?)Id; set => Id = value ?? 0; }
    }
}
