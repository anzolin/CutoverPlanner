
using CutoverPlanner.Web.Data;
using CutoverPlanner.Web.Models;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;

namespace CutoverPlanner.Web.Services
{
    public class ExcelImportService
    {
        private readonly AppDbContext _db;
        public ExcelImportService(AppDbContext db) => _db = db;

        public async Task<(int atividades, int deps, int areas, int endpoints)> ImportAsync(Stream xlsx)
        {
            using var wb = new XLWorkbook(xlsx);
            // Planilha 1: Plano Cutover Consolidado
            var ws1 = wb.Worksheet(1);
            var firstRow = 2;
            var lastRow = ws1.LastRowUsed().RowNumber();
            var tempByPlanId = new Dictionary<int, int>();

            for (int r = firstRow; r <= lastRow; r++)
            {
                int? idPlan = TryGetInt(ws1.Cell(r, GetCol(ws1, "ID")));
                var titulo = ws1.Cell(r, GetCol(ws1, "ATIVIDADES")).GetString();
                var sistema = ws1.Cell(r, GetCol(ws1, "SISTEMA")).GetString();
                if (string.IsNullOrWhiteSpace(titulo) && string.IsNullOrWhiteSpace(sistema))
                    continue;
                var a = new Atividade
                {
                    IdPlanilha = idPlan,
                    Sistema = sistema,
                    Milestone = ws1.Cell(r, GetCol(ws1, "MILESTONE")).GetString(),
                    Titulo = string.IsNullOrWhiteSpace(titulo) ? "(sem título)" : titulo,
                    Categoria = ws1.Cell(r, GetCol(ws1, "CATEGORIA")).GetString(),
                    RequerTestePerformance = NormalizeBool(ws1.Cell(r, GetCol(ws1, "REQUER TESTE PERFORMANCE")).GetString()),
                    TipoTeste = ws1.Cell(r, GetCol(ws1, "TIPO TESTE")).GetString(),
                    Procedimento = ws1.Cell(r, GetCol(ws1, "PROCEDIMENTO")).GetString(),
                    Metricas = ws1.Cell(r, GetCol(ws1, "MÉTRICAS")).GetString(),
                    CriterioAceite = ws1.Cell(r, GetCol(ws1, "CRITÉRIO DE ACEITE")).GetString(),
                    Evidencias = ws1.Cell(r, GetCol(ws1, "EVIDÊNCIAS")).GetString(),
                    Responsavel = ws1.Cell(r, GetCol(ws1, "RESPONSÁVEL")).GetString(),
                    AreaExecutoraNome = ws1.Cell(r, GetCol(ws1, "ÁREA EXECUTORA")).GetString(),
                    Executor = ws1.Cell(r, GetCol(ws1, "EXECUTOR")).GetString(),
                    Status = ParseStatus(ws1.Cell(r, GetCol(ws1, "STATUS")).GetString()),
                    RiscoGoLive = ParseNullableBool(ws1.Cell(r, GetCol(ws1, "Risco Go-live")).GetString()),
                    Start = GetDateTimeOrNull(ws1.Cell(r, GetCol(ws1, "START"))),
                    End = GetDateTimeOrNull(ws1.Cell(r, GetCol(ws1, "END"))),
                    Observacao = ws1.Cell(r, GetCol(ws1, "OBSERVAÇÃO")).GetString(),
                    LinkRepositorio = ws1.Cell(r, GetCol(ws1, "Link do repositório")).GetString(),
                    PredecessorasRaw = ws1.Cell(r, GetCol(ws1, "PREDECESSORA")).GetString()
                };
                _db.Atividades.Add(a);
                await _db.SaveChangesAsync();
                if (idPlan.HasValue) tempByPlanId[idPlan.Value] = a.Id;
            }

            int depCount = 0;
            var all = await _db.Atividades.AsNoTracking().ToListAsync();
            foreach (var a in all)
            {
                if (string.IsNullOrWhiteSpace(a.PredecessorasRaw)) continue;
                var tokens = a.PredecessorasRaw.Split(new[] { ';', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var t in tokens)
                {
                    if (int.TryParse(t.Trim(), out int planIdRef) && tempByPlanId.TryGetValue(planIdRef, out int predDbId))
                    {
                        _db.AtividadeDependencias.Add(new AtividadeDependencia
                        {
                            AtividadeId = a.Id,
                            PredecessoraId = predDbId
                        });
                        depCount++;
                    }
                }
            }
            await _db.SaveChangesAsync();

            // Planilha 2: Endpoints
            if (wb.Worksheets.Count >= 2)
            {
                var ws2 = wb.Worksheet(2);
                var lr2 = ws2.LastRowUsed()?.RowNumber() ?? 1;
                for (int r = 2; r <= lr2; r++)
                {
                    var ep = new Endpoint
                    {
                        Sistemas = ws2.Cell(r, 1).GetString(),
                        TipoIntegracao = ws2.Cell(r, 2).GetString(),
                        Barramento = ws2.Cell(r, 3).GetString(),
                        Integracao = ws2.Cell(r, 4).GetString(),
                        Url = ws2.Cell(r, 5).GetString()
                    };
                    if (!string.IsNullOrWhiteSpace(ep.Sistemas) || !string.IsNullOrWhiteSpace(ep.Url))
                        _db.Endpoints.Add(ep);
                }
                await _db.SaveChangesAsync();
            }

            // Planilha 3: Area Executora
            if (wb.Worksheets.Count >= 3)
            {
                var ws3 = wb.Worksheet(3);
                var lr3 = ws3.LastRowUsed()?.RowNumber() ?? 1;
                for (int r = 2; r <= lr3; r++)
                {
                    var area = new AreaExecutora
                    {
                        Gerencia = ws3.Cell(r, 1).GetString(),
                        NomeAreaExecutora = ws3.Cell(r, 2).GetString(),
                        Torre = ws3.Cell(r, 3).GetString(),
                        Responsavel = ws3.Cell(r, 4).GetString(),
                        Executor = ws3.Cell(r, 5).GetString(),
                    };
                    if (!string.IsNullOrWhiteSpace(area.NomeAreaExecutora))
                        _db.AreasExecutoras.Add(area);
                }
                await _db.SaveChangesAsync();
            }

            return (_db.Atividades.Count(), depCount, _db.AreasExecutoras.Count(), _db.Endpoints.Count());
        }

        private static int GetCol(IXLWorksheet ws, string headerName)
        {
            var rng = ws.RangeUsed().FirstRowUsed();
            foreach (var cell in rng.Cells())
            {
                if (string.Equals(cell.GetString().Trim(), headerName, StringComparison.OrdinalIgnoreCase))
                    return cell.Address.ColumnNumber;
            }
            throw new InvalidOperationException($"Coluna '{headerName}' não encontrada na planilha.");
        }

        private static int? TryGetInt(IXLCell c)
        {
            if (c.DataType == XLDataType.Number)
                return (int)c.GetDouble();
            if (int.TryParse(c.GetString(), out var v)) return v;
            return null;
        }

        private static bool NormalizeBool(string s)
        {
            s = (s ?? string.Empty).Trim().ToUpperInvariant();
            return s == "SIM" || s == "YES" || s == "Y" || s == "TRUE" || s == "1";
        }
        private static bool? ParseNullableBool(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            return NormalizeBool(s);
        }
        private static DateTime? GetDateTimeOrNull(IXLCell cell)
        {
            if (cell.DataType == XLDataType.DateTime && !cell.IsEmpty()) return cell.GetDateTime();
            if (DateTime.TryParse(cell.GetString(), out var dt)) return dt;
            return null;
        }
        private static StatusAtividade ParseStatus(string s)
        {
            s = (s ?? string.Empty).Trim().ToUpperInvariant();
            return s switch
            {
                "EM ANDAMENTO" => StatusAtividade.EmAndamento,
                "CONCLUÍDO" || "CONCLUIDO" => StatusAtividade.Concluido,
                _ => StatusAtividade.NaoIniciado
            };
        }
    }
}
