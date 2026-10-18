using TaskFlow.Domain.Enums;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.DTOs;

public record TarefaResponse(
    int Id,
    string Titulo,
    string? Descricao,
    StatusTarefa Status,
    PrioridadeTarefa Prioridade,
    DateTime? DataVencimento,
    bool EstaAtrasada,
    DateTime CriadoEm,
    DateTime? ConcluidoEm)
{
    public static TarefaResponse FromEntity(Tarefa t) => new(
        t.Id,
        t.Titulo.Valor,
        t.Descricao,
        t.Status,
        t.Prioridade,
        t.DataVencimento,
        t.EstaAtrasada(),
        t.CriadoEm,
        t.ConcluidoEm);
}

