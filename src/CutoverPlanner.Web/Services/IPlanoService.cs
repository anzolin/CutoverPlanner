using CutoverPlanner.Domain.Models;

namespace CutoverPlanner.Web.Services
{
    public interface IPlanoService
    {
        Task<IEnumerable<Plano>> GetAllAsync();
        Task<Plano?> GetByIdAsync(int id);
        Task CreateAsync(Plano plano);
        Task UpdateAsync(Plano plano);
        Task DeleteAsync(int id);
    }
}
