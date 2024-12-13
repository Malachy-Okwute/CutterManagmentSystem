using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CutterManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialDbCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MachineDataStore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SetId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Count = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PartToothSize = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", maxLength: 100, nullable: false),
                    Owner = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", maxLength: 100, nullable: false),
                    FrequencyCheckResult = table.Column<int>(type: "int", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineDataStore", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CutterDataModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CutterId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Count = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kind = table.Column<int>(type: "int", nullable: false),
                    Owner = table.Column<int>(type: "int", nullable: false),
                    Condition = table.Column<int>(type: "int", nullable: false),
                    CutterForeignId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CutterDataModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CutterDataModel_MachineDataStore_CutterForeignId",
                        column: x => x.CutterForeignId,
                        principalTable: "MachineDataStore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PartDataModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartToothCount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kind = table.Column<int>(type: "int", nullable: false),
                    PartForeignId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartDataModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartDataModel_MachineDataStore_PartForeignId",
                        column: x => x.PartForeignId,
                        principalTable: "MachineDataStore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CutterDataModel_CutterForeignId",
                table: "CutterDataModel",
                column: "CutterForeignId");

            migrationBuilder.CreateIndex(
                name: "IX_PartDataModel_PartForeignId",
                table: "PartDataModel",
                column: "PartForeignId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CutterDataModel");

            migrationBuilder.DropTable(
                name: "PartDataModel");

            migrationBuilder.DropTable(
                name: "MachineDataStore");
        }
    }
}
