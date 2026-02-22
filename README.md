
# Cutover Planner (ASP.NET Core MVC + EF Core + SQLite)

Aplicativo para controlar o planejamento/cutover baseado em planilha Excel.

## Requisitos
- .NET 8 SDK
- SQLite (biblioteca nativa já embutida via provider)

## Pacotes NuGet principais
- Microsoft.EntityFrameworkCore, Microsoft.EntityFrameworkCore.Sqlite, Microsoft.EntityFrameworkCore.Tools
- ClosedXML (importação de Excel)

## Como executar
```bash
cd src/CutoverPlanner.Web
# (Opcional) criar migração e banco
dotnet ef migrations add InitialCreate
dotnet ef database update
# rodar
dotnet run
```
Acesse `http://localhost:5000` (ou a porta exibida no console).

## Importar planilha
Menu **Importar** → selecione o `.xlsx` (estrutura baseada nas abas:
1. **Plano Cutover Consolidado** (atividades e dependências; coluna PREDECESSORA aceita múltiplos IDs separados por `;`),
2. **Endpoints**,
3. **Area Executora**).

## Filtro padrão
Ao abrir **Atividades** sem parâmetros de query, aplica-se automaticamente o filtro padrão configurado em `appsettings.json`:
```json
"DefaultFilter": {
  "AreaExecutora": "GEAD/OPERAÇÃO DESPACHO",
  "StatusNot": "Concluido"
}
```
Altere conforme necessário.

## Observações
- Itens sem datas são considerados com duração 0 no cálculo do caminho crítico.
- Campos mapeados diretamente das colunas da planilha anexada.
```
