using CutoverManager.Application.DTOs;
using CutoverManager.Application.Interfaces;
using CutoverManager.Infrastructure.Excel;
using Microsoft.AspNetCore.Mvc;

namespace CutoverManager.Web.Controllers;

public class AtividadeController : Controller
{
    private readonly IAtividadeService _service;
    private readonly IPlanoService _planoService;
    private readonly IAreaService _areaService;
    private readonly IMarcoService _marcoService;
    private readonly ISistemaService _sistemaService;
    private readonly IExecutorService _executorService;

    public AtividadeController(
        IAtividadeService service,
        IPlanoService planoService,
        IAreaService areaService,
        IMarcoService marcoService,
        ISistemaService sistemaService,
        IExecutorService executorService)
    {
        _service = service;
        _planoService = planoService;
        _areaService = areaService;
        _marcoService = marcoService;
        _sistemaService = sistemaService;
        _executorService = executorService;
    }

    public async Task<IActionResult> Index(int? idPlano)
    {
        if (idPlano == null)
        {
            ViewBag.Planos = await _planoService.ListarAsync();

            return View("SelecionarPlano");
        }

        ViewBag.Plano = await _planoService.ObterPorIdAsync(idPlano.Value);
        ViewBag.AreaList = await _areaService.ListarAsync();
        ViewBag.MarcoList = await _marcoService.ListarAsync();
        ViewBag.SistemaList = await _sistemaService.ListarAsync();
        ViewBag.ExecutorList = await _executorService.ListarAsync();

        var lista = await _service.ListarPorPlanoAsync(idPlano.Value);

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

    public async Task<IActionResult> Create(int id)
    {
        var plano = await _planoService.ObterPorIdAsync(id);

        var dataHoraToday = DateTime.Today;
        var inicio = ProximoDiaUtil(plano.Inicio.AddDays(1)).AddHours(8);
        var termino = ProximoDiaUtil(plano.Inicio.AddDays(8)).AddHours(17).AddMinutes(30);

        var dto = new AtividadeDTO { IdPlano = id, Inicio = inicio, Termino = termino };

        ViewBag.SistemaList = await _sistemaService.ListarAsync();
        ViewBag.MarcoList = await _marcoService.ListarAsync();
        ViewBag.ExecutorList = await _executorService.ListarAsync();

        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create(AtividadeDTO dto)
    {
        dto.Id = 0;  // força o EF a entender como nova entidad

        if (!ModelState.IsValid)
        {
            ViewBag.SistemaList = await _sistemaService.ListarAsync();
            ViewBag.MarcoList = await _marcoService.ListarAsync();
            ViewBag.ExecutorList = await _executorService.ListarAsync();

            return View(dto);
        }

        await _service.CriarAsync(dto);

        return RedirectToAction(nameof(Index), new { idPlano = dto.IdPlano });
    }

    public async Task<IActionResult> Edit(int id)
    {
        ViewBag.SistemaList = await _sistemaService.ListarAsync();
        ViewBag.MarcoList = await _marcoService.ListarAsync();
        ViewBag.ExecutorList = await _executorService.ListarAsync();

        var dto = await _service.ObterPorIdAsync(id);

        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(AtividadeDTO dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.SistemaList = await _sistemaService.ListarAsync();
            ViewBag.MarcoList = await _marcoService.ListarAsync();
            ViewBag.ExecutorList = await _executorService.ListarAsync();

            return View(dto);
        }

        await _service.AtualizarAsync(dto);

        return RedirectToAction(nameof(Index), new { idPlano = dto.IdPlano });
    }

    public async Task<IActionResult> Delete(int id)
    {
        var dto = await _service.ObterPorIdAsync(id);
        return View(dto);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirm(int id)
    {
        var dto = await _service.ObterPorIdAsync(id);
        await _service.RemoverAsync(id);

        return RedirectToAction(nameof(Index), new { idPlano = dto!.IdPlano });
    }

    [HttpPost]
    public async Task<IActionResult> ImportarExcel(int idPlano, IFormFile arquivo)
    {
        using var stream = arquivo.OpenReadStream();
        var listaDtos = ExcelImportService.LerAtividades(stream, idPlano);

        foreach (var dto in listaDtos)
            await _service.CriarAsync(dto);

        return RedirectToAction(nameof(Index), new { idPlano });
    }

    public async Task<IActionResult> ExportarExcel(int idPlano)
    {
        var lista = await _service.ListarPorPlanoAsync(idPlano);
        var bytes = ExcelExportService.GerarPlanilha(lista, "Plano");

        return File(bytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"Atividades_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
    }

    public async Task<IActionResult> CopiarAtividades(int origem, int destino)
    {
        await _service.CopiarAtividadesAsync(origem, destino);
        return RedirectToAction(nameof(Index), new { idPlano = destino });
    }

    public async Task<IActionResult> ApagarTodas(int idPlano)
    {
        await _service.RemoverTodasDoPlanoAsync(idPlano);
        return RedirectToAction(nameof(Index), new { idPlano });
    }
}