using ClosedXML.Excel;
using CutoverPlanner.Web.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CutoverPlanner.Web.Services
{
    public class AtividadeExcelExporter : IAtividadeExcelExporter
    {
        public Task<(byte[] content, string fileName)> ExportAsync(IEnumerable<Atividade> itens)
        {
            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Atividades");

            // Cabeçalhos
            ws.Cell(1, 1).Value = "ID";
            ws.Cell(1, 2).Value = "ID Planilha";
            ws.Cell(1, 3).Value = "Sistema";
            ws.Cell(1, 4).Value = "Milestone";
            ws.Cell(1, 5).Value = "Título";
            ws.Cell(1, 6).Value = "Responsável";
            ws.Cell(1, 7).Value = "Área Executora";
            ws.Cell(1, 8).Value = "Executor";
            ws.Cell(1, 9).Value = "Status";
            ws.Cell(1, 10).Value = "Risco Go-Live";
            ws.Cell(1, 11).Value = "Start";
            ws.Cell(1, 12).Value = "End";
            ws.Cell(1, 13).Value = "Observação";
            ws.Cell(1, 14).Value = "Link Repositório";

            var headerRow = ws.Row(1);
            headerRow.Style.Font.Bold = true;
            headerRow.Style.Fill.BackgroundColor = XLColor.FromArgb(0xD3D3D3);

            int rowNum = 2;
            foreach (var a in itens)
            {
                ws.Cell(rowNum, 1).Value = a.Id;
                ws.Cell(rowNum, 2).Value = a.IdPlanilha;
                ws.Cell(rowNum, 3).Value = a.Sistema ?? "";
                ws.Cell(rowNum, 4).Value = a.Milestone ?? "";
                ws.Cell(rowNum, 5).Value = a.Titulo ?? "";
                ws.Cell(rowNum, 6).Value = a.Responsavel ?? "";
                ws.Cell(rowNum, 7).Value = a.AreaExecutoraNome ?? "";
            ws.Cell(rowNum, 8).Value = a.Executor ?? "";
            ws.Cell(rowNum, 9).Value = a.Status.ToString();
            ws.Cell(rowNum, 10).Value = a.RiscoGoLive ? "Sim" : "Não";
            ws.Cell(rowNum, 11).Value = a.Start.HasValue ? a.Start.Value.ToString("dd/MM/yyyy") : "";
            ws.Cell(rowNum, 12).Value = a.End.HasValue ? a.End.Value.ToString("dd/MM/yyyy") : "";
            ws.Cell(rowNum, 13).Value = a.Observacao ?? "";
            ws.Cell(rowNum, 14).Value = a.LinkRepositorio ?? "";
                rowNum++;
            }

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            wb.SaveAs(stream);
            var bytes = stream.ToArray();
            var fileName = $"Atividades_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return Task.FromResult((bytes, fileName));
        }
    }
}