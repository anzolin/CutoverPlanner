using CutoverPlanner.Domain.Models;
using CutoverPlanner.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CutoverPlanner.Web.Controllers
{
    public class ExecutorController : Controller
    {
        private readonly IExecutorService _executorService;
        private readonly IAreaService _areaService;

        public ExecutorController(IExecutorService executorService, IAreaService areaService)
        {
            _executorService = executorService;
            _areaService = areaService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _executorService.GetAllAsync();
            return View(list);
        }

        public async Task<IActionResult> Details(int id)
        {
            var executor = await _executorService.GetByIdAsync(id);
            if (executor == null) return NotFound();
            return View(executor);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Areas = new SelectList(await _areaService.GetAllAsync(), "Id", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Executor executor)
        {
            if (executor.IdArea == 0)
            {
                ModelState.AddModelError(nameof(executor.IdArea), "Selecione uma área");
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    await _executorService.CreateAsync(executor);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao salvar: {ex.Message}");
                }
            }
            ViewBag.Areas = new SelectList(await _areaService.GetAllAsync(), "Id", "Nome", executor.IdArea);
            return View(executor);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var executor = await _executorService.GetByIdAsync(id);
            if (executor == null) return NotFound();
            ViewBag.Areas = new SelectList(await _areaService.GetAllAsync(), "Id", "Nome", executor.IdArea);
            return View(executor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Executor executor)
        {
            if (id != executor.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                await _executorService.UpdateAsync(executor);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Areas = new SelectList(await _areaService.GetAllAsync(), "Id", "Nome", executor.IdArea);
            return View(executor);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var executor = await _executorService.GetByIdAsync(id);
            if (executor == null) return NotFound();
            return View(executor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _executorService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}