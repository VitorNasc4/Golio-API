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
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDetailsViewModel>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<ProductDetailsViewModel> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductByIdAsync(request.Id);

            if (product == null)
            {
                return null;
            }

            var priceDetailsViewModelList = new List<PriceDetailsViewModel>();

            foreach (var price in product.Prices)
            {
                var priceDetailsViewModel = new PriceDetailsViewModel() { Value = price.Value, StoreName = price.Store.Name };

                var suggestionsViewModelList = new List<SuggestionViewModel>();
                foreach (var suggestion in price.Suggestions)
                {
                    var suggestionViewModel = new SuggestionViewModel(suggestion.Value, suggestion.AutorName);
                    suggestionsViewModelList.Add(suggestionViewModel);
                }
                priceDetailsViewModel.Suggestions = suggestionsViewModelList;


                priceDetailsViewModelList.Add(priceDetailsViewModel);
            }

            var productViewModel = new ProductDetailsViewModel
            (
                product.Name,
                product.Brand,
                product.Volume,
                priceDetailsViewModelList
            );

            return productViewModel;
        }
    }
}