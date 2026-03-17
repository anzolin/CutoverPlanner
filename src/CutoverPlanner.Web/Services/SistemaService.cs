using CutoverPlanner.Domain.Models;
using CutoverPlanner.Web.Repositories;

namespace CutoverPlanner.Web.Services
{
    public class SistemaService : ISistemaService
    {
        private readonly ISistemaRepository _sistemaRepository;

        public SistemaService(ISistemaRepository sistemaRepository)
        {
            _sistemaRepository = sistemaRepository;
        }

        public async Task<IEnumerable<Sistema>> GetAllAsync()
        {
            return await _sistemaRepository.GetAllAsync();
        }

        public async Task<Sistema?> GetByIdAsync(int id)
        {
            return await _sistemaRepository.GetByIdAsync(id);
        }

        public async Task<Sistema?> GetByNomeAsync(string nome)
        {
            return await _sistemaRepository.GetByNomeAsync(nome);
        }

        public async Task CreateAsync(Sistema sistema)
        {
            await _sistemaRepository.AddAsync(sistema);
        }

        public async Task UpdateAsync(Sistema sistema)
        {
            await _sistemaRepository.UpdateAsync(sistema);
        }

        public async Task DeleteAsync(int id)
        {
            await _sistemaRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Sistema>> GetByAreaIdAsync(int areaId)
        {
            return await _sistemaRepository.GetByAreaIdAsync(areaId);
        }
    }
}