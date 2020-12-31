using Microsoft.Extensions.Logging;

namespace Web.Tools
{
    public class Logger<T> : FacturationApi.Spi.ILogger
    {
        private readonly ILogger<T> _logger;

        public Logger(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void Warning(string message)
        {
            _logger.LogWarning(message);
        }

        public void Info(string message)
        {
            _logger.LogInformation(message);
        }
    }
}
