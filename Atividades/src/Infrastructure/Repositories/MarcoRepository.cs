using CutoverManager.Domain.Entities;
using CutoverManager.Domain.Interfaces;
using CutoverManager.Infrastructure.Context;

namespace CutoverManager.Infrastructure.Repositories;

public class MarcoRepository : Repository<Marco>, IMarcoRepository
{
    public MarcoRepository(AppDbContext ctx) : base(ctx) {}
}