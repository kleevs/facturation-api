using FacturationApi.Models;
using FacturationApi.Spi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Db
{
    public partial class Provider : DbContext,
        IFactureProvider,
        IFactureDbProvider,
        IPaiementProvider,
        IUserProvider
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

        IQueryable<IFacture> IFactureProvider.Facture => Facture.FromSql($"SELECT * FROM Facture").Include(_ => _.Services).Include(_ => _.Paiements);
        IQueryable<IFacturePdf> IFactureProvider.FacturePdf => Facture.FromSql($"SELECT * FROM Facture").Include(_ => _.Services).Include(_ => _.Paiements);
        IQueryable<IFactureOutput> IFactureProvider.FactureOutput => Facture.FromSql($"SELECT * FROM Facture").Include(_ => _.Services).Include(_ => _.Paiements);
        IQueryable<IFactureFull> IFactureProvider.FactureFull => Facture.FromSql($"SELECT * FROM Facture").Include(_ => _.Services).Include(_ => _.Paiements);
        IQueryable<IPaiementDb> IPaiementProvider.Paiement => Paiement.FromSql($"SELECT * FROM Paiement");
        IQueryable<IUser> IUserProvider.User => UtilisateurData.FromSql($"SELECT * FROM UtilisateurData");
        IQueryable<ILogin> IUserProvider.Login => Utilisateur.FromSql($"SELECT * FROM Utilisateur");
        IQueryable<IAuthenticateLogin> IUserProvider.AuthenticateLogin => AccountProvider.FromSql($"SELECT mdp.UserId AS Id, usr.Email, mdp.Value AS Password FROM MotDePasse AS mdp INNER JOIN Utilisateur AS usr ON mdp.UserId = usr.Id");
        IQueryable<IUserFilterable> IUserProvider.IUserFilterable => UtilisateurData.FromSql($"SELECT * FROM UtilisateurData");

        IQueryable<IFactureDb> IFactureDbProvider.Facture => Facture.FromSql($"SELECT * FROM Facture").Include(_ => _.Services).Include(_ => _.Paiements);
        IFactureDb IFactureDbProvider.NewFacture() => Facture.Add(new Facture()).Entity;
        IPaiementDb IPaiementProvider.NewPaiement() => Paiement.Add(new Paiement()).Entity;
        void IPaiementProvider.RemovePaiement(IPaiementDb item) => Paiement.Remove(item as Paiement);

        public Provider(DbContextOptions<Provider> options, IHasher hasher) : base(options)
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
            new ConsoleLoggerProvider()
        });
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ////optionsBuilder.UseLoggerFactory(_myLoggerFactory);
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

        public async Task<T> SaveChangesAsync<T>(T model)
        {
            await SaveChangesAsync();
            return model;
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
