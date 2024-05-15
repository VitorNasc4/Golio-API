using System;
using System.Collections.Generic;
using System.Text;

namespace Golio.Application.ViewModels
{
    public class PriceDetailsViewModel
    {
        public double Value { get; set; }
        public string StoreName { get; set; }
        public List<SuggestionViewModel> Suggestions { get; set; } = new List<SuggestionViewModel>();
    }
}
