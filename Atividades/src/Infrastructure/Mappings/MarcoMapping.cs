using CutoverManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CutoverManager.Infrastructure.Mappings;

public class MarcoMapping : IEntityTypeConfiguration<Marco>
{
    public void Configure(EntityTypeBuilder<Marco> builder)
    {
        builder.ToTable("Marco");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
               .ValueGeneratedOnAdd();

        builder.Property(m => m.Nome)
               .HasMaxLength(250)
               .IsRequired();
    }
}