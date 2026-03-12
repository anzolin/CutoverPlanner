using CutoverPlanner.Domain.Models;
using CutoverPlanner.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace CutoverPlanner.Web.Controllers
{
    public class MarcoController : Controller
    {
        private readonly IMarcoService _marcoService;

        public MarcoController(IMarcoService marcoService)
        {
            _marcoService = marcoService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _marcoService.GetAllAsync();
            return View(list);
        }

        public async Task<IActionResult> Details(int id)
        {
            var marco = await _marcoService.GetByIdAsync(id);
            if (marco == null) return NotFound();
            return View(marco);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Marco marco)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _marcoService.CreateAsync(marco);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao salvar: {ex.Message}");
                }
            }
            return View(marco);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var marco = await _marcoService.GetByIdAsync(id);
            if (marco == null) return NotFound();
            return View(marco);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Marco marco)
        {
            if (id != marco.Id) return BadRequest();
            
            if (ModelState.IsValid)
            {
                try
                {
                    await _marcoService.UpdateAsync(marco);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao atualizar: {ex.Message}");
                }
            }
            return View(marco);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var marco = await _marcoService.GetByIdAsync(id);
            if (marco == null) return NotFound();
            return View(marco);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _marcoService.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erro ao excluir: {ex.Message}");
                var marco = await _marcoService.GetByIdAsync(id);
                return View(marco);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}