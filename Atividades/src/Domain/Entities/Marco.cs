namespace CutoverManager.Domain.Entities;

public class Marco
{
    public int Id { get; set; }
    public string Nome { get; set; } = default!;

    public ICollection<Atividade> Atividades { get; set; } = new List<Atividade>();
}