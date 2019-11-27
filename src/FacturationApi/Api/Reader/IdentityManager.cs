using FacturationApi.Models;
using FacturationApi.Spi;
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
            .Where(_ => _.Email == email)
            .Where(_ => _.Password == password)
            .FirstOrDefault();

        public IEnumerable<IUser> GetUserInfo() => _loginReader.IUserFilterable
            .Where(_ => _.UserId == _authenticationProvider.Current.Id);
    }
}
