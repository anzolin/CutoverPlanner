using CutoverManager.Domain.Entities;
using CutoverManager.Domain.Services;
using System;
using Xunit;

public class AtividadeDomainServiceTests
{
    [Fact]
    public void Deve_Aceitar_Atividade_Com_Datas_Validas()
    {
        var dominio = new AtividadeDomainService();

        var plano = new Plano
        {
            Inicio = new DateTime(2026, 1, 1),
            Termino = new DateTime(2026, 1, 31)
        };

        var atividade = new Atividade
        {
            Inicio = new DateTime(2026, 1, 5),
            Termino = new DateTime(2026, 1, 10)
        };

        var ex = Record.Exception(() => dominio.ValidarDatas(atividade, plano));
        Assert.Null(ex);
    }

    [Fact]
    public void Deve_Recusar_Atividade_Fora_Do_Plano()
    {
        var dominio = new AtividadeDomainService();

        var plano = new Plano
        {
            Inicio = new DateTime(2026, 1, 1),
            Termino = new DateTime(2026, 1, 31)
        };

        var atividade = new Atividade
        {
            Inicio = new DateTime(2025, 12, 20),
            Termino = new DateTime(2026, 1, 5)
        };

        Assert.Throws<Exception>(() =>
            dominio.ValidarDatas(atividade, plano));
    }

    [Fact]
    public void Deve_Recusar_Atividade_Com_Data_Inicio_Maior_Que_Termino()
    {
        var dominio = new AtividadeDomainService();

        var plano = new Plano
        {
            Inicio = DateTime.Today,
            Termino = DateTime.Today.AddDays(10)
        };

        var atividade = new Atividade
        {
            Inicio = DateTime.Today.AddDays(5),
            Termino = DateTime.Today.AddDays(1)
        };

        Assert.Throws<Exception>(() =>
            dominio.ValidarDatas(atividade, plano));
    }
}