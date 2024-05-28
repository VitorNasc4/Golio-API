using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Golio.Application.InputModels;
using Golio.Core.Entities;
using MediatR;

namespace Golio.Application.Commands.CreateProduct.CreateUser
{
    public class UpdatePriceCommand : IRequest<Unit>
    {
        public int PriceId { get; set; }
        public double Value { get; set; }
    }
}