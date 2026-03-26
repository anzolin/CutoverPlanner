using CutoverManager.Domain.Entities;
using CutoverManager.Domain.Interfaces;
using CutoverManager.Infrastructure.Context;

namespace CutoverManager.Infrastructure.Repositories;

public class AreaRepository : Repository<Area>, IAreaRepository
{
    public AreaRepository(AppDbContext ctx) : base(ctx) {}
}