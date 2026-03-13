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

        public AtividadeController(
            IAtividadeService atividadeService,
            IExecutorService executorService,
            ISistemaService sistemaService,
            IMarcoService marcoService)
        {
            _atividadeService = atividadeService;
            _executorService = executorService;
            _sistemaService = sistemaService;
            _marcoService = marcoService;
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

        public async Task<IActionResult> Create()
        {
            await PopulateDropDowns();

            var dataHoraToday = DateTime.Today;
            var inicio = dataHoraToday.AddDays(1).AddHours(8);
            var termino = dataHoraToday.AddDays(8).AddHours(17).AddMinutes(30);

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

        private async Task PopulateDropDowns(Atividade? atividade = null)
        {
            ViewBag.Sistemas = new SelectList(await _sistemaService.GetAllAsync(), "Id", "Nome", atividade?.IdSistema);
            ViewBag.Executores = new SelectList(await _executorService.GetAllAsync(), "Id", "Nome", atividade?.IdExecutor);
            ViewBag.Marcos = new SelectList(await _marcoService.GetAllAsync(), "Id", "Nome", atividade?.IdMarco);
        }
    }
}