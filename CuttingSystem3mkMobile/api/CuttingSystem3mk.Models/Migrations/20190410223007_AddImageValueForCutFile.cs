using Microsoft.EntityFrameworkCore.Migrations;

namespace CuttingSystem3mkMobile.Models.Migrations
{
    public partial class AddImageValueForCutFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageValue",
                table: "CutFile",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageValue",
                table: "CutFile");
        }
    }
}
