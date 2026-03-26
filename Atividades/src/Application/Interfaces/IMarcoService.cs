using CutoverManager.Application.DTOs;

namespace CutoverManager.Application.Interfaces;

public interface IMarcoService
{
    Task<IEnumerable<MarcoDTO>> ListarAsync();
    Task<MarcoDTO?> ObterPorIdAsync(int id);
    Task CriarAsync(MarcoDTO dto);
    Task AtualizarAsync(MarcoDTO dto);
    Task RemoverAsync(int id);
}