using CutoverPlanner.Domain.Models;
using CutoverPlanner.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CutoverPlanner.Web.Controllers
{
    public class SistemaController : Controller
    {
        private readonly ISistemaService _sistemaService;
        private readonly IAreaService _areaService;

        public SistemaController(ISistemaService sistemaService, IAreaService areaService)
        {
            _sistemaService = sistemaService;
            _areaService = areaService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _sistemaService.GetAllAsync();
            return View(list);
        }

        public async Task<IActionResult> Details(int id)
        {
            var sistema = await _sistemaService.GetByIdAsync(id);
            if (sistema == null) return NotFound();
            return View(sistema);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Areas = new SelectList(await _areaService.GetAllAsync(), "Id", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sistema sistema)
        {
            if (sistema.IdArea == 0)
            {
                ModelState.AddModelError(nameof(sistema.IdArea), "Selecione uma área");
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    await _sistemaService.CreateAsync(sistema);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao salvar: {ex.Message}");
                }
            }
            ViewBag.Areas = new SelectList(await _areaService.GetAllAsync(), "Id", "Nome", sistema.IdArea);
            return View(sistema);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var sistema = await _sistemaService.GetByIdAsync(id);
            if (sistema == null) return NotFound();
            ViewBag.Areas = new SelectList(await _areaService.GetAllAsync(), "Id", "Nome", sistema.IdArea);
            return View(sistema);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Sistema sistema)
        {
            if (id != sistema.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                await _sistemaService.UpdateAsync(sistema);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Areas = new SelectList(await _areaService.GetAllAsync(), "Id", "Nome", sistema.IdArea);
            return View(sistema);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var sistema = await _sistemaService.GetByIdAsync(id);
            if (sistema == null) return NotFound();
            return View(sistema);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _sistemaService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}