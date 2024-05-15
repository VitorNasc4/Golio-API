using System;
using System.Threading;
using System.Threading.Tasks;
using Golio.Core.Entities;
using Golio.Core.Repositories;
using Golio.Infrastructure.CacheService;
using MediatR;

namespace Golio.Application.Commands.CreateProduct.CreateUser
{
    public class CreatePriceCommandHandler : IRequestHandler<CreatePriceCommand, int>
    {

        private readonly IPriceRepository _priceRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly ICacheService _cacheService;

        public CreatePriceCommandHandler(IPriceRepository priceRepository, IStoreRepository storeRepository, ICacheService cacheService)
        {
            _priceRepository = priceRepository;
            _storeRepository = storeRepository;
            _cacheService = cacheService;
        }
        public async Task<int> Handle(CreatePriceCommand request, CancellationToken cancellationToken)
        {
            var store = await _storeRepository.GetStoreByIdAsync(request.StoreId);
            var price = new Price
            {
                Value = request.Value,
                Store = store,
                StoreId = request.StoreId,
                ProductId = request.ProductId,
                CreatedAt = DateTime.UtcNow
            };

            await _priceRepository.AddPriceAsync(price);
            await _cacheService.UpdateDefaultProductQueryAsync();

            return price.Id;
        }
    }
}