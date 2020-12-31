namespace FacturationApi.Spi
{
    public interface ISmtpServer
    {
        void Send(string destinataire, string objet, string content);
    }
}