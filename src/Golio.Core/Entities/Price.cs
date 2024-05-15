using System;
using System.Collections.Generic;

namespace Golio.Core.Entities
{
    public class Price : BaseEntity
    {
        public Store Store { get; set; }
        public double Value { get; set; }
        public int ProductId { get; set; }
        public int StoreId { get; set; }
        public List<Suggestion> Suggestions { get; set; } //= new List<Suggestion>();

        public void Update(double newPrice)
        {
            Value = newPrice;
        }

    }
}
