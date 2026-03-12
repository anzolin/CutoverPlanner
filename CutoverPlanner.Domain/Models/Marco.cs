using System.ComponentModel.DataAnnotations;

namespace CutoverPlanner.Domain.Models
{
    public class Marco
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; } = string.Empty;

        // Navegação
        public ICollection<Atividade> Atividades { get; set; } = new List<Atividade>();
    }
}
