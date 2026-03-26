using CutoverManager.Application.DTOs;
using CutoverManager.Application.Interfaces;
using CutoverManager.Domain.Entities;
using CutoverManager.Domain.Interfaces;

namespace CutoverManager.Application.Services;

public class AreaService : IAreaService
{
    private readonly IAreaRepository _repo;

    public AreaService(IAreaRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<AreaDTO>> ListarAsync()
    {
        return _repo.Query()
            .Select(a => new AreaDTO
            {
                Id = a.Id,
                Nome = a.Nome,
                NomeResponsavel = a.NomeResponsavel,
                EmailResponsavel = a.EmailResponsavel
            })
            .OrderBy(ob => ob.Nome)
            .ToList();
    }

    public async Task<AreaDTO?> ObterPorIdAsync(int id)
    {
        var a = await _repo.GetByIdAsync(id);
        if (a == null) return null;

        return new AreaDTO
        {
            Id = a.Id,
            Nome = a.Nome,
            NomeResponsavel = a.NomeResponsavel,
            EmailResponsavel = a.EmailResponsavel
        };
    }

    public async Task CriarAsync(AreaDTO dto)
    {
        var entity = new Area
        {
            Nome = dto.Nome,
            NomeResponsavel = dto.NomeResponsavel,
            EmailResponsavel = dto.EmailResponsavel
        };

        await _repo.AddAsync(entity);
    }

    public async Task AtualizarAsync(AreaDTO dto)
    {
        var entity = await _repo.GetByIdAsync(dto.Id)
            ?? throw new Exception("Área não encontrada.");

        entity.Nome = dto.Nome;
        entity.NomeResponsavel = dto.NomeResponsavel;
        entity.EmailResponsavel = dto.EmailResponsavel;

        await _repo.UpdateAsync(entity);
    }

    public async Task RemoverAsync(int id)
    {
        var a = await _repo.GetByIdAsync(id)
            ?? throw new Exception("Área não encontrada.");

        await _repo.DeleteAsync(a);
    }
}