using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Golio.Application.InputModels;
using Golio.Core.Entities;
using MediatR;

namespace Golio.Application.Commands.CreateProduct.CreateUser
{
    public class CreatePriceCommand : IRequest<int>
    {
        public int Value { get; set; }
        public int ProductId { get; set; }
        public int StoreId { get; set; }

    }
}