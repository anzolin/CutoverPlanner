using CutoverPlanner.Domain.Models;
using CutoverPlanner.Web.Repositories;

namespace CutoverPlanner.Web.Services
{
    public class PlanoService : IPlanoService
    {
        private readonly IPlanoRepository _planoRepository;

        public PlanoService(IPlanoRepository planoRepository)
        {
            _planoRepository = planoRepository;
        }

        public async Task<IEnumerable<Plano>> GetAllAsync()
        {
            return await _planoRepository.GetAllAsync();
        }

        public async Task<Plano?> GetByIdAsync(int id)
        {
            return await _planoRepository.GetByIdAsync(id);
        }

        public async Task CreateAsync(Plano area)
        {
            await _planoRepository.AddAsync(area);
        }

        public async Task UpdateAsync(Plano area)
        {
            await _planoRepository.UpdateAsync(area);
        }

        public async Task DeleteAsync(int id)
        {
            await _planoRepository.DeleteAsync(id);
        }
    }
}
