namespace CutoverManager.Domain.Entities;

public class Executor
{
    public int Id { get; set; }
    public int IdArea { get; set; }

    public string Nome { get; set; } = default!;
    public string? Email { get; set; }

    public Area Area { get; set; } = default!;
    public ICollection<Atividade> Atividades { get; set; } = new List<Atividade>();
}