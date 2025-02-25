using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo.Persistence.Migrations
{
    public partial class FixPermissionsToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Users_UserId",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_UserId",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Permissions");

            migrationBuilder.CreateTable(
                name: "PermissionUser",
                columns: table => new
                {
                    PermissionsId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionUser", x => new { x.PermissionsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_PermissionUser_Permissions_PermissionsId",
                        column: x => x.PermissionsId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermissionUser_UsersId",
                table: "PermissionUser",
                column: "UsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissionUser");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Permissions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_UserId",
                table: "Permissions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Users_UserId",
                table: "Permissions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
