using System;
using System.Threading;
using System.Threading.Tasks;
using Golio.Core.DTOs;
using Golio.Core.Repositories;
using Golio.Core.Services;
using MediatR;

namespace Golio.Application.Commands.CreateProduct.SendSuggestion
{
    public class SendSuggestionVoteCommandHandler : IRequestHandler<SendSuggestionVoteCommand, Unit>
    {
        private readonly ISuggestionRepository _suggestionRepository;
        private readonly IMessageBusService _messageBusService;

        public SendSuggestionVoteCommandHandler(
            ISuggestionRepository suggestionRepository,
            IMessageBusService messageBusService)
        {
            _suggestionRepository = suggestionRepository;
            _messageBusService = messageBusService;
        }
        public async Task<Unit> Handle(SendSuggestionVoteCommand request, CancellationToken cancellationToken)
        {
            var suggestion = await _suggestionRepository.GetSuggestionByIdAsync(request.SuggestionId);
            if (suggestion == null)
            {
                Console.WriteLine($"Suggestion with ID {request.SuggestionId} not found");
                return Unit.Value;
            }


            var suggestionVoteMessage = new SuggestionVoteMessage()
            {
                SuggestionId = suggestion.Id,
                IsValid = request.IsValid
            };

            await _messageBusService.SendMessageQueueAsync(suggestionVoteMessage);

            return Unit.Value;
        }
    }
}