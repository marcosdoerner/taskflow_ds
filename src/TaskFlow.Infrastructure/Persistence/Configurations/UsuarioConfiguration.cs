using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Persistence.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios", "taskflow");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Nome).IsRequired().HasMaxLength(100);

        // Email (Value Object -> coluna Email)
        builder.Property(u => u.Email)
            .HasConversion(
                e => e.Valor,
                valor => new TaskFlow.Domain.ValueObjects.Email(valor))
            .HasColumnName("Email")
            .IsRequired()
            .HasMaxLength(256);

        builder.HasIndex(u => u.Email).IsUnique();

        // SenhaHash (Value Object => coluna SenhaHash)
        builder.Property(u => u.SenhaHash)
            .HasConversion(
                s => s.Valor,
                valor => new TaskFlow.Domain.ValueObjects.SenhaHash(valor))
            .HasColumnName("SenhaHash")
            .IsRequired();

        builder.Property(u => u.CriadoEm).IsRequired();

        builder.Metadata
            .FindNavigation(nameof(Usuario.Tarefas))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
