using CutoverManager.Application.DTOs;

namespace CutoverManager.Application.Interfaces;

public interface IAreaService
{
    Task<IEnumerable<AreaDTO>> ListarAsync();
    Task<AreaDTO?> ObterPorIdAsync(int id);
    Task CriarAsync(AreaDTO dto);
    Task AtualizarAsync(AreaDTO dto);
    Task RemoverAsync(int id);
}