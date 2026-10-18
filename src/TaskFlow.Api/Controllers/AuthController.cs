using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.UseCases.Auth;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{

    private readonly RegistrarUsuarioUseCase _registrar;
    private readonly LoginUseCase _login;

    public AuthController(RegistrarUsuarioUseCase registrar, LoginUseCase login)
    {
        _registrar = registrar;
        _login = login;
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar([FromBody] RegistroRequest request, CancellationToken ct)
        => Ok(await _registrar.ExecutarAsync(request, ct));

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var result = await _login.ExecuteAsync(request, ct);

        return result == null
        ? Unauthorized(new { mensagem = "Email ou senha inválidos." })
        : Ok(result);
    }

}
