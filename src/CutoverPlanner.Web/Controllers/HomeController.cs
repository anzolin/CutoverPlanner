
using CutoverPlanner.Web.Data;
using Microsoft.AspNetCore.Mvc;
using CutoverPlanner.Web.Models;
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
            var riscos = _db.Atividades.Where(a => a.RiscoGoLive == true).OrderBy(a => a.Start).Take(10).ToList();
            ViewBag.Total = total; ViewBag.Concl = concl; ViewBag.EmAnd = emAnd; ViewBag.NaoIni = naoIni; ViewBag.Riscos = riscos; ViewBag.Pct = total>0 ? (int)(concl*100.0/total) : 0;
            return View();
        }
    }
}
