using CutoverManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CutoverManager.Infrastructure.Mappings;

public class AtividadeMapping : IEntityTypeConfiguration<Atividade>
{
    public void Configure(EntityTypeBuilder<Atividade> builder)
    {
        builder.ToTable("Atividade");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
               .ValueGeneratedOnAdd();

        builder.Property(a => a.Titulo).HasMaxLength(500).IsRequired();
        builder.Property(a => a.Observacao);

        builder.Property(a => a.RiscoGoLive).IsRequired();
        builder.Property(a => a.Inicio).IsRequired();
        builder.Property(a => a.Termino).IsRequired();

        builder.HasOne(a => a.Plano)
               .WithMany(p => p.Atividades)
               .HasForeignKey(a => a.IdPlano)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Sistema)
               .WithMany(s => s.Atividades)
               .HasForeignKey(a => a.IdSistema)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Marco)
               .WithMany(m => m.Atividades)
               .HasForeignKey(a => a.IdMarco)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Executor)
               .WithMany(e => e.Atividades)
               .HasForeignKey(a => a.IdExecutor)
               .OnDelete(DeleteBehavior.Restrict);
    }
}