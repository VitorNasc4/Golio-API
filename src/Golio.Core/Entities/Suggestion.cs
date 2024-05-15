using System;
using System.Collections.Generic;

namespace Golio.Core.Entities
{
    public class Suggestion : BaseEntity
    {
        public double Value { get; set; }
        public string AutorName { get; set; }
        public string AutorEmail { get; set; }
        public int PriceId { get; set; }
    }
}
