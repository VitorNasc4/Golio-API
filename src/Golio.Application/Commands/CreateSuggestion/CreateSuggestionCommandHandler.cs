using System;
using System.Threading;
using System.Threading.Tasks;
using Golio.Core.Entities;
using Golio.Core.Repositories;
using Golio.Infrastructure.CacheService;
using MediatR;

namespace Golio.Application.Commands.CreateProduct.CreateUser
{
    public class CreateSuggestionCommandHandler : IRequestHandler<CreateSuggestionCommand, Unit>
    {
        private readonly ISuggestionRepository _suggestionRepository;
        private readonly ICacheService _cacheService;


        public CreateSuggestionCommandHandler(ISuggestionRepository suggestionRepository, ICacheService cacheService)
        {
            _suggestionRepository = suggestionRepository;
            _cacheService = cacheService;
        }
        public async Task<Unit> Handle(CreateSuggestionCommand request, CancellationToken cancellationToken)
        {
            var suggestion = new Suggestion()
            {
                AutorName = request!.AutorName,
                AutorEmail = request!.AutorEmail,
                PriceId = request.PriceId,
                Value = request.Value
            };
            var suggestionAlreadyExists = await _suggestionRepository.CheckSuggestionExistsAsync(suggestion);

            if (suggestionAlreadyExists)
            {
                Console.WriteLine("Sugestão já existente");
                return Unit.Value;
            }

            await _suggestionRepository.AddSuggestionAsync(suggestion);
            await _suggestionRepository.SaveChangesAsync();
            await _cacheService.UpdateDefaultProductQueryAsync();

            return Unit.Value;
        }
    }
}