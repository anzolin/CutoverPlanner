using CutoverPlanner.Web.Models;
using CutoverPlanner.Web.Repositories;
using CutoverPlanner.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CutoverPlanner.Web.Controllers
{
    public class AtividadesController : Controller
    {
        private readonly IAtividadeRepository _repo;
        private readonly ICriticalPathService _cp;
        private readonly IConfiguration _cfg;
        private readonly IAtividadeExcelExporter _exporter;

        public AtividadesController(
            IAtividadeRepository repo,
            ICriticalPathService cp,
            IAtividadeExcelExporter exporter,
            IConfiguration cfg)
        {
            _repo = repo;
            _cp = cp;
            _exporter = exporter;
            _cfg = cfg;
        }

        public async Task<IActionResult> Index(string? status, string? sistema, string? area, string? responsavel, string? busca, bool? atrasadas)
        {
            if (Request.Query.Count == 0)
            {
                var defArea = _cfg["DefaultFilter:AreaExecutora"]; // GEAD/OPERAÇÃO DESPACHO
                var defStatusNot = _cfg["DefaultFilter:StatusNot"];  // Concluido
                var itensDef = await _repo.GetFilteredAsync(null, null, defArea, null, null, null);
                
                if (!string.IsNullOrWhiteSpace(defStatusNot) && Enum.TryParse<StatusAtividade>(defStatusNot, out var stn))
                    itensDef = itensDef.Where(a => a.Status != stn).ToList();

                ViewBag.DefaultApplied = true;
                ViewBag.DefArea = defArea;
                ViewBag.DefStatusNot = defStatusNot;

                return View(itensDef);
            }

            ViewBag.AtrasadasFilter = atrasadas;

            var itens = await _repo.GetFilteredAsync(status, sistema, area, responsavel, busca, atrasadas);
            
            return View(itens);
        }

        public async Task<IActionResult> Detalhes(int id)
        {
            var a = await _repo.GetWithDependenciesAsync(id);
            if (a == null) return NotFound();

            var backUrl = Request.Headers["Referer"].ToString();

            if (string.IsNullOrWhiteSpace(backUrl))
                backUrl = "/Atividades";

            ViewBag.BackUrl = backUrl;

            return View(a);
        }

        [HttpGet]
        public IActionResult Criar() => View(new Atividade() { Start = DateTime.Now, End = DateTime.Now.AddDays(7) });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar(Atividade model)
        {
            if (!ModelState.IsValid) return View(model);
            
            await _repo.AddAsync(model);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var a = await _repo.FindAsync(id);

            return a == null ? NotFound() : View(a);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Atividade model)
        {
            if (!ModelState.IsValid) return View(model);

            await _repo.UpdateAsync(model);
            
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Excluir(int id)
        {
            var a = await _repo.FindAsync(id);

            if (a != null)
            {
                await _repo.DeleteAsync(a);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirTodas()
        {
            await _repo.DeleteAllAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> CaminhoCritico()
        {
            var (path, total) = await _cp.GetCriticalPathAsync();
            ViewBag.Total = total;
            return View(path);
        }

        [HttpGet]
        public async Task<IActionResult> ExportarExcel(string? status, string? sistema, string? area, string? responsavel, string? busca, bool? atrasadas)
        {
            var itens = await _repo.GetFilteredAsync(status, sistema, area, responsavel, busca, atrasadas);
            var (content, fileName) = await _exporter.ExportAsync(itens);
            
            return File(content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName);
        }
    }
}
