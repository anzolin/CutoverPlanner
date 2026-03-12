using CutoverPlanner.Domain.Models;

namespace CutoverPlanner.Web.Services
{
    public interface IMarcoService
    {
        Task<IEnumerable<Marco>> GetAllAsync();
        Task<Marco?> GetByIdAsync(int id);
        Task CreateAsync(Marco marco);
        Task UpdateAsync(Marco marco);
        Task DeleteAsync(int id);
    }
}