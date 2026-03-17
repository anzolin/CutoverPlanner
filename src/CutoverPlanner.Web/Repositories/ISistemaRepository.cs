using CutoverPlanner.Domain.Models;

namespace CutoverPlanner.Web.Repositories
{
    public interface ISistemaRepository
    {
        Task<IEnumerable<Sistema>> GetAllAsync();
        Task<Sistema?> GetByIdAsync(int id);
        Task<Sistema?> GetByNomeAsync(string nome);
        Task AddAsync(Sistema sistema);
        Task UpdateAsync(Sistema sistema);
        Task DeleteAsync(int id);
        Task<IEnumerable<Sistema>> GetByAreaIdAsync(int areaId);
    }
}
