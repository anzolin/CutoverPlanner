using CutoverPlanner.Domain.Constants;
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
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            #region Áreas

            var areas = new List<Area>
            {
                new Area()
                {
                    Nome = GenericoConstants.AreaNA,
                    NomeResponsavel = "-",
                    EmailResponsavel = ""
                },
                new Area()
                {
                    Nome = GenericoConstants.Generica,
                    NomeResponsavel = "-",
                    EmailResponsavel = ""
                },
                new Area()
                {
                    Nome = GenericoConstants.AreaGeadOperacaoDespacho,
                    NomeResponsavel = "Ana Cristina Barbosa Faria",
                    EmailResponsavel = ""
                },
                new Area()
                {
                    Nome = GenericoConstants.AreaGeadCanaisConvencionais,
                    NomeResponsavel = "Valeria Martelloti da Silva",
                    EmailResponsavel = ""
                },
                new Area()
                {
                    Nome = GenericoConstants.AreaGeadCanaisDigitais,
                    NomeResponsavel = "Marco Aurelio Vilela Sousa",
                    EmailResponsavel = ""
                },
                new Area()
                {
                    Nome = GenericoConstants.AreaGeadCobranca,
                    NomeResponsavel = "Thiago Rodrigues e Rodrigues",
                    EmailResponsavel = ""
                },
                new Area()
                {
                    Nome = GenericoConstants.AreaGeadOperacaoManutencao,
                    NomeResponsavel = "Otavio de Barros Freitas",
                    EmailResponsavel = ""
                },
                new Area()
                {
                    Nome = GenericoConstants.AreaGeadFaturamento,
                    NomeResponsavel = "Eder Marcos da Silva",
                    EmailResponsavel = ""
                },
                new Area()
                {
                    Nome = GenericoConstants.AreaInfraBancoDados,
                    NomeResponsavel = "Daniel Eduardo Garrido Barzellay",
                    EmailResponsavel = ""
                },
                new Area()
                {
                    Nome = GenericoConstants.AreaInfraAplicacoes,
                    NomeResponsavel = "Andrea Cristina Bittencourt Moraes",
                    EmailResponsavel = ""
                },
                new Area()
                {
                    Nome = GenericoConstants.AreaInfraDatacenterECloud,
                    NomeResponsavel = "Valeria Ferreira Victor",
                    EmailResponsavel = ""
                },
                new Area()
                {
                    Nome = GenericoConstants.AreaProjeto,
                    NomeResponsavel = "Erica de Andrade e Santos",
                    EmailResponsavel = ""
                },
                new Area()
                {
                    Nome = GenericoConstants.AreaUsuario,
                    NomeResponsavel = "Jean Carlo Ranucci do Amaral",
                    EmailResponsavel = ""
                }
            };

            foreach (var area in areas)
            {
                if (!this.Areas.Any(q => q.Nome.Equals(area.Nome)))
                    this.Add(area);

                this.SaveChanges();
            }

            #endregion

            #region Sistemas

            var sistemas = new List<Sistema>
            {
                new Sistema()
                {
                    Nome = GenericoConstants.SistemaNA,
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaNA).Id
                },
                new Sistema()
                {
                    Nome = GenericoConstants.Generico,
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.Generica).Id
                },
                new Sistema()
                {
                    Nome = "SICCO",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaGeadFaturamento).Id
                },
                new Sistema()
                {
                    Nome = "SICNT",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaGeadOperacaoDespacho).Id
                },
                new Sistema()
                {
                    Nome = "SIATT",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaGeadCanaisConvencionais).Id
                },
                new Sistema()
                {
                    Nome = "EON",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaGeadCanaisDigitais).Id
                },
                new Sistema()
                {
                    Nome = "GISA",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaGeadCanaisDigitais).Id
                },
                new Sistema()
                {
                    Nome = "NRSS",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaGeadCanaisDigitais).Id
                },
            };

            foreach (var sistema in sistemas)
            {
                if (!this.Sistemas.Any(q => q.Nome.Equals(sistema.Nome)))
                    this.Add(sistema);

                this.SaveChanges();
            }

            #endregion

            #region Executores

            var executores = new List<Executor>
            {
                new Executor()
                {
                    Nome = GenericoConstants.Generico,
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.Generica).Id
                },
                new Executor()
                {
                    Nome = "Wagner Bianchini Narde",
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaGeadOperacaoDespacho).Id
                },
                new Executor()
                {
                    Nome = "Daniel Felipe Borba",
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaGeadOperacaoDespacho).Id
                },
                new Executor()
                {
                    Nome = "Douglas Torres Cravo Borges",
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaGeadOperacaoDespacho).Id
                },
                new Executor()
                {
                    Nome = "Diego Anzolin Ferreira",
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaGeadOperacaoDespacho).Id
                },
                new Executor()
                {
                    Nome = "Bruno Barroso Miranda",
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaGeadOperacaoDespacho).Id
                },
                new Executor()
                {
                    Nome = "Ana Cristina Barbosa Faria",
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaGeadOperacaoDespacho).Id
                },
                new Executor()
                {
                    Nome = "Victor Teixeira Pinheiro",
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaGeadCanaisConvencionais).Id
                },
                new Executor()
                {
                    Nome = "Valeria Martelloti da Silva",
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaGeadCanaisConvencionais).Id
                },
                new Executor()
                {
                    Nome = "Lucas de Almeida Teixeira",
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaGeadCanaisDigitais).Id
                },
                new Executor()
                {
                    Nome = "Marco Aurelio Vilela Sousa",
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaGeadCanaisDigitais).Id
                },
                new Executor()
                {
                    Nome = "Diogo de Souza Miranda",
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaGeadCobranca).Id
                },
                new Executor()
                {
                    Nome = "Thiago Rodrigues e Rodrigues",
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaGeadCobranca).Id
                },
                new Executor()
                {
                    Nome = "Anderson Luiz Pereira Dias",
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaGeadFaturamento).Id
                },
                new Executor()
                {
                    Nome = "Eder Marcos da Silva",
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaGeadFaturamento).Id
                },
                new Executor()
                {
                    Nome = "Marcus Vinicius Alves de Castro",
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaGeadOperacaoManutencao).Id
                },
                new Executor()
                {
                    Nome = "Otavio de Barros Freitas",
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaGeadOperacaoManutencao).Id
                },
                new Executor()
                {
                    Nome = "Andrea Cristina Bittencourt Moraes",
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaInfraAplicacoes).Id
                },
                new Executor()
                {
                    Nome = "Joao Luiz de Souza Torres",
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaInfraAplicacoes).Id
                },
                new Executor()
                {
                    Nome = "Ghullite Tacone Bento",
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaInfraAplicacoes).Id
                },
                new Executor()
                {
                    Nome = "Valeria Ferreira Victor",
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaInfraDatacenterECloud).Id
                },
                new Executor()
                {
                    Nome = "Carlos Roberto Pereira de Aquino",
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaInfraDatacenterECloud).Id
                },
                new Executor()
                {
                    Nome = "Carlos Andre Souza",
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaInfraDatacenterECloud).Id
                },
                new Executor()
                {
                    Nome = "Rebeca Vitoria Costa Souza",
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaInfraDatacenterECloud).Id
                },
                new Executor()
                {
                    Nome = "Erica de Andrade e Santos",
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaProjeto).Id
                },
                new Executor()
                {
                    Nome = "Bruno Miranda Couto",
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaProjeto).Id
                },
                new Executor()
                {
                    Nome = "Livia Mariquito Montes",
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaProjeto).Id
                },
                new Executor()
                {
                    Nome = "Joaquim Camerino Moraes de Souza",
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaProjeto).Id
                },
                new Executor()
                {
                    Nome = "Jean Carlo Ranucci do Amaral",
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaUsuario).Id
                },
                new Executor()
                {
                    Nome = "Sharles Mendes Rodrigues",
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaUsuario).Id
                },
                new Executor()
                {
                    Nome = "Mauro Souza Carvalho Junior",
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaUsuario).Id
                },
                new Executor()
                {
                    Nome = "Rinaldo Antonio Dias Pereira",
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaUsuario).Id
                },
                new Executor()
                {
                    Nome = "Daniel Eduardo Garrido Barzellay",
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaInfraBancoDados).Id
                },
                new Executor()
                {
                    Nome = "Andre Domingues dos Santos Lomba",
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaInfraBancoDados).Id
                }
                ,
                new Executor()
                {
                    Nome = "Eduardo Celso de Paula",
                    Email = "",
                    IdArea = this.Areas.First(q => q.Nome == GenericoConstants.AreaInfraBancoDados).Id
                }
            };

            foreach (var executor in executores)
            {
                if (!this.Executores.Any(q => q.Nome.Equals(executor.Nome)))
                    this.Add(executor);

                this.SaveChanges();
            }

            #endregion
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
