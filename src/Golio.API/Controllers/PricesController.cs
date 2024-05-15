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
    [Route("api/prices")]
    [Authorize]
    public class PricesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PricesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // api/prices/1
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPriceById(int id)
        {
            var getPriceById = new GetPriceByIdQuery(id);
            var price = await _mediator.Send(getPriceById);

            if (price == null)
            {
                return NotFound();
            }

            return Ok(price);
        }


        // api/prices
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PostPrice([FromBody] CreatePriceCommand command)
        {
            var id = await _mediator.Send(command);

            var responseBody = new PriceDetailsViewModel();

            return CreatedAtAction(nameof(GetPriceById), new { id = id }, responseBody);
        }

        // api/prices/1
        // [HttpPut("{priceId}")]
        // [Authorize(Roles = "user, admin")]
        // public async Task<ActionResult> SendSuggestion(int priceId, [FromBody] SendSuggestionCommand command)
        // {
        //     command.PriceId = priceId;
        //     await _mediator.Send(command);

        //     return NoContent();
        // }

    }
}
