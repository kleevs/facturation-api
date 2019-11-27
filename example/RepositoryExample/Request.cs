using Db;
using FacturationApi.Api;
using FacturationApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RepositoryExample
{
    class Request
    {
        public static void Run(string[] args)
        {
            Write(args);
        }

        public static void Read(string[] args)
        {
            var options = new DbContextOptionsBuilder<Provider>();
            options.UseMySQL("server=fla-virtualbox;database=facturation;user=facturation;password=facturation");
            var provider = new Provider(options.Options, new Hasher());
            var result = new FactureService(provider).List().ToList();
        }

        public static void Write(string[] args)
        {
            var options = new DbContextOptionsBuilder<Provider>();
            options.UseMySQL("server=fla-virtualbox;database=facturation;user=facturation;password=facturation");
            var provider = new Provider(options.Options, new Hasher());
            var result = new FactureWriterService(provider).Save(new Facture
            {
                Id = 6,
                UserDataId = 1,
                LastName = "Test",
                Services = new List<IService>
                {
                    new Service
                    {
                        Id = 3,
                        Description = "Test deuxième",
                        Price = 10,
                        Quantity = 100,
                        Tva = 20,
                        Unite = "jour"
                    }
                }
            });
            provider.SaveChanges();
        }

        class Facture : IFacture
        {
            public int UserDataId { get; set; }
            public DateTime? DateCreation { get; set; }
            public DateTime? DateEcheance { get; set; }
            public int? DateEcheanceOption { get; set; }
            public int? PaymentOption { get; set; }
            public string RaisonSociale { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string Street { get; set; }
            public string Complement { get; set; }
            public string ZipCode { get; set; }
            public string Country { get; set; }
            public string City { get; set; }
            public IEnumerable<IService> Services { get; set; }
            public IEnumerable<IPaiement> Paiements { get; set; }
            public int? Id { get; set; }
        }

        class Service : IService
        {
            public string Description { get; set; }
            public decimal? Price { get; set; }
            public decimal? Quantity { get; set; }
            public decimal? Tva { get; set; }
            public string Unite { get; set; }
            public int? Id { get; set; }
        }
    }
}
