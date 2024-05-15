using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Golio.Application.Queries.GetProductById;
using Golio.Application.ViewModels;
using Golio.Core.Repositories;
using Golio.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Golio.Application.Queries.GetProducts
{
    public class GetStoreByIdQueryHandler : IRequestHandler<GetStoreByIdQuery, StoreViewModel>
    {
        private readonly IStoreRepository _storeRepository;

        public GetStoreByIdQueryHandler(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }
        public async Task<StoreViewModel> Handle(GetStoreByIdQuery request, CancellationToken cancellationToken)
        {
            var store = await _storeRepository.GetStoreByIdAsync(request.Id);

            if (store == null)
            {
                return null;
            }

            var storeDetailsViewModel = new StoreViewModel(store.Name, store.Address, store.City, store.State, store.ZipCode);

            return storeDetailsViewModel;
        }
    }
}