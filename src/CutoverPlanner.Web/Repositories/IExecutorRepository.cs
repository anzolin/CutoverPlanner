using CutoverPlanner.Domain.Models;

namespace CutoverPlanner.Web.Repositories
{
    public interface IExecutorRepository
    {
        Task<IEnumerable<Executor>> GetAllAsync();
        Task<Executor?> GetByIdAsync(int id);
        Task<Executor?> GetByNomeAsync(string nome);
        Task<Executor?> GetByNomeAreaAsync(string nome, string area);
        Task AddAsync(Executor executor);
        Task UpdateAsync(Executor executor);
        Task DeleteAsync(int id);
        Task<IEnumerable<Executor>> GetByAreaIdAsync(int areaId);
    }
}