using FluentAssertions;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;
using TaskFlow.Domain.Exceptions;
using TaskFlow.Domain.ValueObjects;

namespace TaskFlow.UnitTests.Domain;

public class TarefaTests
{
    [Fact]
    public void Criar_ComTituloVazio_DeveLancarExcecao()
    {
        var act = () => new Tarefa(new TituloTarefa(""), 1);
        act.Should().Throw<DomainException>()
            .WithMessage("*título*");
    }

    [Fact]
    public void Criar_ComDadosValidos_DeveIniciarComPendente()
    {
        var tarefa = new Tarefa(new TituloTarefa("Estudar Clean Arquitecture"), 1);

        tarefa.Status.Should().Be(StatusTarefa.Pendente);
        tarefa.Titulo.Should().Be("Estudar Clean Arquitecture");
    }

    [Fact]
    public void Concluir_TarefaJaConcluida_DeveLancarExcecao()
    {
        var tarefa = new Tarefa(new TituloTarefa("Teste"), 1);
        tarefa.Concluir();

        var act = () => tarefa.Concluir();
        act.Should().Throw<DomainException>().WithMessage("*Já está concluída*");
    }

    [Fact]
    public void EstaAtrasada_ComDataPassaga_DeveRetornarTrue()
    {
        var tarefa = new Tarefa(new TituloTarefa("Atrasada"), 1, dataVencimento: DateTime.UtcNow.AddDays(-1));
        tarefa.EstaAtrasada().Should().BeTrue();
    }
}
