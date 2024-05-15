using System;
using System.Collections.Generic;
using System.Text;

namespace Golio.Application.ViewModels
{
    public class SuggestionViewModel
    {
        public SuggestionViewModel(double value, string autor)
        {
            Value = value;
            Autor = autor;
        }

        public double Value { get; private set; }
        public string Autor { get; private set; }
    }
}
