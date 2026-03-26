using CutoverManager.Application.DTOs;
using CutoverManager.Application.Interfaces;
using CutoverManager.Domain.Entities;
using CutoverManager.Domain.Interfaces;
using CutoverManager.Domain.Services;

namespace CutoverManager.Application.Services;

public class PlanoService : IPlanoService
{
    private readonly IPlanoRepository _repo;
    private readonly PlanoDomainService _domain;

    public PlanoService(
        IPlanoRepository repo, 
        PlanoDomainService domain
    )
    {
        _repo = repo;
        _domain = domain;
    }

    public async Task<IEnumerable<PlanoDTO>> ListarAsync()
    {
        return _repo.Query()
            .Select(p => new PlanoDTO
            {
                Id = p.Id,
                Nome = p.Nome,
                Inicio = p.Inicio,
                Termino = p.Termino,
                Status = p.Status
            }).ToList();
    }

    public async Task<IEnumerable<PlanoDTO>> FiltrarAsync(string? nome)
    {
        var q = _repo.Query();

        if (!string.IsNullOrWhiteSpace(nome))
            q = q.Where(p => p.Nome.Contains(nome));

        return q.Select(p => new PlanoDTO
        {
            Id = p.Id,
            Nome = p.Nome,
            Inicio = p.Inicio,
            Termino = p.Termino,
            Status = p.Status
        }).ToList();
    }

    public async Task<PlanoDTO?> ObterPorIdAsync(int id)
    {
        var p = await _repo.GetByIdAsync(id);
        if (p == null) return null;

        return new PlanoDTO
        {
            Id = p.Id,
            Nome = p.Nome,
            Inicio = p.Inicio,
            Termino = p.Termino,
            Status = p.Status
        };
    }

    public async Task CriarAsync(PlanoDTO dto)
    {
        var plano = new Plano
        {
            Nome = dto.Nome,
            Inicio = dto.Inicio,
            Termino = dto.Termino,
            Status = dto.Status
        };

        _domain.ValidarPeriodo(plano);

        await _repo.AddAsync(plano);
    }

    public async Task AtualizarAsync(PlanoDTO dto)
    {
        var plano = await _repo.GetByIdAsync(dto.Id)
            ?? throw new Exception("Plano não encontrado.");

        plano.Nome = dto.Nome;
        plano.Inicio = dto.Inicio;
        plano.Termino = dto.Termino;
        plano.Status = dto.Status;

        _domain.ValidarPeriodo(plano);

        await _repo.UpdateAsync(plano);
    }

    public async Task RemoverAsync(int id)
    {
        var plano = await _repo.GetByIdAsync(id)
            ?? throw new Exception("Plano não encontrado.");

        if (plano.Atividades.Any())
            throw new Exception("Não é possível excluir um plano com atividades.");

        await _repo.DeleteAsync(plano);
    }
}