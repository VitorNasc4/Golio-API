using Golio.Application.Commands.CreateProduct.SendSuggestion;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Golio.API.Controllers
{
    [Route("api/suggestions")]
    [Authorize]
    public class SuggestionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SuggestionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // api/suggestions/1
        [HttpPost("{priceId}")]
        [Authorize(Roles = "user, admin")]
        public async Task<ActionResult> SendSuggestion(int priceId, [FromBody] SendSuggestionCommand command)
        {
            command.PriceId = priceId;
            await _mediator.Send(command);

            return NoContent();
        }

        // api/suggestions/votes/1
        [HttpPost("votes/{suggestionId}")]
        [Authorize(Roles = "user, admin")]
        public async Task<ActionResult> SendSuggestionVote(int suggestionId, [FromBody] SendSuggestionVoteCommand command)
        {
            command.SuggestionId = suggestionId;
            await _mediator.Send(command);

            return NoContent();
        }

    }
}
