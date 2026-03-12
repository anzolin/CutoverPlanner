using System.ComponentModel.DataAnnotations;

namespace CutoverPlanner.Domain.Models
{
    public class Area
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; } = string.Empty;
        public string? NomeResponsavel { get; set; }
        public string? EmailResponsavel { get; set; }

        // Navegação
        public ICollection<Executor> Executores { get; set; } = new List<Executor>();
        public ICollection<Sistema> Sistemas { get; set; } = new List<Sistema>();
    }
}
