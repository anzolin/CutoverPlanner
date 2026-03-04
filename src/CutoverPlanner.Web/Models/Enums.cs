
using System.ComponentModel.DataAnnotations;

namespace CutoverPlanner.Web.Models
{
    public enum StatusAtividade
    {
        [Display(Name = "Não iniciado")]
        NaoIniciado = 0,

        [Display(Name = "Em andamento")]
        EmAndamento = 1,

        [Display(Name = "Concluído")]
        Concluido = 2
    }
}
