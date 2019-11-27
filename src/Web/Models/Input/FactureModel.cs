using System;
using System.Collections.Generic;

namespace Web.Models.Input
{
    public partial class FactureModel
    {
        public int? Id { get; set; }
        public int? UserDataId { get; set; }
        public DateTime? DateCreation { get; set; }
        public int? DateEcheanceOption { get; set; }
        public int? PaymentOption { get; set; }
        public ContactModel Contact { get; set; }
        public IEnumerable<ServiceModel> Services { get; set; }
    }
}
