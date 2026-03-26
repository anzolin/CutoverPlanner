using CutoverManager.Application.DTOs;
using CutoverManager.Application.Interfaces;
using CutoverManager.Domain.Entities;
using CutoverManager.Domain.Interfaces;

namespace CutoverManager.Application.Services;

public class MarcoService : IMarcoService
{
    private readonly IMarcoRepository _repo;

    public MarcoService(IMarcoRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<MarcoDTO>> ListarAsync()
    {
        return _repo.Query()
            .Select(m => new MarcoDTO
            {
                Id = m.Id,
                Nome = m.Nome
            })
            .ToList();
    }

    public async Task<MarcoDTO?> ObterPorIdAsync(int id)
    {
        var m = await _repo.GetByIdAsync(id);
        if (m == null) return null;

        return new MarcoDTO
        {
            Id = m.Id,
            Nome = m.Nome
        };
    }

    public async Task CriarAsync(MarcoDTO dto)
    {
        var entity = new Marco
        {
            Nome = dto.Nome
        };

        await _repo.AddAsync(entity);
    }

    public async Task AtualizarAsync(MarcoDTO dto)
    {
        var entity = await _repo.GetByIdAsync(dto.Id)
            ?? throw new Exception("Marco não encontrado.");

        entity.Nome = dto.Nome;

        await _repo.UpdateAsync(entity);
    }

    public async Task RemoverAsync(int id)
    {
        var m = await _repo.GetByIdAsync(id)
            ?? throw new Exception("Marco não encontrado.");

        await _repo.DeleteAsync(m);
    }
}