using FluentAssertions;
using NSubstitute;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.UseCases.Auth;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Exceptions;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Services;
using TaskFlow.Domain.ValueObjects;
using TaskFlow.Domain.Abstractions;

public class RegistrarUsuarioUseCaseTests
{
    private readonly IUsuarioRepository _usuarioRepo = Substitute.For<IUsuarioRepository>();
    private readonly IPasswordHasher _hasher = Substitute.For<IPasswordHasher>();
    private readonly ITokenService _tokenService = Substitute.For<ITokenService>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly RegistrarUsuarioUseCase _useCase;

    public RegistrarUsuarioUseCaseTests()
    {
        _useCase = new RegistrarUsuarioUseCase(_usuarioRepo, _hasher, _tokenService, _uow);
    }

    [Fact]
    public async Task Executar_ComEmailNovo_DeveRegistrarEGerarToken()
    {
        // Arrange
        var request = new RegistroRequest("Marcos", "marcos@teste.com", "senha123");
        _usuarioRepo.ExisteEmailAsync(Arg.Any<Email>(), Arg.Any<CancellationToken>())
            .Returns(false);
        _hasher.Hash("senha123").Returns("hash_gerado");
        _tokenService.GerarToken(Arg.Any<Usuario>()).Returns("token_jwt");

        // Act
        var result = await _useCase.ExecutarAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Token.Should().Be("token_jwt");
        result.Email.Should().Be("marcos@teste.com");

        await _usuarioRepo.Received(1).AdicionarAsync(
            Arg.Is<Usuario>(u => u.Nome == "Marcos" && u.Email.Valor == "marcos@teste.com"),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Executar_ComEmailJaCadastrado_DeveLancarDomainException()
    {
        // Arrange
        var request = new RegistroRequest("Marcos", "marcos@teste.com", "senha123");
        _usuarioRepo.ExisteEmailAsync(Arg.Any<Email>(), Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        var act = () => _useCase.ExecutarAsync(request);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("*já cadastrado*");
        await _usuarioRepo.DidNotReceive().AdicionarAsync(
            Arg.Any<Usuario>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Executar_ComSenhaFraca_DeveLancarDomainException()
    {
        // Arrange - se o seu domínio valida senha, teste aqui
        // (aqui assumindo que a validação está no hasher ou no domínio)
        var request = new RegistroRequest("Marcos", "marcos@teste.com", "");
        _usuarioRepo.ExisteEmailAsync(Arg.Any<Email>(), Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var act = () => _useCase.ExecutarAsync(request);

        // Assert
        await act.Should().ThrowAsync<DomainException>();
    }
}