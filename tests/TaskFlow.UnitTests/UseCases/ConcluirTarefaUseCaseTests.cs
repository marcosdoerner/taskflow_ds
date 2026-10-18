using FluentAssertions;
using NSubstitute;
using TaskFlow.Application.UseCases.Tarefas;
using TaskFlow.Domain.Abstractions;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.ValueObjects;

namespace TaskFlow.UnitTests.UseCases;

public class ConcluirTarefaUseCaseTests
{
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly ITarefaRepository _repo = Substitute.For<ITarefaRepository>();
    private readonly ConcluirTarefaUseCase _useCase;

    public ConcluirTarefaUseCaseTests()
    {
        _useCase = new ConcluirTarefaUseCase(_repo, _uow);
    }

    [Fact]
    public async Task Executar_ComTarefaExistente_DeveConcluir()
    {
        // Arrange
        var tarefa = new Tarefa(new TituloTarefa("Estudar"), 1);
        _repo.ObterPorIdAsync(1, 1).Returns(tarefa);

        // Act
        var result = await _useCase.ExecutarAsync(1, 1);

        // Assert
        result.Should().NotBeNull();
        result!.Status.Should().Be(StatusTarefa.Concluida);
        await _repo.Received(1).AtualizarAsync(tarefa);
    }

    [Fact]
    public async Task Executar_ComTarefaInexistente_DeveRetornarNull()
    {
        // Arrange
        _repo.ObterPorIdAsync(99, 1).Returns((Tarefa?)null);

        // Act
        var result = await _useCase.ExecutarAsync(99, 1);

        // Assert
        result.Should().BeNull();

    }




}
