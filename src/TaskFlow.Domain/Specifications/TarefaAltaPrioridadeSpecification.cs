using System.Linq.Expressions;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Specifications;

public class TarefaAltaPrioridadeSpecification : Specification<Tarefa>
{
    public override Expression<Func<Tarefa, bool>> ToExpression()
        => t => t.Prioridade == PrioridadeTarefa.Alta
             || t.Prioridade == PrioridadeTarefa.Urgente;
}
