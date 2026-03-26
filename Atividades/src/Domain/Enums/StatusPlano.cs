using System.ComponentModel.DataAnnotations;

namespace CutoverManager.Domain.Enums;

public enum StatusPlano
{
    [Display(Name = "Ativo")]
    Ativo = 1,

    [Display(Name = "Inativo")]
    Inativo = 2
}