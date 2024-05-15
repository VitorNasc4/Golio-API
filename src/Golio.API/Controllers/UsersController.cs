using Golio.API.Models;
using Golio.Application.Commands.LoginUser;
using Golio.Application.Commands.UserCommands.CreateUser;
using Golio.Application.Queries.GetUser;
using Golio.Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Golio.API.Controllers
{
    [Route("api/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // api/users/1
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserById(int id)
        {
            var getUserQuery = new GetUserQuery(id);
            var user = await _mediator.Send(getUserQuery);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // api/users
        [HttpPost]
        [AllowAnonymous]
        public IActionResult PostUser([FromBody] CreateUserCommand command)
        {
            var id = _mediator.Send(command);

            var responseBody = new UserViewModel(command.FullName, command.Email);

            return CreatedAtAction(nameof(GetUserById), new { id = id }, responseBody);
        }

        // api/users/1/login
        [HttpPut("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] LoginUserCommand command)
        {
            var loginUserViewModel = await _mediator.Send(command);

            if (loginUserViewModel is null)
            {
                return BadRequest();
            }

            return Ok(loginUserViewModel);
        }
    }
}
