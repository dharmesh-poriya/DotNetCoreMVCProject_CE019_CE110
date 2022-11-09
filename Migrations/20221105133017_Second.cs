using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentAttendanceManagementSystem.Migrations
{
    public partial class Second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Faculty_SubjectId",
                table: "Faculty");

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "Faculty",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Faculty_SubjectId",
                table: "Faculty",
                column: "SubjectId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Faculty_SubjectId",
                table: "Faculty");

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "Faculty",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Faculty_SubjectId",
                table: "Faculty",
                column: "SubjectId");
        }
    }
}
