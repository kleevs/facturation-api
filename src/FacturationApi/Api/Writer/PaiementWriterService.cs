using FacturationApi.Models;
using FacturationApi.Spi;
using System;
using System.Linq;

namespace FacturationApi.Api
{
    public class PaiementWriterService
    {
        private readonly IPaiementProvider _provider;

        public PaiementWriterService(
            IPaiementProvider provider
        )
        {
            _provider = provider;
        }

        public IPaiementDb Save(IPaiement paiement)
        {
            var entity = _provider.Paiement.FirstOrDefault(_ => _.Id == paiement.Id);
            if (entity == null && paiement.Id <= 0)
            {
                entity = _provider.New();
                entity.DateCreation = paiement.DateCreation ?? DateTime.UtcNow;
                entity.FactureId = paiement.FactureId;
            }

            entity.Value = paiement.Value;

            return entity;
        }

        public void Delete(int paiementId) =>
            _provider.Remove(_provider.Paiement.Where(_ => _.Id == paiementId).FirstOrDefault());
    }
}
