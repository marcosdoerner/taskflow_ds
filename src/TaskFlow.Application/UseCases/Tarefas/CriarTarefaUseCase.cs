using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Abstractions;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.ValueObjects;

namespace TaskFlow.Application.UseCases.Tarefas;

public class CriarTarefaUseCase
{
    private readonly ITarefaRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CriarTarefaUseCase(ITarefaRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TarefaResponse> ExecutarAsync(TarefaRequest request, int usuarioId, CancellationToken ct = default)
    {
        var titulo = new TituloTarefa(request.Titulo);

        var tarefa = new Tarefa(titulo, usuarioId, request.Prioridade, request.DataVencimento);

        if (!string.IsNullOrWhiteSpace(request.Descricao))
            tarefa.Atualizar(titulo, request.Descricao, request.Prioridade, request.DataVencimento);

        await _repository.AdicionarAsync(tarefa, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return TarefaResponse.FromEntity(tarefa);
    }

}
