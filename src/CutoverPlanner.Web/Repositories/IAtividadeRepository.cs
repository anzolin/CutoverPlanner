using CutoverPlanner.Domain.Models;

namespace CutoverPlanner.Web.Repositories
{
    /// <summary>
    /// Abstraction for operations on <see cref="Atividade"/> entities.
    /// </summary>
    public interface IAtividadeRepository
    {
        Task<List<Atividade>> GetFilteredAsync(string? status, string? sistema, string? area, string? responsavelArea, string? executor, string? busca, bool? atrasadas, bool? riscoGoLive);
        Task<Atividade?> FindAsync(int id);
        Task AddAsync(Atividade atividade);
        Task UpdateAsync(Atividade atividade);
        Task DeleteAsync(Atividade atividade);
        Task DeleteAllAsync();
        Task<List<Atividade>> GetAllAsync();
    }
}