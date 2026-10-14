using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InversionEnPozo.Api.Migrations
{
    /// <inheritdoc />
    public partial class AgregarProyectosInversion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProyectosInversiones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CostoTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MontoRecaudado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ConstructoraId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProyectosInversiones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HitosObras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProyectoInversionId = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PorcentajeAsignado = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Completado = table.Column<bool>(type: "bit", nullable: false),
                    FechaCompletado = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HitosObras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HitosObras_ProyectosInversiones_ProyectoInversionId",
                        column: x => x.ProyectoInversionId,
                        principalTable: "ProyectosInversiones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inversiones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProyectoInversionId = table.Column<int>(type: "int", nullable: false),
                    InversionistaId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MontoAportado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TokensOtorgados = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TxHashEthereum = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FechaInversion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inversiones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inversiones_ProyectosInversiones_ProyectoInversionId",
                        column: x => x.ProyectoInversionId,
                        principalTable: "ProyectosInversiones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HitosObras_ProyectoInversionId",
                table: "HitosObras",
                column: "ProyectoInversionId");

            migrationBuilder.CreateIndex(
                name: "IX_Inversiones_ProyectoInversionId",
                table: "Inversiones",
                column: "ProyectoInversionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HitosObras");

            migrationBuilder.DropTable(
                name: "Inversiones");

            migrationBuilder.DropTable(
                name: "ProyectosInversiones");
        }
    }
}
