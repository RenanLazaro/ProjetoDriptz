using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoDriptz.Migrations
{
    /// <inheritdoc />
    public partial class campos_desconto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DescontoPercentual",
                table: "VendasItens",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DescontoGeral",
                table: "Vendas",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescontoPercentual",
                table: "VendasItens");

            migrationBuilder.DropColumn(
                name: "DescontoGeral",
                table: "Vendas");
        }
    }
}
