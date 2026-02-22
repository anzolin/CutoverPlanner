using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CutoverPlanner.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AreasExecutoras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Gerencia = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    NomeAreaExecutora = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Torre = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    Responsavel = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Executor = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreasExecutoras", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Atividades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdPlanilha = table.Column<int>(type: "INTEGER", nullable: true),
                    Sistema = table.Column<string>(type: "TEXT", nullable: true),
                    Milestone = table.Column<string>(type: "TEXT", nullable: true),
                    Titulo = table.Column<string>(type: "TEXT", nullable: false),
                    Categoria = table.Column<string>(type: "TEXT", nullable: true),
                    RequerTestePerformance = table.Column<bool>(type: "INTEGER", nullable: false),
                    TipoTeste = table.Column<string>(type: "TEXT", nullable: true),
                    Procedimento = table.Column<string>(type: "TEXT", nullable: true),
                    Metricas = table.Column<string>(type: "TEXT", nullable: true),
                    CriterioAceite = table.Column<string>(type: "TEXT", nullable: true),
                    Evidencias = table.Column<string>(type: "TEXT", nullable: true),
                    Responsavel = table.Column<string>(type: "TEXT", nullable: true),
                    AreaExecutoraNome = table.Column<string>(type: "TEXT", nullable: true),
                    Executor = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    RiscoGoLive = table.Column<bool>(type: "INTEGER", nullable: true),
                    Start = table.Column<DateTime>(type: "TEXT", nullable: true),
                    End = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Observacao = table.Column<string>(type: "TEXT", nullable: true),
                    LinkRepositorio = table.Column<string>(type: "TEXT", nullable: true),
                    PredecessorasRaw = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atividades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Endpoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Sistemas = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    TipoIntegracao = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    Barramento = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    Integracao = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    Url = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Endpoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AtividadeDependencias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AtividadeId = table.Column<int>(type: "INTEGER", nullable: false),
                    PredecessoraId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtividadeDependencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AtividadeDependencias_Atividades_AtividadeId",
                        column: x => x.AtividadeId,
                        principalTable: "Atividades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AtividadeDependencias_Atividades_PredecessoraId",
                        column: x => x.PredecessoraId,
                        principalTable: "Atividades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AtividadeDependencias_AtividadeId",
                table: "AtividadeDependencias",
                column: "AtividadeId");

            migrationBuilder.CreateIndex(
                name: "IX_AtividadeDependencias_PredecessoraId",
                table: "AtividadeDependencias",
                column: "PredecessoraId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AreasExecutoras");

            migrationBuilder.DropTable(
                name: "AtividadeDependencias");

            migrationBuilder.DropTable(
                name: "Endpoints");

            migrationBuilder.DropTable(
                name: "Atividades");
        }
    }
}
