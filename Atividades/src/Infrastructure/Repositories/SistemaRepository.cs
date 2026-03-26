using CutoverManager.Domain.Entities;
using CutoverManager.Domain.Interfaces;
using CutoverManager.Infrastructure.Context;

namespace CutoverManager.Infrastructure.Repositories;

public class SistemaRepository : Repository<Sistema>, ISistemaRepository
{
    public SistemaRepository(AppDbContext ctx) : base(ctx) {}
}