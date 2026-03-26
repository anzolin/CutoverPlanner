using CutoverManager.Application.DTOs;
using CutoverManager.Application.Services;
using CutoverManager.Domain.Entities;
using CutoverManager.Domain.Interfaces;
using CutoverManager.Domain.Services;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

public class AtividadeServiceTests
{
    [Fact]
    public async Task Deve_Criar_Atividade_Valida()
    {
        var repoAtv = new Mock<IAtividadeRepository>();
        var repoPlano = new Mock<IPlanoRepository>();
        var dominio = new AtividadeDomainService();

        repoPlano.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Plano
        {
            Id = 1,
            Inicio = DateTime.Today,
            Termino = DateTime.Today.AddDays(5)
        });

        var service = new AtividadeService(repoAtv.Object, repoPlano.Object, dominio);

        var dto = new AtividadeDTO
        {
            IdPlano = 1,
            IdSistema = 1,
            IdMarco = 1,
            IdExecutor = 1,
            Titulo = "Teste",
            Inicio = DateTime.Today,
            Termino = DateTime.Today.AddDays(1),
            RiscoGoLive = false
        };

        await service.CriarAsync(dto);

        repoAtv.Verify(r => r.AddAsync(It.IsAny<Atividade>()), Times.Once);
    }

    [Fact]
    public async Task Deve_Recusar_Atividade_Fora_Do_Plano()
    {
        var repoAtv = new Mock<IAtividadeRepository>();
        var repoPlano = new Mock<IPlanoRepository>();
        var dominio = new AtividadeDomainService();

        repoPlano.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Plano
        {
            Id = 1,
            Inicio = DateTime.Today,
            Termino = DateTime.Today.AddDays(5)
        });

        var service = new AtividadeService(repoAtv.Object, repoPlano.Object, dominio);

        var dto = new AtividadeDTO
        {
            IdPlano = 1,
            IdSistema = 1,
            IdMarco = 1,
            IdExecutor = 1,
            Titulo = "Invalida",
            Inicio = DateTime.Today.AddDays(10),
            Termino = DateTime.Today.AddDays(11)
        };

        await Assert.ThrowsAsync<Exception>(() => service.CriarAsync(dto));
    }
}