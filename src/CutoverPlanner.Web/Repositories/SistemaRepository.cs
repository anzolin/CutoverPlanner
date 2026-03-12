using CutoverPlanner.Domain.Models;
using CutoverPlanner.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace CutoverPlanner.Web.Repositories
{
    public class SistemaRepository : ISistemaRepository
    {
        private readonly AppDbContext _db;
        public SistemaRepository(AppDbContext db) => _db = db;

        public async Task AddAsync(Sistema sistema)
        {
            _db.Sistemas.Add(sistema);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var sistema = await _db.Sistemas.FindAsync(id);
            if (sistema != null)
            {
                _db.Sistemas.Remove(sistema);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Sistema>> GetAllAsync()
        {
            return await _db.Sistemas.AsNoTracking()
                .Include(s => s.Area)
                .ToListAsync();
        }

        public async Task<IEnumerable<Sistema>> GetByAreaIdAsync(int areaId)
        {
            return await _db.Sistemas.AsNoTracking()
                .Where(s => s.IdArea == areaId)
                .ToListAsync();
        }

        public async Task<Sistema?> GetByIdAsync(int id)
        {
            return await _db.Sistemas.FindAsync(id);
        }

        public async Task UpdateAsync(Sistema sistema)
        {
            _db.Sistemas.Update(sistema);
            await _db.SaveChangesAsync();
        }
    }
}
