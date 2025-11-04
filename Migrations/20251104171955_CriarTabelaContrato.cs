using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrabalhoElvis2.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelaContrato : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contratos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroContrato = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Imovel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Locatario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValorAluguel = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataTermino = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DiaVencimento = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contratos", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contratos");
        }
    }
}
