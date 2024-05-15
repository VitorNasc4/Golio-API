using System;
using System.Collections.Generic;

namespace Golio.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public int Volume { get; set; }
        public List<Price> Prices { get; set; }
    }
}
