using CutoverManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CutoverManager.Infrastructure.Mappings;

public class ExecutorMapping : IEntityTypeConfiguration<Executor>
{
    public void Configure(EntityTypeBuilder<Executor> builder)
    {
        builder.ToTable("Executor");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
               .ValueGeneratedOnAdd();

        builder.Property(e => e.Nome)
               .HasMaxLength(250)
               .IsRequired();

        builder.Property(e => e.Email).HasMaxLength(250);

        builder.HasOne(e => e.Area)
               .WithMany(a => a.Executores)
               .HasForeignKey(e => e.IdArea)
               .OnDelete(DeleteBehavior.Restrict);
    }
}