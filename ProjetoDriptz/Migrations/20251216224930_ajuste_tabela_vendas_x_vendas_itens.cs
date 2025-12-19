using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoDriptz.Migrations
{
    /// <inheritdoc />
    public partial class ajuste_tabela_vendas_x_vendas_itens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vendas_Estoques_EstoqueId",
                table: "Vendas");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendas_Produtos_ProdutoId",
                table: "Vendas");

            migrationBuilder.DropIndex(
                name: "IX_Vendas_EstoqueId",
                table: "Vendas");

            migrationBuilder.DropIndex(
                name: "IX_Vendas_ProdutoId",
                table: "Vendas");

            migrationBuilder.DropColumn(
                name: "EstoqueId",
                table: "Vendas");

            migrationBuilder.DropColumn(
                name: "PrecoItem",
                table: "Vendas");

            migrationBuilder.DropColumn(
                name: "ProdutoId",
                table: "Vendas");

            migrationBuilder.DropColumn(
                name: "Quantidade",
                table: "Vendas");

            migrationBuilder.DropColumn(
                name: "Tamanho",
                table: "Vendas");

            migrationBuilder.AddColumn<decimal>(
                name: "SubTotal",
                table: "VendasItens",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorAdicional",
                table: "Vendas",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EstoqueModelId",
                table: "Vendas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProdutoModelId",
                table: "Vendas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorTotal",
                table: "Vendas",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_EstoqueModelId",
                table: "Vendas",
                column: "EstoqueModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_ProdutoModelId",
                table: "Vendas",
                column: "ProdutoModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendas_Estoques_EstoqueModelId",
                table: "Vendas",
                column: "EstoqueModelId",
                principalTable: "Estoques",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendas_Produtos_ProdutoModelId",
                table: "Vendas",
                column: "ProdutoModelId",
                principalTable: "Produtos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vendas_Estoques_EstoqueModelId",
                table: "Vendas");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendas_Produtos_ProdutoModelId",
                table: "Vendas");

            migrationBuilder.DropIndex(
                name: "IX_Vendas_EstoqueModelId",
                table: "Vendas");

            migrationBuilder.DropIndex(
                name: "IX_Vendas_ProdutoModelId",
                table: "Vendas");

            migrationBuilder.DropColumn(
                name: "SubTotal",
                table: "VendasItens");

            migrationBuilder.DropColumn(
                name: "EstoqueModelId",
                table: "Vendas");

            migrationBuilder.DropColumn(
                name: "ProdutoModelId",
                table: "Vendas");

            migrationBuilder.DropColumn(
                name: "ValorTotal",
                table: "Vendas");

            migrationBuilder.AlterColumn<string>(
                name: "ValorAdicional",
                table: "Vendas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "EstoqueId",
                table: "Vendas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PrecoItem",
                table: "Vendas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ProdutoId",
                table: "Vendas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantidade",
                table: "Vendas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Tamanho",
                table: "Vendas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_EstoqueId",
                table: "Vendas",
                column: "EstoqueId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_ProdutoId",
                table: "Vendas",
                column: "ProdutoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendas_Estoques_EstoqueId",
                table: "Vendas",
                column: "EstoqueId",
                principalTable: "Estoques",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vendas_Produtos_ProdutoId",
                table: "Vendas",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
