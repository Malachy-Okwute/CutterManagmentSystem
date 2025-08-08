using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CutterManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedAColumnToProductionLogArchive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CMMDataId",
                table: "ProductionPartsLogDataArchive",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CutterChangeInfo",
                table: "ProductionPartsLogDataArchive",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPartsLogDataArchive_CMMDataId",
                table: "ProductionPartsLogDataArchive",
                column: "CMMDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionPartsLogDataArchive_CMMData_CMMDataId",
                table: "ProductionPartsLogDataArchive",
                column: "CMMDataId",
                principalTable: "CMMData",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionPartsLogDataArchive_CMMData_CMMDataId",
                table: "ProductionPartsLogDataArchive");

            migrationBuilder.DropIndex(
                name: "IX_ProductionPartsLogDataArchive_CMMDataId",
                table: "ProductionPartsLogDataArchive");

            migrationBuilder.DropColumn(
                name: "CMMDataId",
                table: "ProductionPartsLogDataArchive");

            migrationBuilder.DropColumn(
                name: "CutterChangeInfo",
                table: "ProductionPartsLogDataArchive");

            migrationBuilder.CreateTable(
                name: "ProductionPartsLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CMMDataId = table.Column<int>(type: "int", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CurrentShift = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CutterChangeInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CutterNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", maxLength: 100, nullable: false),
                    DateTimeOfCheck = table.Column<DateTime>(type: "datetime2", maxLength: 100, nullable: false),
                    FrequencyCheckResult = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    MachineNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PartNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PieceCount = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ToothCount = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ToothSize = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserFullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionPartsLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionPartsLog_CMMData_CMMDataId",
                        column: x => x.CMMDataId,
                        principalTable: "CMMData",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPartsLog_CMMDataId",
                table: "ProductionPartsLog",
                column: "CMMDataId");
        }
    }
}
