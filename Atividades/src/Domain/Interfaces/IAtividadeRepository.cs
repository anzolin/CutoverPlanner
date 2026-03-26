using CutoverManager.Domain.Entities;

namespace CutoverManager.Domain.Interfaces;

public interface IAtividadeRepository : IRepository<Atividade>
{
    Task<IEnumerable<Atividade>> ListarPorPlano(int idPlano);
}