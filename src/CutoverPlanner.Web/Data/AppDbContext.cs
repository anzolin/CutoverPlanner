using CutoverPlanner.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CutoverPlanner.Web.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Area> Areas => Set<Area>();
        public DbSet<Atividade> Atividades => Set<Atividade>();
        public DbSet<Executor> Executores => Set<Executor>();
        public DbSet<Marco> Marcos => Set<Marco>();
        public DbSet<Sistema> Sistemas => Set<Sistema>();
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Atividade>()
                .HasOne(d => d.Sistema)
                .WithMany(a => a.Atividades)
                .HasForeignKey(d => d.IdSistema)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Atividade>()
                .HasOne(d => d.Executor)
                .WithMany(a => a.Atividades)
                .HasForeignKey(d => d.IdExecutor)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Atividade>()
                .HasOne(d => d.Marco)
                .WithMany(a => a.Atividades)
                .HasForeignKey(d => d.IdMarco)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
