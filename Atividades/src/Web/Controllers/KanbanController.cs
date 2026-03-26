using CutoverManager.Application.DTOs;
using CutoverManager.Application.Interfaces;
using CutoverManager.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CutoverManager.Web.Controllers;

public class KanbanController : Controller
{
    private readonly IAtividadeService _atividadeService;
    private readonly IPlanoService _planoService;

    public KanbanController(IAtividadeService atividadeService,
                            IPlanoService planoService)
    {
        _atividadeService = atividadeService;
        _planoService = planoService;
    }

    public async Task<IActionResult> Index(int? idPlano)
    {
        // Se nenhum plano foi informado → mostra seletor de plano
        if (idPlano == null)
        {
            ViewBag.Planos = await _planoService.ListarAsync();

            return View("SelecionarPlano");
        }

        ViewBag.IdPlano = idPlano.Value;
        ViewBag.Planos = await _planoService.ListarAsync();

        var atividades = await _atividadeService.ListarPorPlanoAsync(idPlano.Value);

        return View(atividades);
    }

    [HttpPost]
    public async Task<IActionResult> AlterarStatus([FromBody] AlterarStatusDTO dto)
    {
        if (dto == null)
            return BadRequest("Dados inválidos.");

        // Validação básica
        if (dto.IdAtividade <= 0)
            return BadRequest("Atividade inválida.");

        if (!Enum.IsDefined(typeof(StatusAtividade), dto.NovoStatus))
            return BadRequest("Status inválido.");

        var statusEnum = (StatusAtividade)dto.NovoStatus;

        await _atividadeService.AlterarStatusAsync(dto.IdAtividade, statusEnum);

        return Ok(new { sucesso = true });
    }
}