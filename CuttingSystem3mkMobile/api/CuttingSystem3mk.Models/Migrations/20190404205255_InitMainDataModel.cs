using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CuttingSystem3mkMobile.Models.Migrations
{
    public partial class InitMainDataModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CutFile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Value = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CutFile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CutModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    QRCode = table.Column<string>(nullable: true),
                    IdCutFile = table.Column<int>(nullable: false),
                    IdDeviceModel = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CutModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CutModel_CutFile_IdCutFile",
                        column: x => x.IdCutFile,
                        principalTable: "CutFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CutModel_DeviceModel_IdDeviceModel",
                        column: x => x.IdDeviceModel,
                        principalTable: "DeviceModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CutCode",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Barcode = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IdCutModel = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CutCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CutCode_CutModel_IdCutModel",
                        column: x => x.IdCutModel,
                        principalTable: "CutModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CutCode_IdCutModel",
                table: "CutCode",
                column: "IdCutModel");

            migrationBuilder.CreateIndex(
                name: "IX_CutModel_IdCutFile",
                table: "CutModel",
                column: "IdCutFile");

            migrationBuilder.CreateIndex(
                name: "IX_CutModel_IdDeviceModel",
                table: "CutModel",
                column: "IdDeviceModel");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CutCode");

            migrationBuilder.DropTable(
                name: "CutModel");

            migrationBuilder.DropTable(
                name: "CutFile");

            migrationBuilder.DropTable(
                name: "DeviceModel");
        }
    }
}
