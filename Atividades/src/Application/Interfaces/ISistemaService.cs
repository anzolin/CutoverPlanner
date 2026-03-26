using CutoverManager.Application.DTOs;

namespace CutoverManager.Application.Interfaces;

public interface ISistemaService
{
    Task<IEnumerable<SistemaDTO>> ListarAsync();
    Task<SistemaDTO?> ObterPorIdAsync(int id);
    Task CriarAsync(SistemaDTO dto);
    Task AtualizarAsync(SistemaDTO dto);
    Task RemoverAsync(int id);
}