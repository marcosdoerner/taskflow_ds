using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Domain.ValueObjects;

public sealed record SenhaHash
{
    public string Valor { get; }

    public SenhaHash(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new DomainException("Hash de senha é obrigatório.");

        Valor = valor;
    }

    public override string ToString() => "***";

    public static implicit operator string(SenhaHash hash) => hash.Valor;
}
