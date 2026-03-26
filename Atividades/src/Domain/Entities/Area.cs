namespace CutoverManager.Domain.Entities;

public class Area
{
    public int Id { get; set; }
    public string Nome { get; set; } = default!;
    public string? NomeResponsavel { get; set; }
    public string? EmailResponsavel { get; set; }

    public ICollection<Executor> Executores { get; set; } = new List<Executor>();
    public ICollection<Sistema> Sistemas { get; set; } = new List<Sistema>();
}