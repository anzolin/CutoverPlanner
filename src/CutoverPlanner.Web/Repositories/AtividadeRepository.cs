using CutoverPlanner.Domain.Enumerations;
using CutoverPlanner.Domain.Models;
using CutoverPlanner.Web.Data;
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

        public async Task<List<Atividade>> GetFilteredAsync(string? status, string? sistema, string? area, string? responsavelArea, string? executor, string? busca, bool? atrasadas, bool? riscoGoLive)
        {
            // eagerly load related entities so their names are available in views
            var qq = _db.Atividades
                        .AsNoTracking()
                        .Include(a => a.Executor).ThenInclude(e => e.Area)
                        .Include(a => a.Sistema)
                        .Include(a => a.Marco)
                        .AsQueryable();

            if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<StatusAtividade>(status, out var st))
                qq = qq.Where(a => a.Status == st);
            if (!string.IsNullOrWhiteSpace(sistema)) qq = qq.Where(a => a.Sistema!.Nome.Contains(sistema));
            if (!string.IsNullOrWhiteSpace(area)) qq = qq.Where(a => a.Executor!.Area!.Nome.Contains(area));
            if (!string.IsNullOrWhiteSpace(responsavelArea)) qq = qq.Where(a => a.Executor!.Area!.NomeResponsavel!.Contains(responsavelArea));
            if (!string.IsNullOrWhiteSpace(executor)) qq = qq.Where(a => a.Executor!.Nome.Contains(executor));
            if (!string.IsNullOrWhiteSpace(busca))
                qq = qq.Where(a => (a.Titulo != null && a.Titulo.Contains(busca)) ||
                                   (a.Observacao != null && a.Observacao.Contains(busca)));

            if (atrasadas == true)
            {
                var today = DateTime.Today;
                qq = qq.Where(a => a.Status != StatusAtividade.Concluido
                                    && a.Termino.HasValue
                                    && a.Termino.Value.Date < today);
            }

            if (riscoGoLive == true)
            {
                qq = qq.Where(a => a.RiscoGoLive == true);
            }

            return await qq.OrderBy(a => a.Inicio ?? DateTime.MaxValue).ToListAsync();
        }

        public async Task<Atividade?> FindAsync(int id)
        {
            return await _db.Atividades
                        .Include(a => a.Executor).ThenInclude(e => e.Area)
                        .Include(a => a.Sistema).ThenInclude(e => e.Area)
                        .Include(a => a.Marco)
                        .FirstOrDefaultAsync(a => a.Id == id);
        }

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
            _db.Atividades.Remove(atividade);

            await _db.SaveChangesAsync();
        }

        public async Task DeleteAllAsync()
        {
            var todas = await _db.Atividades.ToListAsync();

            if (todas.Any())
            {
                _db.Atividades.RemoveRange(todas);
            }

            await _db.SaveChangesAsync();
        }

        public async Task<List<Atividade>> GetAllAsync()
        {
            return await _db.Atividades
                        .AsNoTracking()
                        .Include(a => a.Executor).ThenInclude(e => e.Area)
                        .Include(a => a.Sistema)
                        .Include(a => a.Marco)
                        .ToListAsync();
        }
    }
}