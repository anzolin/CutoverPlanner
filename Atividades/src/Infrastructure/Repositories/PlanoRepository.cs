using CutoverManager.Domain.Entities;
using CutoverManager.Domain.Interfaces;
using CutoverManager.Infrastructure.Context;

namespace CutoverManager.Infrastructure.Repositories;

public class PlanoRepository : Repository<Plano>, IPlanoRepository
{
    public PlanoRepository(AppDbContext ctx) : base(ctx) {}
}