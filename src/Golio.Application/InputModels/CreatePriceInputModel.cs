using System;
using System.Collections.Generic;
using System.Text;
using Golio.Core.Entities;

namespace Golio.Application.InputModels
{
    public class CreatePriceInputModel
    {
        public double Value { get; set; }
        public int ProductId { get; set; }
        public CreateStoreInputModel Store { get; set; }
        public Price ToEntity()
        {
            return new Price
            {
                Value = Value,
                ProductId = ProductId,
                Store = Store.ToEntity(),
                CreatedAt = DateTime.UtcNow,
                Suggestions = new List<Suggestion>()
            };
        }
    }
}
