using TaskFlow.Domain.Exceptions;
using TaskFlow.Domain.ValueObjects;

namespace TaskFlow.Domain.Entities;

public class Usuario
{
    public int Id { get; private set; }
    public string Nome { get; private set; }
    public Email Email { get; private set; }
    public SenhaHash SenhaHash { get; private set; }
    public DateTime CriadoEm { get; private set; }

    private readonly List<Tarefa> _tarefas = new();
    public IReadOnlyCollection<Tarefa> Tarefas => _tarefas.AsReadOnly();

    // EF Core 
    private Usuario()
    {
        Nome = string.Empty;
        Email = null!;
        SenhaHash = null!;
    }

    public Usuario(string nome, Email email, SenhaHash senhaHash)
    {
        ValidarNome(nome);

        Nome = nome.Trim();
        Email = email ?? throw new DomainException("Email é obrigatório.");
        SenhaHash = senhaHash ?? throw new DomainException("Hash de senha é obrigatório.");
        CriadoEm = DateTime.UtcNow;
    }

    public void AtualizarNome(string novoNome)
    {
        ValidarNome(novoNome);
        Nome = novoNome.Trim();
    }

    public void AlterarEmail(Email novoEmail)
    {
        Email = novoEmail ?? throw new DomainException("Email é obrigatório.");
    }

    public void AlterarSenha(SenhaHash novoHash)
    {
        SenhaHash = novoHash ?? throw new DomainException("Has de senha é obrigatório.");
    }

    private void ValidarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("O nome do usuário é obrigatório.");

        if (nome.Trim().Length < 2)
            throw new DomainException("Nome deve ter pelo menos 2 caracteres.");

        if (nome.Length > 100)
            throw new DomainException("O nome do usuário deve ter no máximo 100 caracteres.");
    }

}
