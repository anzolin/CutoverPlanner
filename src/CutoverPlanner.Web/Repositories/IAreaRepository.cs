using CutoverPlanner.Domain.Models;

namespace CutoverPlanner.Web.Repositories
{
    public interface IAreaRepository
    {
        Task<IEnumerable<Area>> GetAllAsync();
        Task<Area?> GetByIdAsync(int id);
        Task AddAsync(Area area);
        Task UpdateAsync(Area area);
        Task DeleteAsync(int id);
    }
}