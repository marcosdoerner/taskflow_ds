using System.Linq.Expressions;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Specifications;

public class TarefaAtrasadaSpecification : Specification<Tarefa>
{
    public override Expression<Func<Tarefa, bool>> ToExpression()
        => t => t.DataVencimento != null
             && t.DataVencimento < DateTime.UtcNow
             && t.Status != StatusTarefa.Concluida
             && t.Status != StatusTarefa.Cancelada;
}
