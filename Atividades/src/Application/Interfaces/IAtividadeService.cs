using CutoverManager.Application.DTOs;
using CutoverManager.Domain.Enums;

namespace CutoverManager.Application.Interfaces;

public interface IAtividadeService
{
    Task<IEnumerable<AtividadeDTO>> ListarPorPlanoAsync(int idPlano);
    Task<IEnumerable<AtividadeDTO>> FiltrarAsync(
        int idPlano,
        int? idArea,
        bool? atrasadas,
        int? idMarco,
        int? idSistema,
        StatusAtividade? status
    );

    Task<AtividadeDTO?> ObterPorIdAsync(int id);
    Task CriarAsync(AtividadeDTO dto);
    Task AtualizarAsync(AtividadeDTO dto);
    Task RemoverAsync(int id);

    Task CopiarAtividadesAsync(int planoOrigem, int planoDestino);
    Task RemoverTodasDoPlanoAsync(int idPlano);

    Task AlterarStatusAsync(int idAtividade, StatusAtividade novoStatus);
}