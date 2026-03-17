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

        protected AppDbContext()
        {
            var areas = new List<Area>
            {
                new Area()
                {
                    Nome = "Genérica",
                    NomeResponsavel = "",
                    EmailResponsavel = ""
                },
                new Area()
                {
                    Nome = "GEAD/OPERAÇÃO DESPACHO",
                    NomeResponsavel = "Ana Cristina Barbosa Faria",
                    EmailResponsavel = ""
                },
                new Area()
                {
                    Nome = "GEAD/CANAIS CONVENCIONAIS",
                    NomeResponsavel = "Valeria Martelloti da Silva",
                    EmailResponsavel = ""
                },
                new Area()
                {
                    Nome = "GEAD/CANAIS DIGITAIS",
                    NomeResponsavel = "Marco Aurelio Vilela Sousa",
                    EmailResponsavel = ""
                },
                new Area()
                {
                    Nome = "GEAD/COBRANÇA",
                    NomeResponsavel = "Thiago Rodrigues e Rodrigues",
                    EmailResponsavel = ""
                },
                new Area()
                {
                    Nome = "GEAD/OPERAÇÃO MANUTENÇÃO",
                    NomeResponsavel = "Otavio de Barros Freitas",
                    EmailResponsavel = ""
                },
                new Area()
                {
                    Nome = "GEAD/COBRANÇA",
                    NomeResponsavel = "Eder Marcos da Silva",
                    EmailResponsavel = ""
                }
            };

            foreach (var area in areas)
            {
                if (!this.Areas.Any(q => q.Nome.Equals(area.Nome)))
                    this.Add(area);

                this.SaveChanges();
            }
        }

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
