using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Abstractions;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.ValueObjects;

namespace TaskFlow.Application.UseCases.Tarefas;

public class AtualizarTarefaUseCase
{
    private readonly ITarefaRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public AtualizarTarefaUseCase(ITarefaRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TarefaResponse?> ExecutarAsync(int id, TarefaRequest request, int usuarioId, CancellationToken ct = default)
    {
        var tarefa = await _repository.ObterPorIdAsync(id, usuarioId, ct);
        if (tarefa == null) return null;

        var titulo = new TituloTarefa(request.Titulo);

        tarefa.Atualizar(titulo, request.Descricao, request.Prioridade, request.DataVencimento);
        await _repository.AtualizarAsync(tarefa, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return TarefaResponse.FromEntity(tarefa);
    }
}
