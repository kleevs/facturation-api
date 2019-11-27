using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Db
{
    public partial class FakeDbContext : 
        IDbContext, 
        ISaveDbContext,
        IFactureDbContext,
        IServiceDbContext,
        IFactureServiceDbContext
    {
        private readonly IList<Action> _callbacks;
        private readonly IList<Action> _beforeCallbacks;

        private readonly static FakeDbSet<Facture> _facture = new FakeDbSet<Facture>();
        private readonly static FakeDbSet<Service> _service = new FakeDbSet<Service>();
        private readonly static FakeDbSet<UtilisateurData> _utilisateurData = new FakeDbSet<UtilisateurData>();

        public DbSet<Facture> Facture => _facture;
        public DbSet<Service> Service => _service;
        public DbSet<UtilisateurData> UtilisateurData => _utilisateurData;


        public FakeDbContext()
        {
            _callbacks = new List<Action>();
            _beforeCallbacks = new List<Action>();
        }

        public async Task<int> SaveChangesAsync()
        {
            foreach (var callback in _beforeCallbacks) callback();
            foreach (var callback in _callbacks) callback();
            _callbacks.Clear();
            _beforeCallbacks.Clear();
            return 1;
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
