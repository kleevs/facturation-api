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
    public interface IProvider :
        IFactureProvider,
        IFactureDbProvider,
        IPaiementProvider,
        IUserProvider
    {
        Task<int> SaveChangesAsync();
        Task<T> SaveChangesAsync<T>(T model);
    }
}
