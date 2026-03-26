using CutoverManager.Domain.Entities;

namespace CutoverManager.Domain.Services;

public class PlanoDomainService
{
    public void ValidarPeriodo(Plano plano)
    {
        if (plano.Inicio > plano.Termino)
            throw new Exception("Data de início do plano não pode ser maior que a data de término.");
    }
}