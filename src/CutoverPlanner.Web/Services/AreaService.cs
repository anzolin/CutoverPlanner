using CutoverPlanner.Domain.Models;
using CutoverPlanner.Web.Repositories;

namespace CutoverPlanner.Web.Services
{
    public class AreaService : IAreaService
    {
        private readonly IAreaRepository _areaRepository;

        public AreaService(IAreaRepository areaRepository)
        {
            _areaRepository = areaRepository;
        }

        public async Task<IEnumerable<Area>> GetAllAsync()
        {
            return await _areaRepository.GetAllAsync();
        }

        public async Task<Area?> GetByIdAsync(int id)
        {
            return await _areaRepository.GetByIdAsync(id);
        }

        public async Task<Area?> GetByNomeAsync(string nome)
        {
            return await _areaRepository.GetByNomeAsync(nome);
        }

        public async Task CreateAsync(Area area)
        {
            await _areaRepository.AddAsync(area);
        }

        public async Task UpdateAsync(Area area)
        {
            await _areaRepository.UpdateAsync(area);
        }

        public async Task DeleteAsync(int id)
        {
            await _areaRepository.DeleteAsync(id);
        }
    }
}
