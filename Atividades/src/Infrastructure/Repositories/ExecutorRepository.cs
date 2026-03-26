using CutoverManager.Domain.Entities;
using CutoverManager.Domain.Interfaces;
using CutoverManager.Infrastructure.Context;

namespace CutoverManager.Infrastructure.Repositories;

public class ExecutorRepository : Repository<Executor>, IExecutorRepository
{
    public ExecutorRepository(AppDbContext ctx) : base(ctx) {}
}