using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoDriptz.Migrations
{
    /// <inheritdoc />
    public partial class produtos_tamanho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Tamanho",
                table: "Produtos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tamanho",
                table: "Produtos");
        }
    }
}
