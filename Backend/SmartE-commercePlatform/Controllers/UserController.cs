using Application.DTOs.User;
using Application.UseCases.User.Commands.CreateUser;
using Application.UseCases.User.Commands.DeleteUser;
using Application.UseCases.User.Commands.UpdateUser;
using Application.UseCases.User.Queries.GetUserById;
using Application.UseCases.User.Queries.LoginUser;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartE_commercePlatform.Helpers;

namespace SmartE_commercePlatform.Controllers
{
    [Route("api/v1/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly TokenSerializer tokenSerializer;

        public UserController(IMediator mediator, TokenSerializer tokenSerializer)
        {
            this.mediator = mediator;
            this.tokenSerializer = tokenSerializer;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Guid>> CreateUser(CreateUserDTO user)
        {
            var userId = await mediator.Send(new CreateUserCommand(user));
            return Created(nameof(UserDTO), userId);
        }

        [HttpPost("login")]
        public async Task<ActionResult<Guid>> LoginUser(LoginUserDTO user)
        {
            var userResponse = await mediator.Send(new LoginUserQuery(user));

            Response.Headers.Append("Authorization", $"Bearer {tokenSerializer.CreateToken(userResponse)}");

            return Ok();
        }

        [HttpGet("checkToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult CheckToken()
        {
            return NoContent();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("details")]
        public async Task<ActionResult<UserDTO>> GetUserById()
        {
            Guid userId = new Guid(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value!);

            return await mediator.Send(new GetUserByIdQuery(userId));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("edit")]
        public async Task<ActionResult> UpdateUser(UserDTO user)
        {
            await mediator.Send(new UpdateUserCommand(user));
            return NoContent();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteUser()
        {
            Guid userId = new Guid(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value!);

            await mediator.Send(new DeleteUserCommand(userId));
            return NoContent();
        }

    }
}
