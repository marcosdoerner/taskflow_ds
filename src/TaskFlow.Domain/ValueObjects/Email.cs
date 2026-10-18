using System.Text.RegularExpressions;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Domain.ValueObjects;

public sealed record Email
{
    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Valor { get; }

    public Email(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new DomainException("Email é obrigatório.");

        var normalizado = valor.Trim().ToLowerInvariant();

        if (normalizado.Length > 256)
            throw new DomainException("Email deve ter no máximo 256 caracteres.");

        if (!EmailRegex.IsMatch(normalizado))
            throw new DomainException($"Email inválido: {valor}");

        Valor = normalizado;
    }

    public override string ToString() => Valor;

    public static implicit operator string(Email email) => email.Valor;

}
