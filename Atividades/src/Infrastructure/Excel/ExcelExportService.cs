using ClosedXML.Excel;
using CutoverManager.Application.DTOs;

namespace CutoverManager.Infrastructure.Excel;

public static class ExcelExportService
{
    public static byte[] GerarPlanilha(IEnumerable<AtividadeDTO> atividades, string nomePlano)
    {
        using var wb = new XLWorkbook();
        var ws = wb.AddWorksheet("Atividades");

        ws.Cell(1, 1).Value = "Título";
        ws.Cell(1, 2).Value = "Sistema";
        ws.Cell(1, 3).Value = "Marco";
        ws.Cell(1, 4).Value = "Executor";
        ws.Cell(1, 5).Value = "Risco GoLive";
        ws.Cell(1, 6).Value = "Início";
        ws.Cell(1, 7).Value = "Término";
        ws.Cell(1, 8).Value = "Observação";

        int row = 2;

        foreach (var a in atividades)
        {
            ws.Cell(row, 1).Value = a.Titulo;
            ws.Cell(row, 2).Value = a.IdSistema;
            ws.Cell(row, 3).Value = a.IdMarco;
            ws.Cell(row, 4).Value = a.IdExecutor;
            ws.Cell(row, 5).Value = a.RiscoGoLive ? "SIM" : "NÃO";
            ws.Cell(row, 6).Value = a.Inicio;
            ws.Cell(row, 7).Value = a.Termino;
            ws.Cell(row, 8).Value = a.Observacao;

            row++;
        }

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return ms.ToArray();
    }
}