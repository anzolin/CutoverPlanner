using CutoverManager.Domain.Entities;
using CutoverManager.Domain.Interfaces;
using CutoverManager.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CutoverManager.Infrastructure.Repositories;

public class AtividadeRepository : Repository<Atividade>, IAtividadeRepository
{
    public AtividadeRepository(AppDbContext ctx) : base(ctx)
    {}

    public async Task<IEnumerable<Atividade>> ListarPorPlano(int idPlano)
    {
        return await _ctx.Atividades
            .Where(a => a.IdPlano == idPlano)
            .Include(a => a.Executor).ThenInclude(e => e.Area)
            .Include(a => a.Sistema)
            .Include(a => a.Marco)
            .OrderBy(a => a.Inicio)
            .ToListAsync();
    }
}