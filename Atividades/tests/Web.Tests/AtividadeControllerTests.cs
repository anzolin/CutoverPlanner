using CutoverManager.Application.DTOs;
using CutoverManager.Application.Interfaces;
using CutoverManager.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class AtividadeControllerTests
{
    [Fact]
    public async Task Index_Deve_Retornar_View_SelecionarPlano_Se_IdPlano_Null()
    {
        var service = new Mock<IAtividadeService>();
        var planoService = new Mock<IPlanoService>();
        var areaService = new Mock<IAreaService>();
        var marcoService = new Mock<IMarcoService>();
        var sistemaService = new Mock<ISistemaService>();
        var executorService = new Mock<IExecutorService>();

        planoService.Setup(s => s.ListarAsync())
            .ReturnsAsync(new List<PlanoDTO> { new PlanoDTO { Id = 1, Nome = "P1" } });

        var ctrl = new AtividadeController(service.Object, planoService.Object, 
            areaService.Object, marcoService.Object, sistemaService.Object, 
            executorService.Object);

        var result = await ctrl.Index(null, null, null, null, null, null);

        var view = Assert.IsType<ViewResult>(result);
        Assert.Equal("SelecionarPlano", view.ViewName);
    }

    [Fact]
    public async Task Create_Post_Deve_Redirecionar_Apos_Salvar()
    {
        var service = new Mock<IAtividadeService>();
        var planoService = new Mock<IPlanoService>();
        var areaService = new Mock<IAreaService>();
        var marcoService = new Mock<IMarcoService>();
        var sistemaService = new Mock<ISistemaService>();
        var executorService = new Mock<IExecutorService>();

        var ctrl = new AtividadeController(service.Object, planoService.Object,
            areaService.Object, marcoService.Object, sistemaService.Object,
            executorService.Object);

        var dto = new AtividadeDTO
        {
            IdPlano = 1,
            IdSistema = 1,
            IdMarco = 1,
            IdExecutor = 1,
            Titulo = "Teste",
            Inicio = DateTime.Today,
            Termino = DateTime.Today.AddDays(1)
        };

        var result = await ctrl.Create(dto);

        service.Verify(s => s.CriarAsync(dto), Times.Once);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirect.ActionName);
    }
}