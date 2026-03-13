using CutoverPlanner.Domain.Models;
using CutoverPlanner.Web.Repositories;

namespace CutoverPlanner.Web.Services
{
    public class MarcoService : IMarcoService
    {
        private readonly IMarcoRepository _repo;
        public MarcoService(IMarcoRepository repo) => _repo = repo;

        public async Task<IEnumerable<Marco>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Marco?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<Marco?> GetByNomeAsync(string nome)
        {
            return await _repo.GetByNomeAsync(nome);
        }

        public async Task CreateAsync(Marco marco)
        {
            await _repo.AddAsync(marco);
        }

        public async Task UpdateAsync(Marco marco)
        {
            await _repo.UpdateAsync(marco);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}