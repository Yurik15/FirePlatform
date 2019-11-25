using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FirePlatform.Models.Migrations
{
    public partial class AddUserTemplatesTable_updatename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "MainName",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "User");

            migrationBuilder.CreateTable(
                name: "UserTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    MainName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Data = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTemplates", x => x.UserId);
                    table.UniqueConstraint("AK_UserTemplates_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTemplates_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTemplates");

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
                name: "MainName",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "User",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_User_UserTemplates_Id",
                table: "User",
                column: "UserTemplates_Id");
        }
    }
}
