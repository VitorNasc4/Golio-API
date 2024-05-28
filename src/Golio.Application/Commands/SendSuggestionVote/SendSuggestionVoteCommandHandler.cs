using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Golio.Core.DTOs;
using Golio.Core.Repositories;
using Golio.Core.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Golio.Application.Commands.CreateProduct.SendSuggestion
{
    public class SendSuggestionVoteCommandHandler : IRequestHandler<SendSuggestionVoteCommand, Unit>
    {
        private readonly ISuggestionRepository _suggestionRepository;
        private readonly IMessageBusService _messageBusService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SendSuggestionVoteCommandHandler(
            ISuggestionRepository suggestionRepository,
            IMessageBusService messageBusService,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _suggestionRepository = suggestionRepository;
            _messageBusService = messageBusService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Unit> Handle(SendSuggestionVoteCommand request, CancellationToken cancellationToken)
        {
            var suggestion = await _suggestionRepository.GetSuggestionByIdAsync(request.SuggestionId);
            if (suggestion == null)
            {
                Console.WriteLine($"Suggestion with ID {request.SuggestionId} not found");
                return Unit.Value;
            }

            var userEmail = _httpContextAccessor.HttpContext?.User.Identities.FirstOrDefault().Claims.Where(c => c.Type == "userName").FirstOrDefault().Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                Console.WriteLine("Error getting author's email");
                return Unit.Value;
            }
            var suggestionVoteMessage = new SuggestionVoteMessage()
            {
                SuggestionId = suggestion.Id,
                IsValid = request.IsValid,
                EmailAutor = userEmail,
            };

            await _messageBusService.SendMessageQueueAsync(suggestionVoteMessage);

            return Unit.Value;
        }
    }
}