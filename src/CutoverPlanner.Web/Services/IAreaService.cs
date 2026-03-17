using CutoverPlanner.Domain.Models;

namespace CutoverPlanner.Web.Services
{
    public interface IAreaService
    {
        Task<IEnumerable<Area>> GetAllAsync();
        Task<Area?> GetByIdAsync(int id);
        Task<Area?> GetByNomeAsync(string nome);
        Task CreateAsync(Area area);
        Task UpdateAsync(Area area);
        Task DeleteAsync(int id);
    }
}