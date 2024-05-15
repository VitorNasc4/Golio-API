using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Golio.Application.ViewModels;
using MediatR;

namespace Golio.Application.Queries.GetProductById
{
    public class GetStoresByQuery : IRequest<List<StoreViewModel>>
    {
        public GetStoresByQuery(string query)
        {
            Query = query;
        }
        public string Query { get; private set; }
    }
}