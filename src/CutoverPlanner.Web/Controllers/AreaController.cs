using CutoverPlanner.Domain.Models;
using CutoverPlanner.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace CutoverPlanner.Web.Controllers
{
    public class AreaController : Controller
    {
        private readonly IAreaService _areaService;

        public AreaController(IAreaService areaService)
        {
            _areaService = areaService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _areaService.GetAllAsync();
            return View(list);
        }

        public async Task<IActionResult> Details(int id)
        {
            var area = await _areaService.GetByIdAsync(id);
            if (area == null) return NotFound();
            return View(area);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Area area)
        {
            if (ModelState.IsValid)
            {
                await _areaService.CreateAsync(area);
                return RedirectToAction(nameof(Index));
            }
            return View(area);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var area = await _areaService.GetByIdAsync(id);
            if (area == null) return NotFound();
            return View(area);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Area area)
        {
            if (id != area.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                await _areaService.UpdateAsync(area);
                return RedirectToAction(nameof(Index));
            }
            return View(area);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var area = await _areaService.GetByIdAsync(id);
            if (area == null) return NotFound();
            return View(area);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _areaService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}