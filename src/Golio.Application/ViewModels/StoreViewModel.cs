using System;
using System.Collections.Generic;
using System.Text;

namespace Golio.Application.ViewModels
{
    public class StoreViewModel
    {
        public StoreViewModel(string name, string address, string city, string state, string zipCode)
        {
            Name = name;
            Address = address;
            City = city;
            State = state;
            ZipCode = zipCode;
        }

        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

    }
}
