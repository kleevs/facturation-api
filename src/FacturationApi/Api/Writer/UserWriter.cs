using FacturationApi.Models;
using FacturationApi.Spi;
using FacturationApi.Tools;
using System.Linq;

namespace FacturationApi.Api
{
    public class UserWriter
    {
        private readonly IUserProvider _provider;
        private readonly ISmtpServer _smtpServer;
        private readonly IEncryptor _encryptor;
        private readonly IDateTimeService _dateTimeService;
        private readonly IHttpRequest _hostingEnvironment;
        private readonly ILogger _logger;

        public UserWriter(
            IUserProvider provider,
            ISmtpServer smtpServer,
            IEncryptor encryptor,
            IDateTimeService dateTimeService,
            IHttpRequest hostingEnvironment,
            ILogger logger
        )
        {
            _provider = provider;
            _smtpServer = smtpServer;
            _encryptor = encryptor;
            _dateTimeService = dateTimeService;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        public bool CreateRequest(string email, string password)
        {
            Error.ThrowIf<IsEmailEmptyError>(string.IsNullOrWhiteSpace(email));
            Error.ThrowIf<IsPasswordInvalidError>(string.IsNullOrWhiteSpace(password));

            email = email.ToLower();

            Error.ThrowIf<UserExitedError>(_provider.User.Where(_ => _.Email == email).Count() > 0);


            var now = _dateTimeService.UtcNow;
            var token = new Token { Email = email, Password = password, Created = now };
            var hash = _encryptor.Encrypt(token);
            var link = $@"{_hostingEnvironment.BaseUrl}/accounts/{hash}";
            _smtpServer.Send(email, "Création de compte", $"Cliquez sur ce lien pour confirmer votre inscription : <a href=\"{link}\">{link}</a>");

            _logger.Info($"Demande de création d'utilisateur envoyé à {email}.");
            return true;
        }

        public bool Create(string token)
        {
            var now = _dateTimeService.UtcNow;
            var data = _encryptor.Decrypt<Token>(token);
            if (data == null)
            {
                _logger.Warning($"token '{token}' null.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(data.Email))
            {
                _logger.Warning($"email non renseigné.");
                return false;
            }

            if ((now - data.Created).Hours > 24)
            {
                _logger.Warning($"token de création du compte {data.Email} expiré.");
                return false;
            }

            var isUserExist = _provider.Login.Where(_ => _.Email == data.Email).Count() > 0;
            if (isUserExist)
            {
                _logger.Warning($"Utilisateur {data.Email} déjà existant.");
                return false;
            }

            var user = _provider.NewUser();
            user.Email = data.Email.ToLower();
            user.Password = data.Password;

            _logger.Info($"Utilisateur {data.Email} créé.");
            return true;
        }

        public bool Save(int userId, IUser request)
        {
            var isUserExist = _provider.User
                .Where(_ => _.LastName == request.LastName)
                .Where(_ => _.FirstName == request.FirstName)
                .Where(_ => _.Street == request.Street)
                .Where(_ => _.Complement == request.Complement)
                .Where(_ => _.ZipCode == request.ZipCode)
                .Where(_ => _.Country == request.Country)
                .Where(_ => _.City == request.City)
                .Where(_ => _.Phone == request.Phone)
                .Where(_ => _.NumTva == request.NumTva)
                .Where(_ => _.Siret == request.Siret)
                .Where(_ => _.Email == request.Email)
                .Count() > 0;

            if (isUserExist) return false;

            Error.ThrowIf<SiretInvalidError>(request.Siret.Length > 14);

            var user = _provider.NewUserInfo();

            user.UserId = userId;
            user.LastName = request.LastName;
            user.FirstName = request.FirstName;
            user.Street = request.Street;
            user.Complement = request.Complement;
            user.ZipCode = request.ZipCode;
            user.Country = request.Country;
            user.City = request.City;
            user.Phone = request.Phone;
            user.NumTva = request.NumTva;
            user.Siret = request.Siret;
            user.Email = request.Email;

            return true;
        }
    }
}
