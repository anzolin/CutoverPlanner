using CutoverPlanner.Domain.Models;

namespace CutoverPlanner.Web.Repositories
{
    public class ExecutorRepository : IExecutorRepository
    {
        public Task AddAsync(Executor executor)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Executor>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Executor>> GetByAreaIdAsync(int areaId)
        {
            throw new NotImplementedException();
        }

        public Task<Executor?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Executor executor)
        {
            throw new NotImplementedException();
        }
    }
}
