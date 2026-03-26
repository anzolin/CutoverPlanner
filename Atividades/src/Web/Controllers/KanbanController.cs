using CutoverManager.Application.DTOs;
using CutoverManager.Application.Interfaces;
using CutoverManager.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CutoverManger.Web.Controllers
{
    public class KanbanController : Controller
    {
        private readonly IAtividadeService _service;

        public KanbanController(IAtividadeService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(int idPlano)
        {
            if (idPlano <= 0)
                return BadRequest("Plano inválido.");

            var atividades = await _service.ListarPorPlanoAsync(idPlano);
            ViewBag.IdPlano = idPlano;

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

            await _service.AlterarStatusAsync(dto.IdAtividade, statusEnum);

            return Ok(new { sucesso = true });
        }
    }
}
