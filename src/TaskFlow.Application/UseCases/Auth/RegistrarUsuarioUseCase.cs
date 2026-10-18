using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Abstractions;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Exceptions;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Services;
using TaskFlow.Domain.ValueObjects;

namespace TaskFlow.Application.UseCases.Auth;

public class RegistrarUsuarioUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public RegistrarUsuarioUseCase(
        IUsuarioRepository usuarioRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IUnitOfWork unitOfWork)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthResponse> ExecutarAsync(RegistroRequest request, CancellationToken ct = default)
    {
        var email = new Email(request.Email);

        if (await _usuarioRepository.ExisteEmailAsync(email, ct))
        {
            throw new DomainException("Já existe um usuário com este e-mail.");
        }

        var senhaHash = new SenhaHash(_passwordHasher.Hash(request.Senha));

        var usuario = new Usuario(request.Nome, email, senhaHash);

        await _usuarioRepository.AdicionarAsync(usuario, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        var token = _tokenService.GerarToken(usuario);

        return new AuthResponse(token, usuario.Nome, usuario.Email.Valor);
    }
}
