using CutoverManager.Domain.Interfaces;
using CutoverManager.Infrastructure.Context;

namespace CutoverManager.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _ctx;

    public Repository(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task AddAsync(T entity)
    {
        await _ctx.Set<T>().AddAsync(entity);
        await _ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _ctx.Set<T>().Remove(entity);
        await _ctx.SaveChangesAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _ctx.Set<T>().FindAsync(id);
    }

    public IQueryable<T> Query()
    {
        return _ctx.Set<T>().AsQueryable();
    }

    public async Task UpdateAsync(T entity)
    {
        _ctx.Set<T>().Update(entity);
        await _ctx.SaveChangesAsync();
    }
}