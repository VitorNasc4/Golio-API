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
    [Route("api/stores")]
    [Authorize]
    public class StoresController : ControllerBase
    {
        private readonly IMediator _mediator;
        public StoresController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // api/stores/1
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStoreById(int id)
        {
            var getStoreById = new GetStoreByIdQuery(id);
            var store = await _mediator.Send(getStoreById);

            if (store == null)
            {
                return NotFound();
            }

            return Ok(store);
        }

        // api/stores/query=cerveja
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetStoresByQuery(string query)
        {
            var getAllStores = new GetStoresByQuery(query);
            var stores = await _mediator.Send(getAllStores);

            if (stores == null)
            {
                return NotFound();
            }

            return Ok(stores);
        }


        // api/stores
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PostStore([FromBody] CreateStoreCommand command)
        {
            var id = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetStoreById), new { id = id });
        }

        // api/suggestions/1
        [HttpPut("{storeId}")]
        [Authorize(Roles = "user, admin")]
        public async Task<ActionResult> UpdateStore(int storeId, [FromBody] UpdateStoreCommand command)
        {
            command.StoreId = storeId;
            await _mediator.Send(command);

            return NoContent();
        }

    }
}
