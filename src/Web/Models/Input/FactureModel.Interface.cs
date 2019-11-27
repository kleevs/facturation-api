using FacturationApi.Models;
using System;
using System.Collections.Generic;

namespace Web.Models.Input
{
    public partial class FactureModel : IFacture
    {
        DateTime? IFacture.DateEcheance { get; set; }
        string IFacture.LastName => Contact?.LastName;
        string IFacture.FirstName => Contact?.FirstName;
        string IFacture.Street => Contact?.Address?.Street;
        string IFacture.Complement => Contact?.Address?.Complement;
        string IFacture.ZipCode => Contact?.Address?.Cp;
        string IFacture.Country => Contact?.Address?.Country;
        string IFacture.City => Contact?.Address?.City;
        IEnumerable<IService> IFacture.Services => Services;
        IEnumerable<IPaiement> IFacture.Paiements => null;
        string IFacture.RaisonSociale => Contact?.RaisonSociale;
        int IFacture.UserDataId => UserDataId ?? 0;
    }
}
