using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Golio.Core.DTOs;
using Golio.Core.Repositories;
using Golio.Core.Services;
using Golio.Infrastructure.CacheService;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Golio.Application.Commands.CreateProduct.SendSuggestion
{
    public class RemoveSuggestionCommandHandler : IRequestHandler<RemoveSuggestionCommand, Unit>
    {
        private readonly ISuggestionRepository _suggestionRepository;
        private readonly ICacheService _cacheService;


        public RemoveSuggestionCommandHandler(
            ISuggestionRepository suggestionRepository,
            ICacheService cacheService
            )
        {
            _suggestionRepository = suggestionRepository;
            _cacheService = cacheService;
        }
        public async Task<Unit> Handle(RemoveSuggestionCommand request, CancellationToken cancellationToken)
        {

            await _suggestionRepository.DeleteSuggestionAsync(request.SuggestionId);
            await _cacheService.UpdateDefaultProductQueryAsync();

            return Unit.Value;
        }
    }
}