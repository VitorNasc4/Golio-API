using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Golio.Application.ViewModels;
using MediatR;

namespace Golio.Application.Queries.GetProductById
{
    public class GetProductByIdQuery : IRequest<ProductDetailsViewModel>
    {
        public GetProductByIdQuery(int id)
        {
            Id = id;
        }
        public int Id { get; private set; }
    }
}