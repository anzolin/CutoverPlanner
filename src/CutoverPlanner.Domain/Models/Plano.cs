using System.ComponentModel.DataAnnotations;

namespace CutoverPlanner.Domain.Models
{
    public class Plano
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime? Inicio { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime? Termino { get; set; }

        // Navegação
        public ICollection<Atividade> Atividades { get; set; } = new List<Atividade>();
    }
}
