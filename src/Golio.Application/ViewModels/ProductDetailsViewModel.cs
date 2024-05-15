using System;
using System.Collections.Generic;
using System.Text;

namespace Golio.Application.ViewModels
{
    public class ProductDetailsViewModel
    {
        public ProductDetailsViewModel(string name, string brand, int volume, List<PriceDetailsViewModel> prices)
        {
            Name = name;
            Brand = brand;
            Volume = volume;
            Prices = prices;
        }

        public string Name { get; private set; }
        public string Brand { get; private set; }
        public int Volume { get; private set; }
        public List<PriceDetailsViewModel> Prices { get; private set; }
    }
}
