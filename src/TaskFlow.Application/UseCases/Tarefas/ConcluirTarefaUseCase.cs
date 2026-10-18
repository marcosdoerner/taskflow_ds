using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Abstractions;
using TaskFlow.Domain.Repositories;

namespace TaskFlow.Application.UseCases.Tarefas;

public class ConcluirTarefaUseCase
{
    private readonly ITarefaRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ConcluirTarefaUseCase(ITarefaRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TarefaResponse?> ExecutarAsync(int id, int usuarioId, CancellationToken ct = default)
    {
        var tarefa = await _repository.ObterPorIdAsync(id, usuarioId, ct);
        if (tarefa == null) return null;

        tarefa.Concluir(); // regra de negócio na entidade
        await _repository.AtualizarAsync(tarefa, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return TarefaResponse.FromEntity(tarefa);
    }
}
