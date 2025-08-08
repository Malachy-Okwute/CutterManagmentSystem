using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CutterManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ReAddedProductionPartLogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductionPartsLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    MachineNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CutterNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PartNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ToothCount = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PieceCount = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ToothSize = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserFullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FrequencyCheckResult = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CurrentShift = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CutterChangeInfo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CMMDataId = table.Column<int>(type: "int", nullable: true),
                    DateTimeOfCheck = table.Column<DateTime>(type: "datetime2", maxLength: 100, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", maxLength: 100, nullable: false)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductionPartsLog");
        }
    }
}
