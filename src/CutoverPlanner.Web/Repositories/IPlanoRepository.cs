using CutoverPlanner.Domain.Models;

namespace CutoverPlanner.Web.Repositories
{
    public interface IPlanoRepository
    {
        Task<IEnumerable<Plano>> GetAllAsync();
        Task<Plano?> GetByIdAsync(int id);
        Task AddAsync(Plano plano);
        Task UpdateAsync(Plano plano);
        Task DeleteAsync(int id);
    }
}
