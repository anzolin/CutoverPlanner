using System.ComponentModel.DataAnnotations;

namespace CutoverManager.Application.DTOs;

public class ExecutorDTO
{
    public int Id { get; set; }

    [Required]
    public int IdArea { get; set; }

    [Required, MaxLength(250)]
    public string Nome { get; set; } = default!;

    public string? Email { get; set; }
}