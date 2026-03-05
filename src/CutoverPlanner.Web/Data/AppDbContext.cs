using CutoverPlanner.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CutoverPlanner.Web.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Atividade> Atividades => Set<Atividade>();
        public DbSet<AtividadeDependencia> AtividadeDependencias => Set<AtividadeDependencia>();
        public DbSet<AreaExecutora> AreasExecutoras => Set<AreaExecutora>();
        public DbSet<Models.Endpoint> Endpoints => Set<Models.Endpoint>();
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AtividadeDependencia>()
                .HasOne(d => d.Atividade)
                .WithMany(a => a.Predecessoras)
                .HasForeignKey(d => d.AtividadeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AtividadeDependencia>()
                .HasOne(d => d.Predecessora)
                .WithMany(a => a.Sucessoras)
                .HasForeignKey(d => d.PredecessoraId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
