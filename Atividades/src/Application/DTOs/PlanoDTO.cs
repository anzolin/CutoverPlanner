using CutoverManager.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CutoverManager.Application.DTOs;

public class PlanoDTO
{
    public int Id { get; set; }

    [Required, MaxLength(250)]
    public string Nome { get; set; } = default!;

    [Required]
    public DateTime Inicio { get; set; }

    [Required]
    public DateTime Termino { get; set; }

    public StatusPlano Status { get; set; }
}