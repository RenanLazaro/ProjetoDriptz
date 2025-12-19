using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoDriptz.Migrations
{
    /// <inheritdoc />
    public partial class ajuste_tabela_vendas_x_vendas_itens_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "EstoqueModelId",
                table: "Vendas");

            migrationBuilder.DropColumn(
                name: "ProdutoModelId",
                table: "Vendas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
