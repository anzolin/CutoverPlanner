using CutoverPlanner.Domain.Models;
using CutoverPlanner.Web.Repositories;
using CutoverPlanner.Domain.Enumerations;

namespace CutoverPlanner.Web.Services
{
    public class AtividadeService : IAtividadeService
    {
        private readonly IAtividadeRepository _repo;

        public AtividadeService(IAtividadeRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Atividade>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Atividade?> GetByIdAsync(int id)
        {
            return await _repo.FindAsync(id);
        }

        public async Task CreateAsync(Atividade atividade)
        {
            await _repo.AddAsync(atividade);
        }

        public async Task UpdateAsync(Atividade atividade)
        {
            await _repo.UpdateAsync(atividade);
        }

        public async Task DeleteAsync(Atividade atividade)
        {
            await _repo.DeleteAsync(atividade);
        }

        public async Task DeleteAllAsync()
        {
            await _repo.DeleteAllAsync();
        }

        public async Task<List<Atividade>> GetFilteredAsync(
            string? status,
            string? sistema,
            string? area,
            string? responsavelArea,
            string? executor,
            string? busca,
            bool? atrasadas)
        {
            return await _repo.GetFilteredAsync(status, sistema, area, responsavelArea, executor, busca, atrasadas);
        }
    }
}