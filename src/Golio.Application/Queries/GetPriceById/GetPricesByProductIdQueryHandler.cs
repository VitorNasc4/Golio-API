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
    public class GetPriceByIdQueryHandler : IRequestHandler<GetPriceByIdQuery, PriceDetailsViewModel>
    {
        private readonly IPriceRepository _priceRepository;

        public GetPriceByIdQueryHandler(IPriceRepository priceRepository)
        {
            _priceRepository = priceRepository;
        }
        public async Task<PriceDetailsViewModel> Handle(GetPriceByIdQuery request, CancellationToken cancellationToken)
        {
            var price = await _priceRepository.GetPriceByIdAsync(request.Id);

            if (price == null)
            {
                return null;
            }

            var suggestionViewModelList = new List<SuggestionViewModel>();
            foreach (var suggestion in price.Suggestions)
            {
                var suggestionViewModel = new SuggestionViewModel(suggestion.Value, suggestion.AutorName);
                suggestionViewModelList.Add(suggestionViewModel);
            }

            var priceDetailsViewModel = new PriceDetailsViewModel()
            {
                Value = price.Value,
                StoreName = price.Store.Name,
                Suggestions = suggestionViewModelList
            };

            return priceDetailsViewModel;
        }
    }
}