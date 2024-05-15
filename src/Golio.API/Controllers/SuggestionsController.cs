using Golio.API.Models;
using Golio.Application.Commands.CreateProduct.CreateUser;
using Golio.Application.Commands.CreateProduct.SendSuggestion;
using Golio.Application.Commands.LoginUser;
using Golio.Application.Commands.UserCommands.CreateUser;
using Golio.Application.Queries.GetProductById;
using Golio.Application.Queries.GetProducts;
using Golio.Application.Queries.GetUser;
using Golio.Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
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

    }
}
