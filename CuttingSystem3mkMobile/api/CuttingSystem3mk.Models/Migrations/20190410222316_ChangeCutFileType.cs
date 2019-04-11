using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CuttingSystem3mkMobile.Models.Migrations
{
    public partial class ChangeCutFileType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "CutFile",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Value",
                table: "CutFile",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
