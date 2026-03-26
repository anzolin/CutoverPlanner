using CutoverManager.Application.DTOs;
using CutoverManager.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CutoverManager.Web.Controllers;

public class AreaController : Controller
{
    private readonly IAreaService _service;

    public AreaController(IAreaService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index()
    {
        var lista = await _service.ListarAsync();
        return View(lista);
    }

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(AreaDTO dto)
    {
        dto.Id = 0;  // força o EF a entender como nova entidad

        if (!ModelState.IsValid)
            return View(dto);

        await _service.CriarAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var dto = await _service.ObterPorIdAsync(id);
        if (dto == null) return NotFound();
        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(AreaDTO dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

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