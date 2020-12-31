using FacturationApi.Models;
using FacturationApi.Spi;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Db
{
    public partial class MySqlProvider : AbstractProvider, IProvider
    {
        IQueryable<IAuthenticateLogin> IUserProvider.AuthenticateLogin => AccountProvider.FromSqlRaw($"SELECT mdp.UserId AS Id, usr.Email, mdp.Value AS Password FROM MotDePasse AS mdp INNER JOIN Utilisateur AS usr ON mdp.UserId = usr.Id");
        
        public MySqlProvider(DbContextOptions<MySqlProvider> options, IAuthenticationProvider authProvider, IHasher hasher) : base(options, authProvider, hasher)
        {
        }
    }
}
