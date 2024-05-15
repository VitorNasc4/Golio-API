using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Golio.Core.Entities;
using Golio.Core.Repositories;
using Golio.Infrastructure.CacheService;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Golio.Application.Commands.CreateProduct.CreateUser
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICacheService _cacheService;

        public CreateProductCommandHandler(IProductRepository productRepository, ICacheService cacheService)
        {
            _productRepository = productRepository;
            _cacheService = cacheService;
        }
        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var priceList = new List<Price>();
            foreach (var priceInputModel in request.Prices)
            {
                var price = priceInputModel.ToEntity();
                priceList.Add(price);
            }
            var product = new Product
            {
                Name = request.Name,
                Brand = request.Brand,
                Volume = request.Volume,
                Prices = priceList,
                CreatedAt = DateTime.UtcNow,
                Active = true
            };
            await _productRepository.AddProductAsync(product);
            await _cacheService.UpdateDefaultProductQueryAsync();

            return product.Id;
        }
    }
}