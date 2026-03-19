using CutoverPlanner.Domain.Models;
using CutoverPlanner.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace CutoverPlanner.Web.Repositories
{
    public class MarcoRepository : IMarcoRepository
    {
        private readonly AppDbContext _db;
        public MarcoRepository(AppDbContext db) => _db = db;

        public async Task AddAsync(Marco marco)
        {
            _db.Marcos.Add(marco);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var marco = await _db.Marcos.FindAsync(id);
            if (marco != null)
            {
                _db.Marcos.Remove(marco);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Marco>> GetAllAsync()
        {
            return await _db.Marcos.AsNoTracking().OrderBy(ob => ob.Nome).ToListAsync();
        }

        public async Task<Marco?> GetByIdAsync(int id)
        {
            return await _db.Marcos.FindAsync(id);
        }

        public async Task<Marco?> GetByNomeAsync(string nome)
        {
            return await _db.Marcos.FirstOrDefaultAsync(q => q.Nome.ToLower().Equals(nome.ToLower()));
        }

        public async Task UpdateAsync(Marco marco)
        {
            _db.Marcos.Update(marco);
            await _db.SaveChangesAsync();
        }
    }
}
