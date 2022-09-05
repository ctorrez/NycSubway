using Microsoft.EntityFrameworkCore.Migrations;

namespace NycSubway.Database.Migrations.SubwayDb
{
    public partial class RenameColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FirtName",
                table: "AppUsers",
                newName: "FirstName");

            migrationBuilder.RenameIndex(
                name: "IX_AppUsers_FirtName_LastName",
                table: "AppUsers",
                newName: "IX_AppUsers_FirstName_LastName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "AppUsers",
                newName: "FirtName");

            migrationBuilder.RenameIndex(
                name: "IX_AppUsers_FirstName_LastName",
                table: "AppUsers",
                newName: "IX_AppUsers_FirtName_LastName");
        }
    }
}
