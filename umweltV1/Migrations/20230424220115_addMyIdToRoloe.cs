using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace umweltV1.Migrations
{
    public partial class addMyIdToRoloe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MyRoleId",
                table: "Roles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MyRoleId",
                table: "Roles");
        }
    }
}
