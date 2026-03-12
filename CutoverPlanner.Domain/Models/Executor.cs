using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CutoverPlanner.Domain.Models
{
    public class Executor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IdArea { get; set; }

        [ForeignKey("IdArea")]
        public Area? Area { get; set; } = null!;

        [Required]
        public string Nome { get; set; } = string.Empty;

        public string? Email { get; set; }

        // Navegação
        public ICollection<Atividade> Atividades { get; set; } = new List<Atividade>();
    }
}
