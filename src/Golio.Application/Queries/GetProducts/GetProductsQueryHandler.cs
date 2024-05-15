using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Golio.Application.ViewModels;
using Golio.Core.Entities;
using Golio.Core.Repositories;
using Golio.Infrastructure.CacheService;
using Golio.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Golio.Application.Queries.GetProducts
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductDetailsViewModel>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICacheService _cacheService;
        private readonly IConfiguration _configuration;

        public GetProductsQueryHandler(IProductRepository productRepository, ICacheService cacheService, IConfiguration configuration)
        {
            _productRepository = productRepository;
            _cacheService = cacheService;
            _configuration = configuration;
        }
        public async Task<List<ProductDetailsViewModel>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var query = request.Query;
            if (string.IsNullOrEmpty(query))
            {
                query = _configuration["Configs:DefaultProductQuery"];
            }

            var products = await _cacheService.GetAsync<List<Product>>(query);

            if (products is null)
            {
                products = await _productRepository.GetProductsAsync(request.Query);
                if (products is null)
                {
                    return null;
                }

                await _cacheService.SetAsync(query, products);
            }


            if (products.Count == 0)
            {
                return new List<ProductDetailsViewModel>();
            }

            var productsViewModelList = products
                .Select(product => new ProductDetailsViewModel
                (
                    product.Name,
                    product.Brand,
                    product.Volume,
                    product.Prices.Select(price => new PriceDetailsViewModel()
                    {
                        Value = price.Value,
                        StoreName = price.Store.Name,
                        Suggestions = price.Suggestions.Select(suggestion => new SuggestionViewModel(suggestion.Value, suggestion.AutorName)).ToList()
                    }).ToList()
                ))
                .ToList();

            return productsViewModelList;
        }
    }
}