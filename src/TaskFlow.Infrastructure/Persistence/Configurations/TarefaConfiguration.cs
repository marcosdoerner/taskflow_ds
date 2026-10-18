using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Persistence.Configurations;

public class TarefaConfiguration : IEntityTypeConfiguration<Tarefa>
{
    public void Configure(EntityTypeBuilder<Tarefa> builder)
    {
        builder.ToTable("Tarefas", "taskflow");

        builder.HasKey(t => t.Id);

        // Titulo (Value Object -> coluna Titulo)
        builder.Property(t => t.Titulo)
            .HasConversion(
                titulo => titulo.Valor,
                valor => new TaskFlow.Domain.ValueObjects.TituloTarefa(valor))
            .HasColumnName("Titulo")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Descricao).HasMaxLength(1000);

        builder.Property(t => t.Status).IsRequired();
        builder.Property(t => t.Prioridade).IsRequired();
        builder.Property(t => t.CriadoEm).IsRequired();
        builder.Property(t => t.DataVencimento);
        builder.Property(t => t.ConcluidoEm);

        // Ignora a coleção de eventos (não persiste)
        builder.Ignore(t => t.Eventos);

        builder.HasOne(t => t.Usuario)
            .WithMany(u => u.Tarefas)
            .HasForeignKey(t => t.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(t => new { t.UsuarioId, t.Status });
    }
}
