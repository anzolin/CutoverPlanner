using CutoverPlanner.Domain.Models;

namespace CutoverPlanner.Web.Services
{
    public interface ISistemaService
    {
        Task<IEnumerable<Sistema>> GetAllAsync();
        Task<Sistema?> GetByIdAsync(int id);
        Task CreateAsync(Sistema sistema);
        Task UpdateAsync(Sistema sistema);
        Task DeleteAsync(int id);
        Task<IEnumerable<Sistema>> GetByAreaIdAsync(int areaId);
    }
}