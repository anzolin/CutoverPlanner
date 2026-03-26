namespace CutoverManager.Domain.Entities;

public class Sistema
{
    public int Id { get; set; }
    public int IdArea { get; set; }

    public string Nome { get; set; } = default!;

    public Area Area { get; set; } = default!;
    public ICollection<Atividade> Atividades { get; set; } = new List<Atividade>();
}