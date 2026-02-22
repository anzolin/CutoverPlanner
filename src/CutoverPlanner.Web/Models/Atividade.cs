
using System.ComponentModel.DataAnnotations;

namespace CutoverPlanner.Web.Models
{
    public class Atividade
    {
        [Key]
        public int Id { get; set; }
        public int? IdPlanilha { get; set; }
        public string? Sistema { get; set; }
        public string? Milestone { get; set; }
        [Required]
        public string Titulo { get; set; } = string.Empty;
        public string? Categoria { get; set; }
        public bool RequerTestePerformance { get; set; }
        public string? TipoTeste { get; set; }
        public string? Procedimento { get; set; }
        public string? Metricas { get; set; }
        public string? CriterioAceite { get; set; }
        public string? Evidencias { get; set; }
        public string? Responsavel { get; set; }
        public string? AreaExecutoraNome { get; set; }
        public string? Executor { get; set; }
        public StatusAtividade Status { get; set; }
        public bool? RiscoGoLive { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string? Observacao { get; set; }
        public string? LinkRepositorio { get; set; }
        public string? PredecessorasRaw { get; set; }
        public ICollection<AtividadeDependencia> Predecessoras { get; set; } = new List<AtividadeDependencia>();
        public ICollection<AtividadeDependencia> Sucessoras { get; set; } = new List<AtividadeDependencia>();
    }

    public class AtividadeDependencia
    {
        public int Id { get; set; }
        public int AtividadeId { get; set; }
        public Atividade Atividade { get; set; } = null!;
        public int PredecessoraId { get; set; }
        public Atividade Predecessora { get; set; } = null!;
    }
}
