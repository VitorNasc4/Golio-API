using System.Threading;
using System.Threading.Tasks;
using Golio.Core.Entities;
using Golio.Core.Repositories;
using Golio.Infrastructure.CacheService;
using MediatR;

namespace Golio.Application.Commands.CreateProduct.CreateUser
{
    public class UpdatePriceCommandHandler : IRequestHandler<UpdatePriceCommand, Unit>
    {
        private readonly IPriceRepository _priceRepository;
        private readonly ICacheService _cacheService;

        public UpdatePriceCommandHandler(IPriceRepository priceRepository, ICacheService cacheService)
        {
            _priceRepository = priceRepository;
            _cacheService = cacheService;
        }
        public async Task<Unit> Handle(UpdatePriceCommand request, CancellationToken cancellationToken)
        {
            var updatedPrice = new Price
            {
                Id = request.PriceId,
                Value = request.Value
            };

            await _priceRepository.UpdatePriceAsync(updatedPrice);
            await _cacheService.UpdateDefaultProductQueryAsync();

            return Unit.Value;
        }
    }
}