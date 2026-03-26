using CutoverManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CutoverManager.Infrastructure.Mappings;

public class SistemaMapping : IEntityTypeConfiguration<Sistema>
{
    public void Configure(EntityTypeBuilder<Sistema> builder)
    {
        builder.ToTable("Sistema");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
               .ValueGeneratedOnAdd();

        builder.Property(s => s.Nome)
               .HasMaxLength(250)
               .IsRequired();

        builder.HasOne(s => s.Area)
               .WithMany(a => a.Sistemas)
               .HasForeignKey(s => s.IdArea)
               .OnDelete(DeleteBehavior.Restrict);
    }
}