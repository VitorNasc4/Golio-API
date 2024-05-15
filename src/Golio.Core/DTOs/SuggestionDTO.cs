using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Golio.Core.DTOs
{
    public class SuggestionDTO
    {
        public double Value { get; set; }
        public string AutorName { get; set; }
        public string AutorEmail { get; set; }
        public int PriceId { get; set; }
    }
}