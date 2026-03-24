using CutoverPlanner.Domain.Models;
using CutoverPlanner.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace CutoverPlanner.Web.Controllers
{
    public class PlanoController : Controller
    {
        private readonly IPlanoService _planoService;

        public PlanoController(
            IPlanoService planoService)
        {
            _planoService = planoService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _planoService.GetAllAsync();
            return View(list);
        }

        public async Task<IActionResult> Details(int id)
        {
            var plano = await _planoService.GetByIdAsync(id);
            if (plano == null) return NotFound();
            return View(plano);
        }

        /// <summary>
        /// Retorna true se a data for um dia útil (segunda a sexta).
        /// </summary>
        public static bool EhDiaUtil(DateTime data)
        {
            return data.DayOfWeek != DayOfWeek.Saturday && data.DayOfWeek != DayOfWeek.Sunday;
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

        public IActionResult Create()
        {
            var dataHoraToday = DateTime.Today;
            var inicio = ProximoDiaUtil(dataHoraToday).AddHours(8);
            var termino = ProximoDiaUtil(dataHoraToday.AddDays(30)).AddHours(17).AddMinutes(30);

            return View(new Plano() { Inicio = inicio, Termino = termino });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Plano plano)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _planoService.CreateAsync(plano);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao salvar: {ex.Message}");
                }
            }
            return View(plano);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var plano = await _planoService.GetByIdAsync(id);
            if (plano == null) return NotFound();
            return View(plano);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Plano plano)
        {
            if (id != plano.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    await _planoService.UpdateAsync(plano);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao atualizar: {ex.Message}");
                }
            }
            return View(plano);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var plano = await _planoService.GetByIdAsync(id);
            if (plano == null) return NotFound();
            return View(plano);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _planoService.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erro ao excluir: {ex.Message}");
                var plano = await _planoService.GetByIdAsync(id);
                return View(plano);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
