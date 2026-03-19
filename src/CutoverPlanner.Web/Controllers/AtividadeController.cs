using ClosedXML.Excel;
using CutoverPlanner.Domain.Models;
using CutoverPlanner.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CutoverPlanner.Web.Controllers
{
    public class AtividadeController : Controller
    {
        private readonly IAtividadeService _atividadeService;
        private readonly IExecutorService _executorService;
        private readonly ISistemaService _sistemaService;
        private readonly IMarcoService _marcoService;
        private readonly ExcelImportService _import;

        public AtividadeController(
            IAtividadeService atividadeService,
            IExecutorService executorService,
            ISistemaService sistemaService,
            IMarcoService marcoService,
            ExcelImportService import)
        {
            _atividadeService = atividadeService;
            _executorService = executorService;
            _sistemaService = sistemaService;
            _marcoService = marcoService;
            _import = import;
        }

        public async Task<IActionResult> Index(
            string? status,
            string? sistema,
            string? area,
            string? responsavelArea,
            string? executor,
            string? busca,
            bool? atrasadas,
            bool? riscoGoLive)
        {
            var list = await _atividadeService.GetFilteredAsync(status, sistema, area, responsavelArea, executor, busca, atrasadas, riscoGoLive);

            return View(list);
        }

        public async Task<IActionResult> Details(int id)
        {
            var atividade = await _atividadeService.GetByIdAsync(id);
            if (atividade == null) return NotFound();

            return View(atividade);
        }

        /// <summary>
        /// Retorna true se a data for um dia útil (segunda a sexta).
        /// </summary>
        public static bool EhDiaUtil(DateTime data)
        {
            return data.DayOfWeek != DayOfWeek.Saturday &&
                data.DayOfWeek != DayOfWeek.Sunday;
        }

        /// <summary>
        /// Se a data não for dia útil, incrementa até encontrar o próximo dia útil.
        /// Se já for dia útil, retorna a própria data.
        /// </summary>
        public static DateTime ProximoDiaUtil(DateTime data)
        {
            var d = data.Date;
            while (!EhDiaUtil(d))
            {
                d = d.AddDays(1);
            }
            return d;
        }

        public async Task<IActionResult> Create()
        {
            await PopulateDropDowns();

            var dataHoraToday = DateTime.Today;
            //var inicio = dataHoraToday.AddDays(1).AddHours(8);
            var inicio = ProximoDiaUtil(dataHoraToday.AddDays(1)).AddHours(8);
            //var termino = dataHoraToday.AddDays(8).AddHours(17).AddMinutes(30);
            var termino = ProximoDiaUtil(dataHoraToday.AddDays(8)).AddHours(17).AddMinutes(30);

            // default status should use the existing enum values
            return View(new Atividade() { Status = Domain.Enumerations.StatusAtividade.NaoIniciado, Inicio = inicio, Termino = termino });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Atividade atividade)
        {
            if (ModelState.IsValid)
            {
                await _atividadeService.CreateAsync(atividade);

                return RedirectToAction(nameof(Index));
            }

            await PopulateDropDowns(atividade);

            return View(atividade);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var atividade = await _atividadeService.GetByIdAsync(id);

            if (atividade == null) return NotFound();

            await PopulateDropDowns(atividade);

            return View(atividade);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Atividade atividade)
        {
            if (id != atividade.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                await _atividadeService.UpdateAsync(atividade);
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropDowns(atividade);

            return View(atividade);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var atividade = await _atividadeService.GetByIdAsync(id);

            if (atividade == null) return NotFound();

            return View(atividade);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var atividade = await _atividadeService.GetByIdAsync(id);

            if (atividade != null)
            {
                await _atividadeService.DeleteAsync(atividade);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirTodas()
        {
            // then remove all activities
            await _atividadeService.DeleteAllAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult ImportarExcel() => View();

        [HttpPost]
        [RequestSizeLimit(104857600)]
        public async Task<IActionResult> ImportarExcelUpload()
        {
            //var file = Request.Form.Files.FirstOrDefault();

            //if (file == null || file.Length == 0)
            //{
            //    TempData["Error"] = "Selecione um arquivo .xlsx.";

            //    return RedirectToAction(nameof(Index));
            //}

            //using var ms = new MemoryStream();
            //await file.CopyToAsync(ms);
            //ms.Position = 0;
            //var (ativ, deps, areas, endpoints) = await _import.ImportAsync(ms);
            //TempData["Msg"] = $"Importação concluída: {ativ} atividades, {deps} dependências, {areas} áreas, {endpoints} endpoints.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ExportarExcel(
            string? status,
            string? sistema,
            string? area,
            string? responsavelArea,
            string? executor,
            string? busca,
            bool? atrasadas,
            bool? riscoGoLive)
        {
            var itens = await _atividadeService.GetFilteredAsync(status, sistema, area, responsavelArea, executor, busca, atrasadas, riscoGoLive);

            // Criar workbook Excel
            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Atividades");

                // Cabeçalhos
                ws.Cell(1, 1).Value = "ID";
                ws.Cell(1, 2).Value = "Marco";
                ws.Cell(1, 3).Value = "Sistema";
                ws.Cell(1, 4).Value = "Título";
                ws.Cell(1, 5).Value = "Executor";
                ws.Cell(1, 6).Value = "Área";
                ws.Cell(1, 7).Value = "Responsável";
                ws.Cell(1, 8).Value = "Status";
                ws.Cell(1, 9).Value = "Início";
                ws.Cell(1, 10).Value = "Término";
                ws.Cell(1, 11).Value = "Risco GoLive";
                ws.Cell(1, 12).Value = "Observação";

                // Formatar cabeçalhos
                var headerRow = ws.Row(1);
                headerRow.Style.Font.Bold = true;
                headerRow.Style.Fill.BackgroundColor = XLColor.FromArgb(0xD3D3D3);

                // Adicionar dados
                int rowNum = 2;
                foreach (var a in itens)
                {
                    ws.Cell(rowNum, 1).Value = a.Id;
                    ws.Cell(rowNum, 2).Value = a.Marco?.Nome;
                    ws.Cell(rowNum, 3).Value = a.Sistema?.Nome;
                    ws.Cell(rowNum, 4).Value = a.Titulo;
                    ws.Cell(rowNum, 5).Value = a.Executor?.Nome;
                    ws.Cell(rowNum, 6).Value = a.Executor?.Area?.Nome;
                    ws.Cell(rowNum, 7).Value = a.Executor?.Area?.NomeResponsavel;
                    ws.Cell(rowNum, 8).Value = a.Status.ToString();
                    ws.Cell(rowNum, 9).Value = a.Inicio;
                    ws.Cell(rowNum, 10).Value = a.Termino;
                    ws.Cell(rowNum, 11).Value = a.RiscoGoLive ? "Sim" : "Não";
                    ws.Cell(rowNum, 12).Value = a.Observacao ?? "";
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

        private async Task PopulateDropDowns(Atividade? atividade = null)
        {
            ViewBag.Sistemas = new SelectList(await _sistemaService.GetAllAsync(), "Id", "Nome", atividade?.IdSistema, "Area.Nome");
            ViewBag.Executores = new SelectList(await _executorService.GetAllAsync(), "Id", "Nome", atividade?.IdExecutor, "Area.Nome");
            ViewBag.Marcos = new SelectList(await _marcoService.GetAllAsync(), "Id", "Nome", atividade?.IdMarco);
        }
    }
}