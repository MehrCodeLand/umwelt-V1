using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace umweltV1.Migrations
{
    public partial class AddMyIDtoPermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MyPermissionId",
                table: "Permissions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MyPermissionId",
                table: "Permissions");
        }
    }
}
