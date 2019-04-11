using Microsoft.EntityFrameworkCore.Migrations;

namespace CuttingSystem3mkMobile.Models.Migrations
{
    public partial class SetCutModelIdToNullableAsAforeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CutCode_CutModel_IdCutModel",
                table: "CutCode");

            migrationBuilder.AlterColumn<int>(
                name: "IdCutModel",
                table: "CutCode",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_CutCode_CutModel_IdCutModel",
                table: "CutCode",
                column: "IdCutModel",
                principalTable: "CutModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CutCode_CutModel_IdCutModel",
                table: "CutCode");

            migrationBuilder.AlterColumn<int>(
                name: "IdCutModel",
                table: "CutCode",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CutCode_CutModel_IdCutModel",
                table: "CutCode",
                column: "IdCutModel",
                principalTable: "CutModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
