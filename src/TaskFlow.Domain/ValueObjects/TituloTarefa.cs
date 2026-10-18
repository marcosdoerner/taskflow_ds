using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Domain.ValueObjects;

public sealed record TituloTarefa
{
    public const int TamanhoMaximo = 200;

    public string Valor { get; }

    public TituloTarefa(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new DomainException("Titulo é obrigatório.");

        var normalizado = valor.Trim();

        if (normalizado.Length > TamanhoMaximo)
            throw new DomainException($"Título deve ter no máximo {TamanhoMaximo} caracteres.");

        Valor = normalizado;
    }

    public override string ToString() => Valor;

    public static implicit operator string(TituloTarefa titulo) => titulo.Valor;

}
