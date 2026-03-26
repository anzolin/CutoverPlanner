using CutoverManager.Application.DTOs;

namespace CutoverManager.Application.Interfaces;

public interface IExecutorService
{
    Task<IEnumerable<ExecutorDTO>> ListarAsync();
    Task<ExecutorDTO?> ObterPorIdAsync(int id);
    Task CriarAsync(ExecutorDTO dto);
    Task AtualizarAsync(ExecutorDTO dto);
    Task RemoverAsync(int id);
}