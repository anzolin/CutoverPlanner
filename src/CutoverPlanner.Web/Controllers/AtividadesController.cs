
using CutoverPlanner.Web.Data;
using CutoverPlanner.Web.Models;
using CutoverPlanner.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CutoverPlanner.Web.Controllers
{
    public class AtividadesController : Controller
    {
        private readonly AppDbContext _db;
        private readonly CriticalPathService _cp;
        private readonly IConfiguration _cfg;

        public AtividadesController(AppDbContext db, CriticalPathService cp, IConfiguration cfg)
        { _db = db; _cp = cp; _cfg = cfg; }

        public async Task<IActionResult> Index(string? status, string? sistema, string? area, string? responsavel, string? busca)
        {
            // Aplica filtro padrão quando nenhum parâmetro é enviado
            if (Request.Query.Count == 0)
            {
                var defArea = _cfg["DefaultFilter:AreaExecutora"]; // GEAD/OPERAÇÃO DESPACHO
                var defStatusNot = _cfg["DefaultFilter:StatusNot"];  // Concluido
                var q = _db.Atividades.AsNoTracking().AsQueryable();
                if (!string.IsNullOrWhiteSpace(defArea)) q = q.Where(a => a.AreaExecutoraNome != null && a.AreaExecutoraNome.Contains(defArea));
                if (!string.IsNullOrWhiteSpace(defStatusNot) && Enum.TryParse<StatusAtividade>(defStatusNot, out var stn)) q = q.Where(a => a.Status != stn);
                var itensDef = await q.OrderBy(a => a.Start ?? DateTime.MaxValue).ToListAsync();
                ViewBag.DefaultApplied = true; ViewBag.DefArea = defArea; ViewBag.DefStatusNot = defStatusNot;
                return View(itensDef);
            }

            var qq = _db.Atividades.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<StatusAtividade>(status, out var st)) qq = qq.Where(a => a.Status == st);
            if (!string.IsNullOrWhiteSpace(sistema)) qq = qq.Where(a => a.Sistema!.Contains(sistema));
            if (!string.IsNullOrWhiteSpace(area)) qq = qq.Where(a => a.AreaExecutoraNome!.Contains(area));
            if (!string.IsNullOrWhiteSpace(responsavel)) qq = qq.Where(a => a.Responsavel!.Contains(responsavel));
            if (!string.IsNullOrWhiteSpace(busca)) qq = qq.Where(a => (a.Titulo != null && a.Titulo.Contains(busca)) || (a.Observacao != null && a.Observacao.Contains(busca)));
            var itens = await qq.OrderBy(a => a.Start ?? DateTime.MaxValue).ToListAsync();
            return View(itens);
        }

        public async Task<IActionResult> Detalhes(int id)
        {
            var a = await _db.Atividades
                .Include(x => x.Predecessoras).ThenInclude(d => d.Predecessora)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (a == null) return NotFound();
            return View(a);
        }

        [HttpGet]
        public IActionResult Criar() => View(new Atividade());

        [HttpPost]
        public async Task<IActionResult> Criar(Atividade model)
        {
            if (!ModelState.IsValid) return View(model);
            _db.Atividades.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var a = await _db.Atividades.FindAsync(id);
            return a == null ? NotFound() : View(a);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Atividade model)
        {
            if (!ModelState.IsValid) return View(model);
            _db.Atividades.Update(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Excluir(int id)
        {
            var a = await _db.Atividades.FindAsync(id);
            if (a != null)
            {
                _db.Atividades.Remove(a);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> CaminhoCritico()
        {
            var (path, total) = await _cp.GetCriticalPathAsync();
            ViewBag.Total = total;
            return View(path);
        }
    }
}
