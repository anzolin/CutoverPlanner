using CutoverManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CutoverManager.Infrastructure.Mappings;

public class AreaMapping : IEntityTypeConfiguration<Area>
{
    public void Configure(EntityTypeBuilder<Area> builder)
    {
        builder.ToTable("Area");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
               .ValueGeneratedOnAdd();

        builder.Property(a => a.Nome).HasMaxLength(250).IsRequired();
        builder.Property(a => a.NomeResponsavel).HasMaxLength(250);
        builder.Property(a => a.EmailResponsavel).HasMaxLength(250);
    }
}