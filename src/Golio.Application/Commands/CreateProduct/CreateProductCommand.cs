using System.Collections.Generic;
using Golio.Application.InputModels;
using MediatR;

namespace Golio.Application.Commands.CreateProduct.CreateUser
{
    public class CreateProductCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public int Volume { get; set; }
        public List<CreatePriceInputModel> Prices { get; set; }

    }
}