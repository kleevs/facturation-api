using FacturationApi.Models;
using System;
using System.Collections.Generic;

namespace Web.Models.Input
{
    public partial class FactureModel : IFacture
    {
        DateTime? IFacture.DateEcheance { get; set; }
        IEnumerable<IService> IFacture.Services => Services;
        IEnumerable<IPaiement> IFacture.Paiements => null;
        int IFacture.UserDataId => UserDataId ?? 0;
        int IFacture.Numero => 0;
    }
}
