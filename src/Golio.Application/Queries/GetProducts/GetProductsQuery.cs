using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Golio.Application.ViewModels;
using MediatR;

namespace Golio.Application.Queries.GetProducts
{
    public class GetProductsQuery : IRequest<List<ProductDetailsViewModel>>
    {
        public GetProductsQuery(string query)
        {
            Query = query;
        }
        public string Query { get; private set; }
    }
}