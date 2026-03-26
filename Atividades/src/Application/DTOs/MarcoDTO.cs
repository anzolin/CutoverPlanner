using System.ComponentModel.DataAnnotations;

namespace CutoverManager.Application.DTOs;

public class MarcoDTO
{
    public int Id { get; set; }

    [Required, MaxLength(250)]
    public string Nome { get; set; } = default!;
}