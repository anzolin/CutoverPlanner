using CutoverManager.Domain.Enums;

namespace CutoverManager.Domain.Entities;

/// <summary>
/// Atividade de execução dentro de um plano.
/// </summary>
public class Atividade
{
    public int Id { get; set; }

    public int IdPlano { get; set; }
    public int IdSistema { get; set; }
    public int IdMarco { get; set; }
    public int IdExecutor { get; set; }

    public string Titulo { get; set; } = default!;
    public StatusAtividade Status { get; set; }
    public bool RiscoGoLive { get; set; }
    public DateTime Inicio { get; set; }
    public DateTime Termino { get; set; }
    public string? Observacao { get; set; }

    public Plano Plano { get; set; } = default!;
    public Sistema Sistema { get; set; } = default!;
    public Marco Marco { get; set; } = default!;
    public Executor Executor { get; set; } = default!;
}