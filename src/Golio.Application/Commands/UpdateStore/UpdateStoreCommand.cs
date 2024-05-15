using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Golio.Application.InputModels;
using Golio.Core.Entities;
using MediatR;

namespace Golio.Application.Commands.CreateProduct.CreateUser
{
    public class UpdateStoreCommand : IRequest<Unit>
    {
        public int StoreId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

    }
}