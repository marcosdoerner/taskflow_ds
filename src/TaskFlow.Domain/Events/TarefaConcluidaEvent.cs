namespace TaskFlow.Domain.Events;

public record TarefaConcluidaEvent(
    int TarefaId,
    int UsuarioId,
    DateTime OcorridoEm) : IDomainEvent;

