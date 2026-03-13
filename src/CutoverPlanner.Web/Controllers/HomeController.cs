using CutoverPlanner.Domain.Enumerations;
using CutoverPlanner.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CutoverPlanner.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        public HomeController(AppDbContext db) => _db = db;
        public IActionResult Index()
        {
            var total = _db.Atividades.Count();
            var concl = _db.Atividades.Count(a => a.Status == StatusAtividade.Concluido);
            var emAnd = _db.Atividades.Count(a => a.Status == StatusAtividade.EmAndamento);
            var naoIni = total - concl - emAnd;
            var riscos = _db.Atividades
                .Where(a => a.RiscoGoLive == true && a.Status != StatusAtividade.Concluido)
                .Include(i => i.Sistema)
                .Include(i => i.Executor)
                .OrderBy(a => a.Inicio)
                .Take(10).ToList();

            // distribuição por área e por marco
            var distArea = _db.Atividades
                             .Include(a => a.Executor).ThenInclude(e => e.Area)
                             .Where(a => a.Executor != null && a.Executor.Area != null)
                             .GroupBy(a => a.Executor.Area!.Nome)
                             .Select(g => new { Name = g.Key, Count = g.Count() })
                             .ToList();
            var distMarco = _db.Atividades
                              .Include(a => a.Marco)
                              .Where(a => a.Marco != null)
                              .GroupBy(a => a.Marco!.Nome)
                              .Select(g => new { Name = g.Key, Count = g.Count() })
                              .ToList();

            ViewBag.Total = total;
            ViewBag.Concl = concl;
            ViewBag.EmAnd = emAnd;
            ViewBag.NaoIni = naoIni;
            ViewBag.Riscos = riscos;
            ViewBag.Pct = total>0 ? (int)(concl*100.0/total) : 0;
            ViewBag.DistArea = distArea;
            ViewBag.DistMarco = distMarco;

            return View();
        }
    }
}
