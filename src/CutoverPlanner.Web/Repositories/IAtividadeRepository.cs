using CutoverPlanner.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CutoverPlanner.Web.Repositories
{
    /// <summary>
    /// Abstraction for operations on <see cref="Atividade"/> entities.
    /// </summary>
    public interface IAtividadeRepository
    {
        Task<List<Atividade>> GetFilteredAsync(string? status, string? sistema, string? area,
            string? responsavel, string? busca, bool? atrasadas);

        Task<Atividade?> FindAsync(int id);
        Task<Atividade?> GetWithDependenciesAsync(int id);
        Task AddAsync(Atividade atividade);
        Task UpdateAsync(Atividade atividade);
        Task DeleteAsync(Atividade atividade);
        Task DeleteAllAsync();
        Task<List<Atividade>> GetAllAsync();
    }
}