using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Specifications;

namespace TaskFlow.Infrastructure.Persistence.Repositories;

public class TarefaRepository : ITarefaRepository
{
    private readonly AppDbContext _context;

    public TarefaRepository(AppDbContext context) => _context = context;

    public async Task<Tarefa?> ObterPorIdAsync(int id, int usuarioId, CancellationToken ct = default)
        => await _context.Tarefas
            .FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == usuarioId, ct);

    public async Task<IReadOnlyList<Tarefa>> ListarPorUsuarioAsync(int usuarioId, CancellationToken ct = default)
        => await _context.Tarefas
            .Where(t => t.UsuarioId == usuarioId)
            .OrderByDescending(t => t.CriadoEm)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<Tarefa>> ListarPorUsuarioComSpecificationAsync(
        int usuarioId, Specification<Tarefa> spec, CancellationToken ct = default)
        => await _context.Tarefas
            .Where(t => t.UsuarioId == usuarioId)
            .Where(spec.ToExpression())
            .OrderByDescending(t => t.CriadoEm)
            .ToListAsync(ct);

    public async Task AdicionarAsync(Tarefa tarefa, CancellationToken ct = default)
    {
        await _context.Tarefas.AddAsync(tarefa, ct);
    }

    public Task AtualizarAsync(Tarefa tarefa, CancellationToken ct = default)
    {
        _context.Tarefas.Update(tarefa);
        return Task.CompletedTask;
    }

    public Task RemoverAsync(Tarefa tarefa, CancellationToken ct = default)
    {
        _context.Tarefas.Remove(tarefa);
        return Task.CompletedTask;
    }
}
