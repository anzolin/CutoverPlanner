using CutoverManager.Domain.Entities;
using CutoverManager.Domain.Services;
using System;
using Xunit;

public class PlanoDomainServiceTests
{
    [Fact]
    public void Deve_Aceitar_Plano_Valido()
    {
        var dominio = new PlanoDomainService();
        var p = new Plano
        {
            Nome = "Plano 1",
            Inicio = DateTime.Today,
            Termino = DateTime.Today.AddDays(2)
        };

        var ex = Record.Exception(() => dominio.ValidarPeriodo(p));
        Assert.Null(ex);
    }

    [Fact]
    public void Deve_Recusar_Plano_Com_Inicio_Maior_Que_Termino()
    {
        var dominio = new PlanoDomainService();
        var p = new Plano
        {
            Nome = "Plano inválido",
            Inicio = DateTime.Today.AddDays(5),
            Termino = DateTime.Today
        };

        Assert.Throws<Exception>(() => dominio.ValidarPeriodo(p));
    }
}