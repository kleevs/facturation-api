using System.Linq;

namespace FacturationApi.Spi
{
    public interface IGenericReaderRepository<TOutput>
    {
        IQueryable<TOutput> Read();
    }

    public interface IGenericReaderRepository<TOutput, TFilter>
    {
        IQueryable<TOutput> GetBy(TFilter filter);
    }
}