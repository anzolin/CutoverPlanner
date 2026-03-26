using CutoverManager.Application.DTOs;
using CutoverManager.Application.Interfaces;
using CutoverManager.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class PlanoControllerTests
{
    [Fact]
    public async Task Index_Deve_Retornar_View_Com_Lista()
    {
        var mock = new Mock<IPlanoService>();
        mock.Setup(s => s.FiltrarAsync(null))
            .ReturnsAsync(new List<PlanoDTO>());

        var ctrl = new PlanoController(mock.Object);

        var result = await ctrl.Index(null);

        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Create_Post_Deve_Salvar_E_Redirecionar()
    {
        var mock = new Mock<IPlanoService>();
        var ctrl = new PlanoController(mock.Object);

        var dto = new PlanoDTO
        {
            Nome = "Plano X",
            Inicio = DateTime.Today,
            Termino = DateTime.Today.AddDays(1)
        };

        var result = await ctrl.Create(dto);

        mock.Verify(s => s.CriarAsync(dto), Times.Once);
        Assert.IsType<RedirectToActionResult>(result);
    }
}