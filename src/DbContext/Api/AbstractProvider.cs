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
    public abstract class AbstractProvider : DbContext, IProvider
    {
        private readonly IHasher _hasher;
        private readonly IList<Action> _callbacks;
        private readonly IList<Action> _beforeCallbacks;
        private readonly IAuthenticationProvider _authProvider;

        public virtual DbSet<Facture> Facture { get; set; }
        public virtual DbSet<Service> Service { get; set; }
        public virtual DbSet<Paiement> Paiement { get; set; }
        public virtual DbSet<Utilisateur> Utilisateur { get; set; }
        public virtual DbSet<UtilisateurData> UtilisateurData { get; set; }
        public virtual DbSet<MotDePasse> MotDePasse { get; set; }
        public virtual DbSet<Account> AccountProvider { get; set; }

        protected IQueryable<Facture> FactureFilteredByCurrentUser => Facture.Include(_ => _.Services).Include(_ => _.Paiements).Where(_ => _.UserData.UserId == _authProvider.Current.Id);

        IQueryable<IFacture> IFactureProvider.Facture => FactureFilteredByCurrentUser;
        IQueryable<IFacturePdf> IFactureProvider.FacturePdf => FactureFilteredByCurrentUser.Include(_ => _.UserData);
        IQueryable<IFactureOutput> IFactureProvider.FactureOutput => FactureFilteredByCurrentUser;
        IQueryable<IFactureFull> IFactureProvider.FactureFull => FactureFilteredByCurrentUser;
        IQueryable<IFactureDb> IFactureDbProvider.Facture => FactureFilteredByCurrentUser;
        IQueryable<IPaiementDb> IPaiementProvider.Paiement => Paiement;
        IQueryable<IUser> IUserProvider.User => UtilisateurData;
        IQueryable<ILogin> IUserProvider.Login => Utilisateur;
        IQueryable<IAuthenticateLogin> IUserProvider.AuthenticateLogin => AccountProvider;
        IQueryable<IUserFilterable> IUserProvider.IUserFilterable => UtilisateurData;

        public AbstractProvider(DbContextOptions options, IAuthenticationProvider authProvider, IHasher hasher) : base(options)
        {
            _callbacks = new List<Action>();
            _beforeCallbacks = new List<Action>();
            _authProvider = authProvider;
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

            modelBuilder.Entity<MotDePasse>()
                .Property(_ => _.Value)
                .HasConversion(
                    v => _hasher.Compute(v),
                    v => null
                );
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

        public void OnBeforeSaveChanges(Action callback) => _beforeCallbacks.Add(callback);
        public void OnSaveChanges(Action callback) => _callbacks.Add(callback);
        IFactureDb IFactureDbProvider.New() => Facture.Add(new Facture()).Entity;
        IPaiementDb IPaiementProvider.New() => Paiement.Add(new Paiement()).Entity;
        void IPaiementProvider.Remove(IPaiementDb item) => Paiement.Remove(item as Paiement);
        IUserDb IUserProvider.NewUser() => MotDePasse.Add(new MotDePasse()).Entity;
        IUserInfoDb IUserProvider.NewUserInfo() => UtilisateurData.Add(new UtilisateurData()).Entity;
        void IFactureDbProvider.Remove(IFactureDb item) => Facture.Remove(item as Facture);
    }
}
