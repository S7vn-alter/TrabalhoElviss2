using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrabalhoElvis2.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelaContratos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiaVencimento",
                table: "Contratos");

            migrationBuilder.RenameColumn(
                name: "ValorAluguel",
                table: "Contratos",
                newName: "Valor");

            migrationBuilder.AlterColumn<string>(
                name: "Imovel",
                table: "Contratos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Contratos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Contratos");

            migrationBuilder.RenameColumn(
                name: "Valor",
                table: "Contratos",
                newName: "ValorAluguel");

            migrationBuilder.AlterColumn<string>(
                name: "Imovel",
                table: "Contratos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "DiaVencimento",
                table: "Contratos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
