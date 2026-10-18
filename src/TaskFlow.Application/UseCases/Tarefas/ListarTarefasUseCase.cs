using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Repositories;

namespace TaskFlow.Application.UseCases.Tarefas;

public class ListarTarefasUseCase
{
    private readonly ITarefaRepository _repository;

    public ListarTarefasUseCase(ITarefaRepository repository) => _repository = repository;

    public async Task<IReadOnlyList<TarefaResponse>> ExecutarAsync(int usuarioId, CancellationToken ct = default)
    {
        var tarefas = await _repository.ListarPorUsuarioAsync(usuarioId, ct);
        return tarefas.Select(TarefaResponse.FromEntity).ToList();
    }
}
