using CutoverManager.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CutoverManager.Web.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _ctx;

    public HomeController(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<IActionResult> Index()
    {
        // Progresso por status
        var progressoPorStatus = await _ctx.Atividades
            .GroupBy(a => a.Status)
            .Select(g => new { Status = g.Key.ToString(), Total = g.Count() })
            .ToListAsync();

        // Progresso por área
        var progressoPorArea = await _ctx.Atividades
            .Include(a => a.Executor).ThenInclude(e => e.Area)
            .GroupBy(a => a.Executor.Area.Nome)
            .Select(g => new { Area = g.Key, Total = g.Count() })
            .ToListAsync();

        // Progresso por marco
        var progressoPorMarco = await _ctx.Atividades
            .Include(a => a.Marco)
            .GroupBy(a => a.Marco.Nome)
            .Select(g => new { Marco = g.Key, Total = g.Count() })
            .ToListAsync();

        // Atividades com risco de GoLive
        var riscoGoLive = await _ctx.Atividades
            .Where(a => a.RiscoGoLive)
            .Include(a => a.Plano)
            .GroupBy(a => a.Plano.Nome)
            .Select(g => new { Plano = g.Key, Total = g.Count() })
            .ToListAsync();

        ViewBag.Status = progressoPorStatus;
        ViewBag.Area = progressoPorArea;
        ViewBag.Marco = progressoPorMarco;
        ViewBag.Riscos = riscoGoLive;

        return View();
    }
}