using System.Linq.Expressions;

namespace CutoverManager.Domain.Interfaces;

/// <summary>
/// Interface genérica para repositórios.
/// </summary>
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    IQueryable<T> Query();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}