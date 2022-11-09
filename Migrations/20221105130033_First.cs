using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentAttendanceManagementSystem.Migrations
{
    public partial class First : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Faculty");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Faculty",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
