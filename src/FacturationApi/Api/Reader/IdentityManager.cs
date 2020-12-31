using FacturationApi.Models;
using FacturationApi.Spi;
using FacturationApi.Tools;
using System.Collections.Generic;
using System.Linq;

namespace FacturationApi.Api
{
    public class IdentityManager
    {
        private readonly IUserProvider _loginReader;
        private readonly IAuthenticationProvider _authenticationProvider;

        public IdentityManager(IUserProvider loginReader, IAuthenticationProvider authenticationProvider)
        {
            _loginReader = loginReader;
            _authenticationProvider = authenticationProvider;
        }

        public ILogin Login(string email, string password) => _loginReader.AuthenticateLogin
            .Where(_ => _.Email == email.ToLower())
            .Where(_ => _.Password == password)
            .FirstOrDefault() ?? throw new LoginFailedError();

        public IEnumerable<IUser> GetUserInfo() => _loginReader.IUserFilterable
            .Where(_ => _.UserId == _authenticationProvider.Current.Id)
            .OrderByDescending(_ => _.Id) ?? throw new UnAuthorizedError();
    }
}
