namespace FacturationApi.Spi
{
    public interface ILogger
    {
        void Warning(string message);
        void Info(string message);
    }
}