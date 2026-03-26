using CutoverManager.Application.DTOs;

namespace CutoverManager.Application.Interfaces;

public interface IPlanoService
{
    Task<IEnumerable<PlanoDTO>> ListarAsync();
    Task<IEnumerable<PlanoDTO>> FiltrarAsync(string? nome);
    Task<PlanoDTO?> ObterPorIdAsync(int id);
    Task CriarAsync(PlanoDTO dto);
    Task AtualizarAsync(PlanoDTO dto);
    Task RemoverAsync(int id);
}