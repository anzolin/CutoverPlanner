using CutoverManager.Application.DTOs;
using CutoverManager.Application.Interfaces;
using Humanizer;
using Microsoft.AspNetCore.Mvc;

namespace CutoverManager.Web.Controllers;

public class PlanoController : Controller
{
    private readonly IPlanoService _service;

    public PlanoController(IPlanoService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index(string? nome)
    {
        var lista = await _service.FiltrarAsync(nome);
        return View(lista);
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

        return View(new PlanoDTO() { Inicio = inicio, Termino = termino });
    }

    [HttpPost]
    public async Task<IActionResult> Create(PlanoDTO dto)
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
    public async Task<IActionResult> Edit(PlanoDTO dto)
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