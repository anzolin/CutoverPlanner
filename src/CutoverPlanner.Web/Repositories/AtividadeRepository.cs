using CutoverPlanner.Web.Data;
using CutoverPlanner.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace CutoverPlanner.Web.Repositories
{
    /// <summary>
    /// EF Core implementation of <see cref="IAtividadeRepository"/>.
    /// Encapsulates all filtering and persistence logic for activities.
    /// </summary>
    public class AtividadeRepository : IAtividadeRepository
    {
        private readonly AppDbContext _db;

        public AtividadeRepository(AppDbContext db) => _db = db;

        public async Task<List<Atividade>> GetFilteredAsync(string? status, string? sistema,
            string? area, string? responsavel, string? busca, bool? atrasadas)
        {
            var qq = _db.Atividades.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<StatusAtividade>(status, out var st))
                qq = qq.Where(a => a.Status == st);
            if (!string.IsNullOrWhiteSpace(sistema)) qq = qq.Where(a => a.Sistema!.Contains(sistema));
            if (!string.IsNullOrWhiteSpace(area)) qq = qq.Where(a => a.AreaExecutoraNome!.Contains(area));
            if (!string.IsNullOrWhiteSpace(responsavel)) qq = qq.Where(a => a.Responsavel!.Contains(responsavel));
            if (!string.IsNullOrWhiteSpace(busca))
                qq = qq.Where(a => (a.Titulo != null && a.Titulo.Contains(busca)) ||
                                   (a.Observacao != null && a.Observacao.Contains(busca)));
            if (atrasadas == true)
            {
                var today = DateTime.Today;
                qq = qq.Where(a => a.Status != StatusAtividade.Concluido
                                    && a.End.HasValue
                                    && a.End.Value.Date < today);
            }
            return await qq.OrderBy(a => a.Start ?? DateTime.MaxValue).ToListAsync();
        }

        public async Task<Atividade?> FindAsync(int id) => await _db.Atividades.FindAsync(id);

        public async Task<Atividade?> GetWithDependenciesAsync(int id)
            => await _db.Atividades
                         .Include(x => x.Predecessoras).ThenInclude(d => d.Predecessora)
                         .FirstOrDefaultAsync(x => x.Id == id);

        public async Task AddAsync(Atividade atividade)
        {
            _db.Atividades.Add(atividade);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Atividade atividade)
        {
            _db.Atividades.Update(atividade);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Atividade atividade)
        {
            // remove dependency records first so FK won't block
            var deps = await _db.AtividadeDependencias
                                 .Where(d => d.AtividadeId == atividade.Id || d.PredecessoraId == atividade.Id)
                                 .ToListAsync();
            if (deps.Any())
            {
                _db.AtividadeDependencias.RemoveRange(deps);
            }

            _db.Atividades.Remove(atividade);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAllAsync()
        {
            var allDeps = await _db.AtividadeDependencias.ToListAsync();
            if (allDeps.Any())
            {
                _db.AtividadeDependencias.RemoveRange(allDeps);
            }

            var todas = await _db.Atividades.ToListAsync();
            if (todas.Any())
            {
                _db.Atividades.RemoveRange(todas);
            }

            await _db.SaveChangesAsync();
        }

        public async Task<List<Atividade>> GetAllAsync() => await _db.Atividades.AsNoTracking().ToListAsync();
    }
}