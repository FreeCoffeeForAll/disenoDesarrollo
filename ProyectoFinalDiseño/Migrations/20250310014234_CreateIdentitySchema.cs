using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinalDiseño.Migrations
{
    /// <inheritdoc />
    public partial class CreateIdentitySchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoriaID",
                table: "Inventario",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    CategoriaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCategoria = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.CategoriaID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_CategoriaID",
                table: "Inventario",
                column: "CategoriaID");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventario_Categorias_CategoriaID",
                table: "Inventario",
                column: "CategoriaID",
                principalTable: "Categorias",
                principalColumn: "CategoriaID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventario_Categorias_CategoriaID",
                table: "Inventario");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropIndex(
                name: "IX_Inventario_CategoriaID",
                table: "Inventario");

            migrationBuilder.DropColumn(
                name: "CategoriaID",
                table: "Inventario");
        }
    }
}
