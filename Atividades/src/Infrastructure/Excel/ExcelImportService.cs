using ClosedXML.Excel;
using CutoverManager.Application.DTOs;

namespace CutoverManager.Infrastructure.Excel;

public static class ExcelImportService
{
    public static IEnumerable<AtividadeDTO> LerAtividades(Stream stream, int idPlano)
    {
        var wb = new XLWorkbook(stream);
        var ws = wb.Worksheet(1);

        var resultado = new List<AtividadeDTO>();

        foreach (var row in ws.RowsUsed().Skip(1))
        {
            var dto = new AtividadeDTO
            {
                IdPlano = idPlano,
                Titulo = row.Cell(1).GetString(),
                IdExecutor = 0,   // será resolvido pelo ApplicationService
                IdSistema = 0,
                IdMarco = 0,
                RiscoGoLive = row.Cell(5).GetString().ToUpper() == "SIM",
                Inicio = row.Cell(6).GetDateTime(),
                Termino = row.Cell(7).GetDateTime(),
                Observacao = row.Cell(8).GetString(),
                Status = Domain.Enums.StatusAtividade.NaoIniciado
            };

            resultado.Add(dto);
        }

        return resultado;
    }
}