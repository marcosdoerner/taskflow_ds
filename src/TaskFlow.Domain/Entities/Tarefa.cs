using TaskFlow.Domain.Enums;
using TaskFlow.Domain.Exceptions;
using TaskFlow.Domain.ValueObjects;
using TaskFlow.Domain.Events;

namespace TaskFlow.Domain.Entities;

public class Tarefa
{
    public int Id { get; private set; }
    public TituloTarefa Titulo { get; private set; }
    public string? Descricao { get; private set; }
    public StatusTarefa Status { get; private set; }
    public PrioridadeTarefa Prioridade { get; private set; }
    public DateTime? DataVencimento { get; private set; }
    public DateTime CriadoEm { get; private set; }
    public DateTime? ConcluidoEm { get; private set; }

    public int UsuarioId { get; private set; }
    public Usuario Usuario { get; private set; } = null!;

    private readonly List<IDomainEvent> _eventos = new();
    public IReadOnlyCollection<IDomainEvent> Eventos => _eventos.AsReadOnly();

    // EF Core
    private Tarefa()
    {
        Titulo = null!;
        Usuario = null!;
    }

    public Tarefa(TituloTarefa titulo,
                  int usuarioId,
                  PrioridadeTarefa prioridade = PrioridadeTarefa.Media,
                  DateTime? dataVencimento = null)
    {

        if (usuarioId <= 0)
            throw new DomainException("Usuário inválido.");

        if (dataVencimento.HasValue && dataVencimento.Value < DateTime.UtcNow.Date)
            throw new DomainException("Data de vencimento não pode estar no passado.");

        Titulo = titulo ?? throw new DomainException("Titulo é obrigatório.");
        UsuarioId = usuarioId;
        Prioridade = prioridade;
        DataVencimento = dataVencimento;
        Status = StatusTarefa.Pendente;
        CriadoEm = DateTime.UtcNow;
    }

    public void Atualizar(TituloTarefa titulo,
                          string? descricao,
                          PrioridadeTarefa prioridade,
                          DateTime? dataVencimento)
    {

        if (Status == StatusTarefa.Concluida)
            throw new DomainException("Não é possível editar uma tarefa concluída.");

        if (Status == StatusTarefa.Cancelada)
            throw new DomainException("Não é possível editar uma tarefa cancelada.");

        if (dataVencimento.HasValue && dataVencimento.Value < DateTime.UtcNow.Date)
            throw new DomainException("Data de vencimento não pode estar no passado.");

        Titulo = titulo ?? throw new DomainException("Titulo é obrigatório.");
        Descricao = descricao?.Trim();
        Prioridade = prioridade;
        DataVencimento = dataVencimento;
    }

    public void Iniciar()
    {
        if (Status != StatusTarefa.Pendente)
            throw new DomainException("Apenas tarefas pendentes podem ser iniciadas.");

        Status = StatusTarefa.EmAndamento;
    }

    public void Concluir()
    {
        if (Status == StatusTarefa.Concluida)
            throw new DomainException("Tarefa já está concluída.");

        if (Status == StatusTarefa.Cancelada)
            throw new DomainException("Não é possível concluir uma tarefa cancelada.");

        Status = StatusTarefa.Concluida;
        ConcluidoEm = DateTime.UtcNow;
        _eventos.Add(new TarefaConcluidaEvent(Id, UsuarioId, DateTime.UtcNow));
    }

    public void Reabrir()
    {
        if (Status != StatusTarefa.Concluida)
            throw new DomainException("Apenas tarefas concluídas podem ser reabertas.");

        Status = StatusTarefa.Pendente;
        ConcluidoEm = null;
    }

    public void Cancelar()
    {
        if (Status == StatusTarefa.Concluida)
            throw new DomainException("Não é possível cancelar uma tarefa concluída.");

        if (Status == StatusTarefa.Cancelada)
            throw new DomainException("Tarefa já está cancelada.");

        Status = StatusTarefa.Cancelada;
    }

    public void AlterarPrioridade(PrioridadeTarefa novaPrioridade)
    {
        if (Status == StatusTarefa.Concluida)
            throw new DomainException("Não é possível alterar prioridade de tarefa concluída.");

        if (Status == StatusTarefa.Cancelada)
            throw new DomainException("Não é possível alterar prioridade de tarefa cancelada.");

        Prioridade = novaPrioridade;

    }

    public bool EstaAtrasada()
        => DataVencimento.HasValue
            && DataVencimento.Value < DateTime.UtcNow
            && Status != StatusTarefa.Concluida
            && Status != StatusTarefa.Cancelada;

    public bool EstaAtiva()
        => Status == StatusTarefa.Pendente || Status == StatusTarefa.EmAndamento;

    public void LimparEventos() => _eventos.Clear();

}


