using System.Threading;
using System.Threading.Tasks;
using Golio.Core.Entities;
using Golio.Core.Repositories;
using Golio.Infrastructure.CacheService;
using MediatR;

namespace Golio.Application.Commands.CreateProduct.CreateUser
{
    public class UpdateStoreCommandHandler : IRequestHandler<UpdateStoreCommand, Unit>
    {
        private readonly IStoreRepository _storeRepository;
        private readonly ICacheService _cacheService;

        public UpdateStoreCommandHandler(IStoreRepository storeRepository, ICacheService cacheService)
        {
            _storeRepository = storeRepository;
            _cacheService = cacheService;
        }
        public async Task<Unit> Handle(UpdateStoreCommand request, CancellationToken cancellationToken)
        {
            var updatedStore = new Store
            {
                Id = request.StoreId,
                Name = request.Name,
                Address = request.Address,
                City = request.City,
                State = request.State,
                ZipCode = request.ZipCode,
            };

            await _storeRepository.UpdateStoreAsync(updatedStore);
            await _cacheService.UpdateDefaultProductQueryAsync();

            return Unit.Value;
        }
    }
}