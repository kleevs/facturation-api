using System;

namespace FacturationApi.Spi
{
    public interface IDateTimeService
    {
        DateTime UtcNow { get; }
    }
}