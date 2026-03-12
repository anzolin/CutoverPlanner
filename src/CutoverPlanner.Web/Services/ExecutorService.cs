using CutoverPlanner.Domain.Models;
using CutoverPlanner.Web.Repositories;

namespace CutoverPlanner.Web.Services
{
    public class ExecutorService : IExecutorService
    {
        private readonly IExecutorRepository _executorRepository;

        public ExecutorService(IExecutorRepository executorRepository)
        {
            _executorRepository = executorRepository;
        }

        public async Task<IEnumerable<Executor>> GetAllAsync()
        {
            return await _executorRepository.GetAllAsync();
        }

        public async Task<Executor?> GetByIdAsync(int id)
        {
            return await _executorRepository.GetByIdAsync(id);
        }

        public async Task CreateAsync(Executor executor)
        {
            await _executorRepository.AddAsync(executor);
        }

        public async Task UpdateAsync(Executor executor)
        {
            await _executorRepository.UpdateAsync(executor);
        }

        public async Task DeleteAsync(int id)
        {
            await _executorRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Executor>> GetByAreaIdAsync(int areaId)
        {
            return await _executorRepository.GetByAreaIdAsync(areaId);
        }
    }
}