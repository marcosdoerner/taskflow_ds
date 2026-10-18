using TaskFlow.Domain.Abstractions;
using TaskFlow.Domain.Repositories;

namespace TaskFlow.Application.UseCases.Tarefas;

public class DeletarTarefaUseCase
{
    private readonly ITarefaRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeletarTarefaUseCase(ITarefaRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> ExecutarAsync(int id, int usuarioId, CancellationToken ct = default)
    {
        var tarefa = await _repository.ObterPorIdAsync(id, usuarioId, ct);
        if (tarefa == null) return false;

        await _repository.RemoverAsync(tarefa, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return true;
    }

}
