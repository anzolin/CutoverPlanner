
using CutoverPlanner.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace CutoverPlanner.Web.Controllers
{
    public class ImportController : Controller
    {
        private readonly ExcelImportService _import;
        public ImportController(ExcelImportService import) => _import = import;

        [HttpGet]
        public IActionResult Index() => View();

        [HttpPost]
        [RequestSizeLimit(104857600)]
        public async Task<IActionResult> Upload()
        {
            var file = Request.Form.Files.FirstOrDefault();
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Selecione um arquivo .xlsx.";
                return RedirectToAction(nameof(Index));
            }
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            ms.Position = 0;
            var (ativ, deps, areas, endpoints) = await _import.ImportAsync(ms);
            TempData["Msg"] = $"Importação concluída: {ativ} atividades, {deps} dependências, {areas} áreas, {endpoints} endpoints.";
            return RedirectToAction(nameof(Index));
        }
    }
}
