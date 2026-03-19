using CutoverPlanner.Domain.Models;
using CutoverPlanner.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace CutoverPlanner.Web.Repositories
{
    public class AreaRepository : IAreaRepository
    {
        private readonly AppDbContext _db;
        public AreaRepository(AppDbContext db) => _db = db;

        public async Task AddAsync(Area area)
        {
            _db.Areas.Add(area);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var area = await _db.Areas.FindAsync(id);
            if (area != null)
            {
                _db.Areas.Remove(area);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Area>> GetAllAsync()
        {
            return await _db.Areas.AsNoTracking().OrderBy(ob => ob.Nome).ToListAsync();
        }

        public async Task<Area?> GetByIdAsync(int id)
        {
            return await _db.Areas.FindAsync(id);
        }

        public async Task<Area?> GetByNomeAsync(string nome)
        {
            return await _db.Areas.FirstOrDefaultAsync(q => q.Nome.ToLower().Equals(nome.ToLower()));
        }

        public async Task UpdateAsync(Area area)
        {
            _db.Areas.Update(area);
            await _db.SaveChangesAsync();
        }
    }
}
