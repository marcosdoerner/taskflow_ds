using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Repositories;

namespace TaskFlow.Application.UseCases.Tarefas;

public class ObterTarefaUseCase
{
    private readonly ITarefaRepository _repository;

    public ObterTarefaUseCase(ITarefaRepository repository)
        => _repository = repository;

    public async Task<TarefaResponse?> ExecutarAsync(int id, int usuarioId, CancellationToken ct = default)
    {
        var tarefa = await _repository.ObterPorIdAsync(id, usuarioId, ct);
        return tarefa == null ? null : TarefaResponse.FromEntity(tarefa);
    }

}
