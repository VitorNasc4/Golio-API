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
    [Route("api/products")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // api/products/1
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductById(int id)
        {
            var getProductById = new GetProductByIdQuery(id);
            var product = await _mediator.Send(getProductById);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // api/products?query=cerveja
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetProducts(string query)
        {
            var getProductsQuery = new GetProductsQuery(query);
            var products = await _mediator.Send(getProductsQuery);

            if (products is null)
            {
                return NotFound();
            }

            return Ok(products);
        }

        // api/products
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PostProduct([FromBody] CreateProductCommand command)
        {
            var id = await _mediator.Send(command);

            var responseBody = new ProductDetailsViewModel
            (
                command.Name,
                command.Brand,
                command.Volume,
                command.Prices.Select(price => new PriceDetailsViewModel() { Value = price.Value, StoreName = price.Store.Name }).ToList()
            );

            return CreatedAtAction(nameof(GetProductById), new { id = id }, responseBody);
        }

        // api/products/1
        [HttpPut("{productId}")]
        [Authorize(Roles = "user, admin")]
        public async Task<ActionResult> SendSuggestion(int productId, [FromBody] UpdateProductCommand command)
        {
            command.ProductId = productId;
            await _mediator.Send(command);

            return NoContent();
        }

    }
}
