using System;
using FacturationApi.Spi;

namespace Web.Tools
{
    public class DateTimeService : IDateTimeService
    {
        public DateTimeService()
        {
            UtcNow = DateTime.UtcNow;
        }

        public DateTime UtcNow { get; }
    }
}
