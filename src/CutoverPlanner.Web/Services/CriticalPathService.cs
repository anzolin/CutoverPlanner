
using CutoverPlanner.Web.Data;
using CutoverPlanner.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace CutoverPlanner.Web.Services
{
    public class CriticalPathService
    {
        private readonly AppDbContext _db;
        public CriticalPathService(AppDbContext db) => _db = db;

        public async Task<(IReadOnlyList<Atividade> path, TimeSpan totalDur)> GetCriticalPathAsync()
        {
            var atividades = await _db.Atividades
                .Include(a => a.Predecessoras)
                .AsNoTracking()
                .ToListAsync();

            var byId = atividades.ToDictionary(a => a.Id);
            var dur = atividades.ToDictionary(a => a.Id, a => (a.Start.HasValue && a.End.HasValue) ? a.End.Value - a.Start.Value : TimeSpan.Zero);
            var memoLen = new Dictionary<int, TimeSpan>();
            var memoNext = new Dictionary<int, int?>();

            TimeSpan Dfs(int id)
            {
                if (memoLen.TryGetValue(id, out var v)) return v;
                var preds = _db.AtividadeDependencias.AsNoTracking().Where(p => p.AtividadeId == id).Select(p => p.PredecessoraId).ToList();
                if (preds.Count == 0)
                {
                    memoLen[id] = dur[id];
                    memoNext[id] = null;
                    return memoLen[id];
                }
                TimeSpan best = TimeSpan.Zero; int? bestPred = null;
                foreach (var predId in preds)
                {
                    var len = Dfs(predId);
                    if (len > best) { best = len; bestPred = predId; }
                }
                memoLen[id] = best + dur[id];
                memoNext[id] = bestPred;
                return memoLen[id];
            }

            TimeSpan bestTotal = TimeSpan.Zero; int? bestId = null;
            foreach (var a in atividades)
            {
                var len = Dfs(a.Id);
                if (len > bestTotal) { bestTotal = len; bestId = a.Id; }
            }

            var path = new List<Atividade>();
            while (bestId.HasValue)
            {
                path.Add(byId[bestId.Value]);
                bestId = memoNext[bestId.Value];
            }
            path.Reverse();
            return (path, bestTotal);
        }
    }
}
