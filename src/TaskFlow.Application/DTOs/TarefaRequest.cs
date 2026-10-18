using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.DTOs;

public record TarefaRequest(
    string Titulo,
    string? Descricao,
    PrioridadeTarefa Prioridade,
    DateTime? DataVencimento
);
