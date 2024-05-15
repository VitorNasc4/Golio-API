using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Golio.Application.InputModels;
using Golio.Core.Entities;
using MediatR;

namespace Golio.Application.Commands.CreateProduct.CreateUser
{
    public class CreateSuggestionCommand : IRequest<Unit>
    {
        public double Value { get; set; }
        public string AutorName { get; set; }
        public string AutorEmail { get; set; }
        public int PriceId { get; set; }

    }
}