using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoDriptz.Migrations
{
    /// <inheritdoc />
    public partial class tabela_vendas_data_venda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataVenda",
                table: "Vendas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataVenda",
                table: "Vendas");
        }
    }
}
