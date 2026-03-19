using CutoverPlanner.Domain.Models;
using CutoverPlanner.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace CutoverPlanner.Web.Repositories
{
    public class ExecutorRepository : IExecutorRepository
    {
        private readonly AppDbContext _db;
        public ExecutorRepository(AppDbContext db) => _db = db;

        public async Task AddAsync(Executor executor)
        {
            _db.Executores.Add(executor);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var executor = await _db.Executores.FindAsync(id);
            if (executor != null)
            {
                _db.Executores.Remove(executor);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Executor>> GetAllAsync()
        {
            return await _db.Executores.AsNoTracking()
                .Include(e => e.Area)
                .OrderBy(ob => ob.Area.Nome)
                .ThenBy(ob => ob.Nome)
                .ToListAsync();
        }

        public async Task<IEnumerable<Executor>> GetByAreaIdAsync(int areaId)
        {
            return await _db.Executores.AsNoTracking()
                .Where(e => e.IdArea == areaId)
                .ToListAsync();
        }

        public async Task<Executor?> GetByIdAsync(int id)
        {
            return await _db.Executores.FindAsync(id);
        }

        public async Task<Executor?> GetByNomeAsync(string nome)
        {
            return await _db.Executores.FirstOrDefaultAsync(q => q.Nome.ToLower().Equals(nome.ToLower()));
        }

        public async Task<Executor?> GetByNomeAreaAsync(string nome, string area)
        {
            return await _db.Executores.FirstOrDefaultAsync(q => q.Nome.ToLower().Equals(nome.ToLower()) && q.Area.Nome.ToLower().Equals(area.ToLower()));
        }

        public async Task UpdateAsync(Executor executor)
        {
            _db.Executores.Update(executor);
            await _db.SaveChangesAsync();
        }
    }
}
