using CutoverManager.Application.DTOs;
using CutoverManager.Application.Interfaces;
using CutoverManager.Domain.Entities;
using CutoverManager.Domain.Interfaces;

namespace CutoverManager.Application.Services;

public class SistemaService : ISistemaService
{
    private readonly ISistemaRepository _repo;

    public SistemaService(ISistemaRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<SistemaDTO>> ListarAsync()
    {
        return _repo.Query()
            .Select(s => new SistemaDTO
            {
                Id = s.Id,
                IdArea = s.IdArea,
                Nome = s.Nome
            })
            .ToList();
    }

    public async Task<SistemaDTO?> ObterPorIdAsync(int id)
    {
        var sys = await _repo.GetByIdAsync(id);
        if (sys == null) return null;

        return new SistemaDTO
        {
            Id = sys.Id,
            IdArea = sys.IdArea,
            Nome = sys.Nome
        };
    }

    public async Task CriarAsync(SistemaDTO dto)
    {
        var entity = new Sistema
        {
            IdArea = dto.IdArea,
            Nome = dto.Nome
        };

        await _repo.AddAsync(entity);
    }

    public async Task AtualizarAsync(SistemaDTO dto)
    {
        var entity = await _repo.GetByIdAsync(dto.Id)
            ?? throw new Exception("Sistema não encontrado.");

        entity.IdArea = dto.IdArea;
        entity.Nome = dto.Nome;

        await _repo.UpdateAsync(entity);
    }

    public async Task RemoverAsync(int id)
    {
        var sys = await _repo.GetByIdAsync(id)
            ?? throw new Exception("Sistema não encontrado.");

        await _repo.DeleteAsync(sys);
    }
}