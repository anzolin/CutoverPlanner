using CutoverPlanner.Domain.Models;

namespace CutoverPlanner.Web.Repositories
{
    public interface IMarcoRepository
    {
        Task<IEnumerable<Marco>> GetAllAsync();
        Task<Marco?> GetByIdAsync(int id);
        Task<Marco?> GetByNomeAsync(string nome);
        Task AddAsync(Marco marco);
        Task UpdateAsync(Marco marco);
        Task DeleteAsync(int id);
    }
}
