using CutoverManager.Application.DTOs;
using CutoverManager.Application.Services;
using CutoverManager.Domain.Entities;
using CutoverManager.Domain.Interfaces;
using CutoverManager.Domain.Services;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

public class PlanoServiceTests
{
    [Fact]
    public async Task Deve_Salvar_Plano_Valido()
    {
        var repo = new Mock<IPlanoRepository>();
        var dominio = new PlanoDomainService();

        var service = new PlanoService(repo.Object, dominio);

        var dto = new PlanoDTO
        {
            Nome = "Plano Teste",
            Inicio = new DateTime(2026, 1, 10),
            Termino = new DateTime(2026, 1, 15),
            Status = CutoverManager.Domain.Enums.StatusPlano.Ativo
        };

        await service.CriarAsync(dto);

        repo.Verify(r => r.AddAsync(It.IsAny<Plano>()), Times.Once);
    }

    [Fact]
    public async Task Nao_Deve_Salvar_Plano_Com_Datas_Invalidas()
    {
        var repo = new Mock<IPlanoRepository>();
        var dominio = new PlanoDomainService();

        var service = new PlanoService(repo.Object, dominio);

        var dto = new PlanoDTO
        {
            Nome = "Plano com erro",
            Inicio = new DateTime(2026, 3, 10),
            Termino = new DateTime(2026, 3, 1),
        };

        await Assert.ThrowsAsync<Exception>(() =>
            service.CriarAsync(dto));
    }
}