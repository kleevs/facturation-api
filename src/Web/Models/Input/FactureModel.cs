using System;
using System.Collections.Generic;

namespace Web.Models.Input
{
    public partial class FactureModel
    {
        public int Id { get; set; }
        public int? UserDataId { get; set; }
        public DateTime? DateCreation { get; set; }
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
        public IEnumerable<ServiceModel> Services { get; set; }
    }
}
