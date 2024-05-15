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
    public class SendSuggestionCommandHandler : IRequestHandler<SendSuggestionCommand, Unit>
    {
        private readonly IPriceRepository _priceRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMessageBusService _messageBusService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SendSuggestionCommandHandler(
            IPriceRepository priceRepository,
            IUserRepository userRepository,
            IMessageBusService messageBusService,
            IHttpContextAccessor httpContextAccessor)
        {
            _priceRepository = priceRepository;
            _userRepository = userRepository;
            _messageBusService = messageBusService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Unit> Handle(SendSuggestionCommand request, CancellationToken cancellationToken)
        {
            var price = await _priceRepository.GetPriceByIdAsync(request.PriceId);
            if (price == null)
            {
                Console.WriteLine("Preço não encontrado");
                return Unit.Value;
            }

            var userEmail = _httpContextAccessor.HttpContext?.User.Identities.FirstOrDefault().Claims.Where(c => c.Type == "userName").FirstOrDefault().Value;
            var userName = "Usuario Desconhecido";
            if (userEmail is not null)
            {
                var user = await _userRepository.GetUserByEmailAsync(userEmail);
                userName = user.FullName;
            }

            var suggestionDTO = new SuggestionDTO()
            {
                AutorName = userName,
                AutorEmail = userEmail,
                PriceId = price.Id,
                Value = request.NewPrice
            };

            await _messageBusService.SendMessageQueueAsync(suggestionDTO);

            return Unit.Value;
        }
    }
}