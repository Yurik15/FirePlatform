using Microsoft.EntityFrameworkCore.Migrations;

namespace FirePlatform.Models.Migrations
{
    public partial class ScriptDefinition2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "ScriptDefinitions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Link",
                table: "ScriptDefinitions");
        }
    }
}
