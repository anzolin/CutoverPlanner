using CutoverPlanner.Domain.Models;
using CutoverPlanner.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace CutoverPlanner.Web.Repositories
{
    public class PlanoRepository : IPlanoRepository
    {
        private readonly AppDbContext _db;
        public PlanoRepository(AppDbContext db) => _db = db;

        public async Task AddAsync(Plano plano)
        {
            _db.Planos.Add(plano);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var plano = await _db.Planos.FindAsync(id);
            if (plano != null)
            {
                _db.Planos.Remove(plano);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Plano>> GetAllAsync()
        {
            return await _db.Planos
                .AsNoTracking()
                .Include(i => i.Atividades)
                .OrderByDescending(ob => ob.Inicio)
                .ThenBy(ob => ob.Nome)
                .ToListAsync();
        }

        public async Task<Plano?> GetByIdAsync(int id)
        {
            return await _db.Planos.FindAsync(id);
        }

        public async Task UpdateAsync(Plano plano)
        {
            _db.Planos.Update(plano);
            await _db.SaveChangesAsync();
        }
    }
}
