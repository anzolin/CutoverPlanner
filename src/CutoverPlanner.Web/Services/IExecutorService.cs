using CutoverPlanner.Domain.Models;

namespace CutoverPlanner.Web.Services
{
    public interface IExecutorService
    {
        Task<IEnumerable<Executor>> GetAllAsync();
        Task<Executor?> GetByIdAsync(int id);
        Task<Executor?> GetByNomeAsync(string nome);
        Task<Executor?> GetByNomeAreaAsync(string nome, string area);
        Task CreateAsync(Executor executor);
        Task UpdateAsync(Executor executor);
        Task DeleteAsync(int id);
        Task<IEnumerable<Executor>> GetByAreaIdAsync(int areaId);
    }
}
