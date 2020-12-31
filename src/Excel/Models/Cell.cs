using FacturationApi.Models;
using System;

namespace Excel
{
    public class Cell
    {
        public string Name { get; set; }
        public Style Style { get; set; }
        public string Value { get; set; }
        public string Function { get; set; }
    }
}
