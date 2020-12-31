using FacturationApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Web.Models.Output
{
    public class RecetteModel
    {
        public int Year { get; set; }
        public IEnumerable<RecetteRowModel> Rows { get; set; }

        public static Func<IRecette, RecetteModel> Map = (recette) => new RecetteModel
        {
            Year = recette.Year,
            Rows = recette.Rows.Select(RecetteRowModel.Map)
        };
    }

    public class RecetteRowModel
    {
        public DateTime Date { get; set; }
        public string NumeroFacture { get; set; }
        public string Client { get; set; }
        public string Nature { get; set; }
        public decimal MontantHT { get; set; }
        public decimal Tva { get; set; }
        public string ModeEncaissement { get; set; }

        public static Func<IRecetteRow, RecetteRowModel> Map = (row) => new RecetteRowModel
        {
            Date = row.Date,
            NumeroFacture = row.NumeroFacture,
            Client = row.Client,
            Nature = row.Nature,
            MontantHT = row.MontantHT,
            Tva = row.Tva,
            ModeEncaissement = row.ModeEncaissement
        };
    }
}
