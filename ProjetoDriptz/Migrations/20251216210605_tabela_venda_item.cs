using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoDriptz.Migrations
{
    /// <inheritdoc />
    public partial class tabela_venda_item : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VendasItens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProdutoId = table.Column<int>(type: "int", nullable: false),
                    EstoqueId = table.Column<int>(type: "int", nullable: false),
                    VendaId = table.Column<int>(type: "int", nullable: false),
                    Tamanho = table.Column<int>(type: "int", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    PrecoUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendasItens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VendasItens_Estoques_EstoqueId",
                        column: x => x.EstoqueId,
                        principalTable: "Estoques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VendasItens_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VendasItens_Vendas_VendaId",
                        column: x => x.VendaId,
                        principalTable: "Vendas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VendasItens_EstoqueId",
                table: "VendasItens",
                column: "EstoqueId");

            migrationBuilder.CreateIndex(
                name: "IX_VendasItens_ProdutoId",
                table: "VendasItens",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_VendasItens_VendaId",
                table: "VendasItens",
                column: "VendaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VendasItens");
        }
    }
}
