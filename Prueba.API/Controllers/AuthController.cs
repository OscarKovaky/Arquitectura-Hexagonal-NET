using MediatR;
using Microsoft.AspNetCore.Mvc;
using Prueba.Application.Commands;
using Prueba.Application.Queries;
using Prueba.Core.Dtos;

namespace Prueba.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController: ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto userDto)
    {
        var command = new RegisterUserCommand { UserDto = userDto };
        var userId = await _mediator.Send(command);
        return CreatedAtAction(nameof(Register), new { id = userId }, null);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto userDto)
    {
        var command = new LoginUserCommand { UserDto = userDto };
        var user = await _mediator.Send(command);
        if (user == null)
        {
            return Unauthorized(new ProblemDetails
            {
                Title = "Invalid credentials",
                Status = StatusCodes.Status401Unauthorized,
                Detail = "The username or password is incorrect."
            });
        }

        // Devolver la API Key al cliente
        return Ok(new { ApiKey = "test" });
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _mediator.Send(new GetUsersQuery());
        return Ok(users);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNode(int id)
    {
        // Crear y enviar el comando de eliminación
        var command = new DeleteUserCommand(id);
        await _mediator.Send(command);

        // Retornar NoContent para indicar que la operación fue exitosa
        return NoContent();
    }
    
    [HttpPut("update-username")]
    public async Task<IActionResult> UpdateUsername([FromBody] UpdateUsernameDto dto)
    {

        var command = new UpdateUsernameCommand
        {
            Id = dto.Id,
            Username = dto.Username
        };

        await _mediator.Send(command);

        return NoContent();
    }
}