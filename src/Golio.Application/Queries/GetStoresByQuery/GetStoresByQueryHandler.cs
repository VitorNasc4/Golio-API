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
    public class GetStoresByQueryHandler : IRequestHandler<GetStoresByQuery, List<StoreViewModel>>
    {
        private readonly IStoreRepository _storeRepository;

        public GetStoresByQueryHandler(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }
        public async Task<List<StoreViewModel>> Handle(GetStoresByQuery request, CancellationToken cancellationToken)
        {
            var stores = await _storeRepository.GetStoresByQueryAsync(request.Query);

            if (stores == null)
            {
                return null;
            }

            var storeDetailsViewModelList = stores.Select(store => new StoreViewModel(store.Name, store.Address, store.City, store.State, store.ZipCode)).ToList();


            return storeDetailsViewModelList;
        }
    }
}