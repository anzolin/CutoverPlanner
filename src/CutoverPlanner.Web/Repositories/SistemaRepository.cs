using CutoverPlanner.Domain.Models;

namespace CutoverPlanner.Web.Repositories
{
    public class SistemaRepository : ISistemaRepository
    {
        public Task AddAsync(Sistema sistema)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Sistema>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Sistema>> GetByAreaIdAsync(int areaId)
        {
            throw new NotImplementedException();
        }

        public Task<Sistema?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Sistema sistema)
        {
            throw new NotImplementedException();
        }
    }
}
