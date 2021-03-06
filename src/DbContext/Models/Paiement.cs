﻿using FacturationApi.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Repository.Models
{
    public partial class Paiement : IPaiement, IPaiementDb
    {
        [Key]
        public int Id { get; set; }
        public int? FactureId { get; set; }
        public DateTime? DateCreation { get; set; }
        public decimal? Value { get; set; }
    }
}
