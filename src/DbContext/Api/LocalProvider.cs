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
    public partial class LocalProvider : AbstractProvider
    {       
        public LocalProvider(DbContextOptions<LocalProvider> options, IAuthenticationProvider authProvider, IHasher hasher) : base(options, authProvider, hasher)
        {
        }
    }
}
