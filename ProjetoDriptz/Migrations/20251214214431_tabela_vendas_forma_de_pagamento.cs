using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoDriptz.Migrations
{
    /// <inheritdoc />
    public partial class tabela_vendas_forma_de_pagamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FormaDePagamento",
                table: "Vendas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FormaDePagamentoAdicional",
                table: "Vendas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PossuiMaisDeUmaFormaPagamento",
                table: "Vendas",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormaDePagamento",
                table: "Vendas");

            migrationBuilder.DropColumn(
                name: "FormaDePagamentoAdicional",
                table: "Vendas");

            migrationBuilder.DropColumn(
                name: "PossuiMaisDeUmaFormaPagamento",
                table: "Vendas");
        }
    }
}
