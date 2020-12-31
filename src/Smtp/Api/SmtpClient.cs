using FacturationApi.Spi;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace Smtp
{
    public class SmtpClient : ISmtpServer
    {
        private readonly string _host;
        private readonly string _user;
        private readonly string _password;
        private readonly int _port;
        private readonly NetworkCredential _credential;

        public SmtpClient(string connectionStrings)
        {
            var config = connectionStrings.Split(';')
                .Where(_ => !string.IsNullOrEmpty(_))
                .Select(keyValue =>
                {
                    var tmp = keyValue.Split('=');
                    return new KeyValuePair<string, string>(tmp[0], tmp[1]);
                });

            _host = config.FirstOrDefault(_ => _.Key == "server").Value;
            _port = int.Parse(config.FirstOrDefault(_ => _.Key == "port").Value);
            _user = config.FirstOrDefault(_ => _.Key == "user").Value;
            _password = config.FirstOrDefault(_ => _.Key == "password").Value;
            _credential = new NetworkCredential(_user, _password);
        }

        public void Send(string destinataire, string subject, string content)
        {
            var client = new System.Net.Mail.SmtpClient(_host, _port)
            {
                Credentials = _credential,
                EnableSsl = true
            };
            client.Send(_user, destinataire, subject, content);
        }
    }
}
