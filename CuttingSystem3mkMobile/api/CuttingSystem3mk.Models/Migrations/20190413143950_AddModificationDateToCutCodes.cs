using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CuttingSystem3mkMobile.Models.Migrations
{
    public partial class AddModificationDateToCutCodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ModificactionDate",
                table: "CutCode",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModificactionDate",
                table: "CutCode");
        }
    }
}
