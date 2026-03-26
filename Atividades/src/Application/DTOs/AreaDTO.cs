using System.ComponentModel.DataAnnotations;

namespace CutoverManager.Application.DTOs;

public class AreaDTO
{
    public int Id { get; set; }

    [Required, MaxLength(250)]
    public string Nome { get; set; } = default!;

    public string? NomeResponsavel { get; set; }
    public string? EmailResponsavel { get; set; }
}