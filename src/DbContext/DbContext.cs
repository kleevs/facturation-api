using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Db
{
    public partial class DbContext : Microsoft.EntityFrameworkCore.DbContext, 
        IDbContext, 
        ISaveDbContext,
        IFactureDbContext,
        IServiceDbContext,
        IPaiementDbContext,
        IFactureServiceDbContext,
        IUserDbContext
    {
        private readonly IHasher _hasher;
        private readonly IList<Action> _callbacks;
        private readonly IList<Action> _beforeCallbacks;

        public virtual DbSet<Facture> Facture { get; set; }
        public virtual DbSet<Service> Service { get; set; }
        public virtual DbSet<Paiement> Paiement { get; set; }
        public virtual DbSet<Utilisateur> Utilisateur { get; set; }
        public virtual DbSet<UtilisateurData> UtilisateurData { get; set; }
        public virtual DbSet<MotDePasse> MotDePasse { get; set; }
        public virtual DbSet<Account> AccountProvider { get; set; }
        
        public IQueryable<Account> Account => AccountProvider
            .FromSql("SELECT mdp.UserId AS Id, usr.Email, mdp.Value AS Password FROM MotDePasse AS mdp INNER JOIN Utilisateur AS usr ON mdp.UserId = usr.Id");
        

        public DbContext(DbContextOptions<DbContext> options, IHasher hasher) : base(options)
        {
            _callbacks = new List<Action>();
            _beforeCallbacks = new List<Action>();
            _hasher = hasher;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .Property(_ => _.Password)
                .HasConversion(
                    v => _hasher.Compute(v),
                    v => null
                );
        }

        public static readonly LoggerFactory _myLoggerFactory =
        new LoggerFactory(new[] {
            //new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()
            new ConsoleLoggerProvider()
        });
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_myLoggerFactory); 
        }

        public async Task<int> SaveChangesAsync()
        {
            foreach (var callback in _beforeCallbacks) callback();
            var res = await base.SaveChangesAsync();
            foreach (var callback in _callbacks) callback();
            _callbacks.Clear();
            _beforeCallbacks.Clear();
            return res;
        }

        public void OnBeforeSaveChanges(Action callback)
        {
            _beforeCallbacks.Add(callback);
        }

        public void OnSaveChanges(Action callback)
        {
            _callbacks.Add(callback);
        }
    }
}
