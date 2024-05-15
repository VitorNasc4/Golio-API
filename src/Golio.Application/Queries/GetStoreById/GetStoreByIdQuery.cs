using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Golio.Application.ViewModels;
using MediatR;

namespace Golio.Application.Queries.GetProductById
{
    public class GetStoreByIdQuery : IRequest<StoreViewModel>
    {
        public GetStoreByIdQuery(int id)
        {
            Id = id;
        }
        public int Id { get; private set; }
    }
}