using CutoverManager.Domain.Enums;

namespace CutoverManager.Domain.Entities;

/// <summary>
/// Representa um plano de cutover.
/// </summary>
public class Plano
{
    public int Id { get; set; }
    public string Nome { get; set; } = default!;
    public DateTime Inicio { get; set; }
    public DateTime Termino { get; set; }
    public StatusPlano Status { get; set; }

    public ICollection<Atividade> Atividades { get; set; } = new List<Atividade>();
}