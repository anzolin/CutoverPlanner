using CutoverPlanner.Web.Data;
using CutoverPlanner.Web.Models;
using CutoverPlanner.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;

namespace CutoverPlanner.Web.Controllers
{
    public class AtividadesController : Controller
    {
        private readonly AppDbContext _db;
        private readonly CriticalPathService _cp;
        private readonly IConfiguration _cfg;

        public AtividadesController(AppDbContext db, CriticalPathService cp, IConfiguration cfg)
        { _db = db; _cp = cp; _cfg = cfg; }

        public async Task<IActionResult> Index(string? status, string? sistema, string? area, string? responsavel, string? busca, bool? atrasadas)
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
            if (atrasadas == true)
            {
                var today = DateTime.Today;
                qq = qq.Where(a => a.Status != StatusAtividade.Concluido
                                    && a.End.HasValue
                                    && a.End.Value.Date < today);
            }
            var itens = await qq.OrderBy(a => a.Start ?? DateTime.MaxValue).ToListAsync();
            ViewBag.AtrasadasFilter = atrasadas;
            return View(itens);
        }

        public async Task<IActionResult> Detalhes(int id)
        {
            var a = await _db.Atividades
                .Include(x => x.Predecessoras).ThenInclude(d => d.Predecessora)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (a == null) return NotFound();
            
            // Capture the referrer URL for back button, default to Index
            var backUrl = Request.Headers["Referer"].ToString();
            if (string.IsNullOrWhiteSpace(backUrl))
                backUrl = "/Atividades";
            ViewBag.BackUrl = backUrl;
            
            return View(a);
        }

        [HttpGet]
        public IActionResult Criar() => View(new Atividade());

        [HttpPost]
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Atividade model)
        {
            if (!ModelState.IsValid) return View(model);
            _db.Atividades.Update(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Excluir(int id)
        {
            // remove relationship records first so FK won't block
            var deps = await _db.AtividadeDependencias
                                 .Where(d => d.AtividadeId == id || d.PredecessoraId == id)
                                 .ToListAsync();
            if (deps.Any())
            {
                _db.AtividadeDependencias.RemoveRange(deps);
            }

            var a = await _db.Atividades.FindAsync(id);
            if (a != null)
            {
                _db.Atividades.Remove(a);
            }
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirTodas()
        {
            // delete all dependency records first to avoid FK violations
            var allDeps = await _db.AtividadeDependencias.ToListAsync();
            if (allDeps.Any())
            {
                _db.AtividadeDependencias.RemoveRange(allDeps);
            }

            // then remove all activities
            var todas = await _db.Atividades.ToListAsync();
            if (todas.Any())
            {
                _db.Atividades.RemoveRange(todas);
            }
            await _db.SaveChangesAsync();
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
            // Aplica os mesmos filtros que a ação Index
            var qq = _db.Atividades.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<StatusAtividade>(status, out var st)) qq = qq.Where(a => a.Status == st);
            if (!string.IsNullOrWhiteSpace(sistema)) qq = qq.Where(a => a.Sistema!.Contains(sistema));
            if (!string.IsNullOrWhiteSpace(area)) qq = qq.Where(a => a.AreaExecutoraNome!.Contains(area));
            if (!string.IsNullOrWhiteSpace(responsavel)) qq = qq.Where(a => a.Responsavel!.Contains(responsavel));
            if (!string.IsNullOrWhiteSpace(busca)) qq = qq.Where(a => (a.Titulo != null && a.Titulo.Contains(busca)) || (a.Observacao != null && a.Observacao.Contains(busca)));
            if (atrasadas == true)
            {
                var today = DateTime.Today;
                qq = qq.Where(a => a.Status != StatusAtividade.Concluido
                                    && a.End.HasValue
                                    && a.End.Value.Date < today);
            }
            var itens = await qq.OrderBy(a => a.Start ?? DateTime.MaxValue).ToListAsync();

            // Criar workbook Excel
            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Atividades");
                
                // Cabeçalhos
                ws.Cell(1, 1).Value = "ID";
                ws.Cell(1, 2).Value = "ID Planilha";
                ws.Cell(1, 3).Value = "Sistema";
                ws.Cell(1, 4).Value = "Milestone";
                ws.Cell(1, 5).Value = "Título";
                ws.Cell(1, 6).Value = "Categoria";
                ws.Cell(1, 7).Value = "Requer Teste Perf.";
                ws.Cell(1, 8).Value = "Tipo Teste";
                ws.Cell(1, 9).Value = "Procedimento";
                ws.Cell(1, 10).Value = "Métricas";
                ws.Cell(1, 11).Value = "Critério de Aceite";
                ws.Cell(1, 12).Value = "Evidências";
                ws.Cell(1, 13).Value = "Responsável";
                ws.Cell(1, 14).Value = "Área Executora";
                ws.Cell(1, 15).Value = "Executor";
                ws.Cell(1, 16).Value = "Status";
                ws.Cell(1, 17).Value = "Risco Go-Live";
                ws.Cell(1, 18).Value = "Start";
                ws.Cell(1, 19).Value = "End";
                ws.Cell(1, 20).Value = "Observação";
                ws.Cell(1, 21).Value = "Link Repositório";

                // Formatar cabeçalhos
                var headerRow = ws.Row(1);
                headerRow.Style.Font.Bold = true;
                headerRow.Style.Fill.BackgroundColor = XLColor.FromArgb(0xD3D3D3);

                // Adicionar dados
                int rowNum = 2;
                foreach (var a in itens)
                {
                    ws.Cell(rowNum, 1).Value = a.Id;
                    ws.Cell(rowNum, 2).Value = a.IdPlanilha;
                    ws.Cell(rowNum, 3).Value = a.Sistema ?? "";
                    ws.Cell(rowNum, 4).Value = a.Milestone ?? "";
                    ws.Cell(rowNum, 5).Value = a.Titulo ?? "";
                    ws.Cell(rowNum, 6).Value = a.Categoria ?? "";
                    ws.Cell(rowNum, 7).Value = a.RequerTestePerformance ? "Sim" : "Não";
                    ws.Cell(rowNum, 8).Value = a.TipoTeste ?? "";
                    ws.Cell(rowNum, 9).Value = a.Procedimento ?? "";
                    ws.Cell(rowNum, 10).Value = a.Metricas ?? "";
                    ws.Cell(rowNum, 11).Value = a.CriterioAceite ?? "";
                    ws.Cell(rowNum, 12).Value = a.Evidencias ?? "";
                    ws.Cell(rowNum, 13).Value = a.Responsavel ?? "";
                    ws.Cell(rowNum, 14).Value = a.AreaExecutoraNome ?? "";
                    ws.Cell(rowNum, 15).Value = a.Executor ?? "";
                    ws.Cell(rowNum, 16).Value = a.Status.ToString();
                    ws.Cell(rowNum, 17).Value = a.RiscoGoLive.HasValue ? (a.RiscoGoLive.Value ? "Sim" : "Não") : "";
                    ws.Cell(rowNum, 18).Value = a.Start.HasValue ? a.Start.Value.ToString("dd/MM/yyyy") : "";
                    ws.Cell(rowNum, 19).Value = a.End.HasValue ? a.End.Value.ToString("dd/MM/yyyy") : "";
                    ws.Cell(rowNum, 20).Value = a.Observacao ?? "";
                    ws.Cell(rowNum, 21).Value = a.LinkRepositorio ?? "";
                    rowNum++;
                }

                // Ajustar largura das colunas
                ws.Columns().AdjustToContents();

                // Salvar em memória e retornar como download
                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    var fileName = $"Atividades_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
    }
}
