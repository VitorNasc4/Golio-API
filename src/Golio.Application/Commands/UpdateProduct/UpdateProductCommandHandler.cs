using System.Threading;
using System.Threading.Tasks;
using Golio.Core.Entities;
using Golio.Core.Repositories;
using Golio.Infrastructure.CacheService;
using MediatR;

namespace Golio.Application.Commands.CreateProduct.CreateUser
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICacheService _cacheService;

        public UpdateProductCommandHandler(IProductRepository productRepository, ICacheService cacheService)
        {
            _productRepository = productRepository;
            _cacheService = cacheService;
        }
        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var updatedProduct = new Product
            {
                Id = request.ProductId,
                Name = request.Name,
                Brand = request.Brand,
                Volume = request.Volume
            };

            await _productRepository.UpdateProductAsync(updatedProduct);
            await _cacheService.UpdateDefaultProductQueryAsync();

            return Unit.Value;
        }
    }
}