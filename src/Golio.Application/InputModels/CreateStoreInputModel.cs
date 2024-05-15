using System;
using System.Collections.Generic;
using System.Text;
using Golio.Core.Entities;

namespace Golio.Application.InputModels
{
    public class CreateStoreInputModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public Store ToEntity()
        {
            return new Store
            {
                Name = Name,
                Address = Address,
                City = City,
                State = State,
                ZipCode = ZipCode,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
