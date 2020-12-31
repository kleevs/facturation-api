using FacturationApi.Models;
using System;
using System.Collections.Generic;

namespace Excel
{
    public class Sheet
    {
        public string Name { get; set; }
        public IEnumerable<Cell> Cells { get; set; }
    }
}
