using System.ComponentModel.DataAnnotations;

namespace CutoverPlanner.Domain.Enumerations
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
