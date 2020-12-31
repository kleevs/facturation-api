using FacturationApi.Models;
using FacturationApi.Spi;
using FacturationApi.Tools;
using System;
using System.Linq;

namespace FacturationApi.Api
{
    public class FactureWriterService
    {
        private readonly IFactureDbProvider _provider;

        public FactureWriterService(
            IFactureDbProvider provider
        )
        {
            _provider = provider;
        }

        public IFactureDb Save(IFacture request)
        {
            Error.ThrowIf<UnAuthorizedToSaveFactureWithoutClientInfoError>(request.UserDataId < 0);

            if (request.DateEcheanceOption == 1)
            {
                request.DateEcheance = request.DateCreation.Value.AddDays(45);
                var date = request.DateCreation.Value.AddDays(45);
                request.DateEcheance = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
            }

            var entity = _provider.Facture.FirstOrDefault(_ => _.Id == request.Id);

            Error.ThrowIf<UnAuthorizedToSaveFactureError>((entity?.Paiements?.Count() ?? 0) > 0);

            if (request.Id <= 0)
            {
                entity = _provider.New();
                entity.DateCreation = request.DateCreation;
                entity.Numero = (_provider.Facture
                    .Where(_ => _.UserDataId == request.UserDataId)
                    .Select(_ => (int?)_.Numero)
                    .Max() ?? 0) + 1;
            }

            var serviceIds = request.Services?.Select(s => s.Id).Where(_ => _ > 0) ?? Enumerable.Empty<int>();
            foreach (var toRemove in entity.Services.Where(_ => !serviceIds.Contains(_.Id)).ToList())
            {
                entity.RemoveService(toRemove);
            }

            foreach (var service in request.Services ?? Enumerable.Empty<IService>())
            {
                var serviceEntity = entity.Services?.FirstOrDefault(_ => _.Id == service.Id);
                if (service.Id <= 0)
                {
                    entity.AddService(serviceEntity = entity.NewService);
                }

                serviceEntity.Description = service.Description;
                serviceEntity.Price = service.Price;
                serviceEntity.Quantity = service.Quantity;
                serviceEntity.Tva = service.Tva;
                serviceEntity.Unite = service.Unite;
            }

            entity.RaisonSociale = request.RaisonSociale;
            entity.DateEcheance = request.DateEcheance;
            entity.PaymentOption = request.PaymentOption;
            entity.LastName = request.LastName;
            entity.FirstName = request.FirstName;
            entity.Street = request.Street;
            entity.Complement = request.Complement;
            entity.ZipCode = request.ZipCode;
            entity.Country = request.Country;
            entity.City = request.City;
            entity.UserDataId = request.UserDataId;

            return entity;
        }

        public int Delete(int factureId)
        {
            var facture = _provider.Facture.Where(_ => _.Id == factureId).FirstOrDefault();
            var lastNumero = _provider.Facture
                    .Where(_ => _.UserDataId == facture.UserDataId)
                    .Select(_ => (int?)_.Numero)
                    .Max() ?? 0;

            Error.ThrowIf<DeleteFactureError>(lastNumero != facture.Numero);
            _provider.Remove(facture);

            return factureId;
        }
    }
}
