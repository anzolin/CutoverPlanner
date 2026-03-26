using CutoverManager.Application.DTOs;
using CutoverManager.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CutoverManager.Web.Controllers;

public class ExecutorController : Controller
{
    private readonly IExecutorService _service;
    private readonly IAreaService _areaService;

    public ExecutorController(IExecutorService service, IAreaService areaService)
    {
        _service = service;
        _areaService = areaService;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.Areas = await _areaService.ListarAsync();

        var lista = await _service.ListarAsync();
        return View(lista);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Areas = await _areaService.ListarAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ExecutorDTO dto)
    {
        dto.Id = 0;  // força o EF a entender como nova entidad

        if (!ModelState.IsValid)
        {
            ViewBag.Areas = await _areaService.ListarAsync();
            return View(dto);
        }

        await _service.CriarAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        ViewBag.Areas = await _areaService.ListarAsync();
        var dto = await _service.ObterPorIdAsync(id);
        if (dto == null) return NotFound();
        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ExecutorDTO dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Areas = await _areaService.ListarAsync();
            return View(dto);
        }

        await _service.AtualizarAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var dto = await _service.ObterPorIdAsync(id);
        if (dto == null) return NotFound();
        return View(dto);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirm(int id)
    {
        await _service.RemoverAsync(id);
        return RedirectToAction(nameof(Index));
    }
}