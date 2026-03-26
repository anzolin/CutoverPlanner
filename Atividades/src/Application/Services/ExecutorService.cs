using CutoverManager.Application.DTOs;
using CutoverManager.Application.Interfaces;
using CutoverManager.Domain.Entities;
using CutoverManager.Domain.Interfaces;

namespace CutoverManager.Application.Services;

public class ExecutorService : IExecutorService
{
    private readonly IExecutorRepository _repo;

    public ExecutorService(IExecutorRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<ExecutorDTO>> ListarAsync()
    {
        return _repo.Query()
            .Select(e => new ExecutorDTO
            {
                Id = e.Id,
                IdArea = e.IdArea,
                Nome = e.Nome,
                Email = e.Email
            })
            .OrderBy(ob => ob.Nome)
            .ToList();
    }

    public async Task<ExecutorDTO?> ObterPorIdAsync(int id)
    {
        var e = await _repo.GetByIdAsync(id);
        if (e == null) return null;

        return new ExecutorDTO
        {
            Id = e.Id,
            IdArea = e.IdArea,
            Nome = e.Nome,
            Email = e.Email
        };
    }

    public async Task CriarAsync(ExecutorDTO dto)
    {
        var entity = new Executor
        {
            IdArea = dto.IdArea,
            Nome = dto.Nome,
            Email = dto.Email
        };

        await _repo.AddAsync(entity);
    }

    public async Task AtualizarAsync(ExecutorDTO dto)
    {
        var entity = await _repo.GetByIdAsync(dto.Id)
            ?? throw new Exception("Executor não encontrado.");

        entity.IdArea = dto.IdArea;
        entity.Nome = dto.Nome;
        entity.Email = dto.Email;

        await _repo.UpdateAsync(entity);
    }

    public async Task RemoverAsync(int id)
    {
        var e = await _repo.GetByIdAsync(id)
            ?? throw new Exception("Executor não encontrado.");

        await _repo.DeleteAsync(e);
    }
}