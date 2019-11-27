namespace FacturationApi.Models
{
    public interface IService : IIdentifiable
    {
        string Description { get; set; }
        decimal? Price { get; set; }
        decimal? Quantity { get; set; }
        decimal? Tva { get; set; }
        string Unite { get; set; }
    }
}
