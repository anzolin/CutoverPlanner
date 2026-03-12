using CutoverPlanner.Domain.Models;
using CutoverPlanner.Domain.Enumerations;

namespace CutoverPlanner.Web.Services
{
    public interface IAtividadeService
    {
        Task<List<Atividade>> GetAllAsync();
        Task<Atividade?> GetByIdAsync(int id);
        Task CreateAsync(Atividade atividade);
        Task UpdateAsync(Atividade atividade);
        Task DeleteAsync(Atividade atividade);
        Task DeleteAllAsync();

        Task<List<Atividade>> GetFilteredAsync(
            string? status,
            string? sistema,
            string? area,
            string? responsavelArea,
            string? executor,
            string? busca,
            bool? atrasadas);
    }
}