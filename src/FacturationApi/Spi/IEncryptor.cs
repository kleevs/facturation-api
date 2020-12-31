namespace FacturationApi.Spi
{
    public interface IEncryptor
    {
        string Encrypt<T>(T obj);
        T Decrypt<T>(string cryptedstring);
    }
}