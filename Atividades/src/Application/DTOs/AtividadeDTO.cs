using CutoverManager.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CutoverManager.Application.DTOs;

public class AtividadeDTO
{
    public int Id { get; set; }

    [Required]
    public int IdPlano { get; set; }

    [Required]
    public int IdSistema { get; set; }

    [Required]
    public int IdMarco { get; set; }

    [Required]
    public int IdExecutor { get; set; }

    [Required, MaxLength(500)]
    public string Titulo { get; set; } = default!;

    public StatusAtividade Status { get; set; }

    [Required]
    public bool RiscoGoLive { get; set; }

    [Required]
    public DateTime Inicio { get; set; }

    [Required]
    public DateTime Termino { get; set; }

    public string? Observacao { get; set; }

    public string? ExecutorNome { get; set; }
    public string? ExecutorArea { get; set; }
}
