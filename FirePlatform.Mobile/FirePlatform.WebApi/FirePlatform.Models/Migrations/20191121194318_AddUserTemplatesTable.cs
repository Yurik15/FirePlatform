using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FirePlatform.Models.Migrations
{
    public partial class AddUserTemplatesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Data",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserTemplates_Id",
                table: "User",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "User",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_User_UserTemplates_Id",
                table: "User",
                column: "UserTemplates_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_User_UserTemplates_Id",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Data",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UserTemplates_Id",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "User");
        }
    }
}
