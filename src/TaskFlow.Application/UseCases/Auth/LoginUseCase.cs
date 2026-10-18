using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Services;
using TaskFlow.Domain.ValueObjects;

namespace TaskFlow.Application.UseCases.Auth;

public class LoginUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public LoginUseCase(
        IUsuarioRepository usuarioRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse?> ExecuteAsync(LoginRequest request, CancellationToken ct = default)
    {
        var email = new Email(request.Email);

        var usuario = await _usuarioRepository.ObterPorEmailAsync(email, ct);

        if (usuario == null) return null;

        if (!_passwordHasher.Verificar(request.Senha, usuario.SenhaHash.Valor))
            return null;

        var token = _tokenService.GerarToken(usuario);

        return new AuthResponse(token, usuario.Nome, usuario.Email.Valor);
    }
}
