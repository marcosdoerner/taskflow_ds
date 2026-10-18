using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.ValueObjects;

namespace TaskFlow.Infrastructure.Persistence.Repositories;

public class UsuarioRepository : IUsuarioRepository
{

    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context) => _context = context;

    public async Task<Usuario?> ObterPorIdAsync(int id, CancellationToken ct = default)
        => await _context.Usuarios.FindAsync(new object[] { id }, ct);

    public async Task<Usuario?> ObterPorEmailAsync(Email email, CancellationToken ct = default)
        => await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email, ct);

    public async Task<bool> ExisteEmailAsync(Email email, CancellationToken ct = default)
        => await _context.Usuarios
            .AnyAsync(u => u.Email == email, ct);

    public async Task AdicionarAsync(Usuario usuario, CancellationToken ct = default)
    {
        await _context.Usuarios.AddAsync(usuario, ct);
    }

    public Task AtualizarAsync(Usuario usuario, CancellationToken ct = default)
    {
        _context.Usuarios.Update(usuario);
        return Task.CompletedTask;
    }

}
