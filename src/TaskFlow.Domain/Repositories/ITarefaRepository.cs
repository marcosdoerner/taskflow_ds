using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Specifications;

namespace TaskFlow.Domain.Repositories;

public interface ITarefaRepository
{
    Task<Tarefa?> ObterPorIdAsync(int id, int usuarioId, CancellationToken ct = default);
    Task<IReadOnlyList<Tarefa>> ListarPorUsuarioAsync(int usuarioId, CancellationToken ct = default);
    Task<IReadOnlyList<Tarefa>> ListarPorUsuarioComSpecificationAsync(int usuarioId, Specification<Tarefa> spec, CancellationToken ct = default);
    Task AdicionarAsync(Tarefa tarefa, CancellationToken ct = default);
    Task AtualizarAsync(Tarefa tarefa, CancellationToken ct = default);
    Task RemoverAsync(Tarefa tarefa, CancellationToken ct = default);
}
