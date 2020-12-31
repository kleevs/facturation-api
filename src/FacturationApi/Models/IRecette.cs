using System;
using System.Collections.Generic;

namespace FacturationApi.Models
{
    public interface IRecette
    {
        int Year { get; }
        IEnumerable<IRecetteRow> Rows { get; }
    }

    public interface IRecetteRow
    {
        DateTime Date { get; }
        string NumeroFacture { get; }
        string Client { get; }
        string Nature { get; }
        decimal MontantHT { get; }
        decimal Tva { get; }
        string ModeEncaissement { get; }
    }

    public class Recette : IRecette
    {
        public int Year { get; set; }
        public IEnumerable<IRecetteRow> Rows { get; set; }
    }

    public class RecetteRow : IRecetteRow
    {
        public DateTime Date { get; set; }
        public string NumeroFacture { get; set; }
        public string Client { get; set; }
        public string Nature { get; set; }
        public decimal MontantHT { get; set; }
        public decimal Tva { get; set; }
        public string ModeEncaissement { get; set; }
    }
}
