using CutoverManager.Application.DTOs;
using CutoverManager.Application.Interfaces;
using CutoverManager.Domain.Entities;
using CutoverManager.Domain.Enums;
using CutoverManager.Domain.Interfaces;
using CutoverManager.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace CutoverManager.Application.Services;

public class AtividadeService : IAtividadeService
{
    private readonly IAtividadeRepository _repo;
    private readonly IPlanoRepository _repoPlano;
    private readonly AtividadeDomainService _domain;

    public AtividadeService(
        IAtividadeRepository repo,
        IPlanoRepository repoPlano,
        AtividadeDomainService domain
    )
    {
        _repo = repo;
        _repoPlano = repoPlano;
        _domain = domain;
    }

    public async Task<IEnumerable<AtividadeDTO>> ListarPorPlanoAsync(int idPlano)
    {
        var atividades = await _repo.ListarPorPlano(idPlano);

        return atividades.Select(a => ToDTO(a));
    }

    public async Task<IEnumerable<AtividadeDTO>> FiltrarAsync(
        int idPlano,
        int? idArea,
        bool? atrasadas,
        int? idMarco,
        int? idSistema,
        StatusAtividade? status)
    {
        var q = _repo.Query()
            .Where(a => a.IdPlano == idPlano);

        if (idArea.HasValue)
            q = q.Where(a => a.Executor.IdArea == idArea.Value);

        if (idMarco.HasValue)
            q = q.Where(a => a.IdMarco == idMarco.Value);

        if (idSistema.HasValue)
            q = q.Where(a => a.IdSistema == idSistema.Value);

        if (status.HasValue)
            q = q.Where(a => a.Status == status.Value);

        if (atrasadas == true)
            q = q.Where(a => a.Termino < DateTime.Now && a.Status != StatusAtividade.Concluido);

        q = q.Include(a => a.Executor).ThenInclude(e => e.Area);

        return await q
            .OrderBy(a => a.Inicio)
            .Select(a => ToDTO(a))
            .ToListAsync();
    }

    public async Task<AtividadeDTO?> ObterPorIdAsync(int id)
    {
        var a = await _repo.GetByIdAsync(id);
        return a == null ? null : ToDTO(a);
    }

    public async Task CriarAsync(AtividadeDTO dto)
    {
        var plano = await _repoPlano.GetByIdAsync(dto.IdPlano)
            ?? throw new Exception("Plano não encontrado.");

        var atividade = FromDTO(dto);

        _domain.ValidarDatas(atividade, plano);

        await _repo.AddAsync(atividade);
    }

    public async Task AtualizarAsync(AtividadeDTO dto)
    {
        var atividade = await _repo.GetByIdAsync(dto.Id)
            ?? throw new Exception("Atividade não encontrada.");

        var plano = await _repoPlano.GetByIdAsync(dto.IdPlano)
            ?? throw new Exception("Plano não encontrado.");

        atividade.IdSistema = dto.IdSistema;
        atividade.IdMarco = dto.IdMarco;
        atividade.IdExecutor = dto.IdExecutor;
        atividade.Titulo = dto.Titulo;
        atividade.Status = dto.Status;
        atividade.RiscoGoLive = dto.RiscoGoLive;
        atividade.Inicio = dto.Inicio;
        atividade.Termino = dto.Termino;
        atividade.Observacao = dto.Observacao;

        _domain.ValidarDatas(atividade, plano);

        await _repo.UpdateAsync(atividade);
    }

    public async Task RemoverAsync(int id)
    {
        var atividade = await _repo.GetByIdAsync(id)
            ?? throw new Exception("Atividade não encontrada.");

        await _repo.DeleteAsync(atividade);
    }

    public async Task CopiarAtividadesAsync(int planoOrigem, int planoDestino)
    {
        var origem = await _repoPlano.GetByIdAsync(planoOrigem)
            ?? throw new Exception("Plano origem não encontrado.");

        var destino = await _repoPlano.GetByIdAsync(planoDestino)
            ?? throw new Exception("Plano destino não encontrado.");

        if (destino.Status != StatusPlano.Ativo)
            throw new Exception("Plano destino deve estar Ativo.");

        var atividadesOrigem = await _repo.ListarPorPlano(planoOrigem);

        foreach (var a in atividadesOrigem)
        {
            var nova = new Atividade
            {
                IdPlano = planoDestino,
                IdSistema = a.IdSistema,
                IdMarco = a.IdMarco,
                IdExecutor = a.IdExecutor,
                Titulo = a.Titulo,
                Status = StatusAtividade.NaoIniciado,
                RiscoGoLive = a.RiscoGoLive,

                // Ajuste automático proporcional ao novo plano
                Inicio = destino.Inicio.Add(a.Inicio - origem.Inicio),
                Termino = destino.Inicio.Add(a.Termino - origem.Inicio),

                Observacao = a.Observacao
            };

            await _repo.AddAsync(nova);
        }
    }

    public async Task RemoverTodasDoPlanoAsync(int idPlano)
    {
        var atividades = await _repo.ListarPorPlano(idPlano);

        foreach (var a in atividades)
            await _repo.DeleteAsync(a);
    }

    public async Task AlterarStatusAsync(int idAtividade, StatusAtividade novoStatus)
    {
        var atividade = await _repo.GetByIdAsync(idAtividade)
            ?? throw new Exception("Atividade não encontrada.");

        atividade.Status = novoStatus;

        await _repo.UpdateAsync(atividade);
    }

    // -----------------------
    // Métodos auxiliares
    // -----------------------

    private static AtividadeDTO ToDTO(Atividade a) =>
        new()
        {
            Id = a.Id,
            IdPlano = a.IdPlano,
            IdSistema = a.IdSistema,
            IdMarco = a.IdMarco,
            IdExecutor = a.IdExecutor,
            ExecutorNome = a.Executor.Nome,
            ExecutorArea = a.Executor.Area.Nome,
            Titulo = a.Titulo,
            Status = a.Status,
            RiscoGoLive = a.RiscoGoLive,
            Inicio = a.Inicio,
            Termino = a.Termino,
            Observacao = a.Observacao
        };

    private static Atividade FromDTO(AtividadeDTO dto) =>
        new()
        {
            Id = dto.Id,
            IdPlano = dto.IdPlano,
            IdSistema = dto.IdSistema,
            IdMarco = dto.IdMarco,
            IdExecutor = dto.IdExecutor,
            Titulo = dto.Titulo,
            Status = dto.Status,
            RiscoGoLive = dto.RiscoGoLive,
            Inicio = dto.Inicio,
            Termino = dto.Termino,
            Observacao = dto.Observacao
        };
}