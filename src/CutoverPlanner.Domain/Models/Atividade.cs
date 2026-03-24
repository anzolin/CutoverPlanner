using CutoverPlanner.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace CutoverPlanner.Domain.Models
{
    public class Atividade
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IdPlano { get; set; }

        public Plano? Plano { get; set; }

        [Required]
        public int IdSistema { get; set; }

        public Sistema? Sistema { get; set; }

        [Required]
        public int IdExecutor { get; set; }

        public Executor? Executor { get; set; }

        [Required]
        public int IdMarco { get; set; }

        // should reference Marco entity, not Sistema
        public Marco? Marco { get; set; }

        [Required]
        public string Titulo { get; set; } = string.Empty;

        public StatusAtividade Status { get; set; }

        public bool RiscoGoLive { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime? Inicio { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime? Termino { get; set; }

        public string? Observacao { get; set; }

        public string? LinkRepositorio { get; set; }
    }
}
