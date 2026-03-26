using CutoverManager.Domain.Entities;

namespace CutoverManager.Domain.Services;

/// <summary>
/// Regras de negócio e validações de domínio relacionadas às atividades.
/// </summary>
public class AtividadeDomainService
{
    public void ValidarDatas(Atividade atividade, Plano plano)
    {
        if (atividade.Inicio < plano.Inicio || atividade.Termino > plano.Termino)
            throw new Exception("As datas da atividade devem estar dentro do período do plano.");

        if (atividade.Inicio > atividade.Termino)
            throw new Exception("A data de início não pode ser maior que a data de término.");
    }
}