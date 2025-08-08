using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CutterManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCMMDATAFieldINProductionPartsLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasAttachedMoves",
                table: "InfoUpdates");

            migrationBuilder.AddColumn<int>(
                name: "CMMDataId",
                table: "ProductionPartsLog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CutterChangeInfo",
                table: "ProductionPartsLog",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPartsLog_CMMDataId",
                table: "ProductionPartsLog",
                column: "CMMDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionPartsLog_CMMData_CMMDataId",
                table: "ProductionPartsLog",
                column: "CMMDataId",
                principalTable: "CMMData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionPartsLog_CMMData_CMMDataId",
                table: "ProductionPartsLog");

            migrationBuilder.DropIndex(
                name: "IX_ProductionPartsLog_CMMDataId",
                table: "ProductionPartsLog");

            migrationBuilder.DropColumn(
                name: "CMMDataId",
                table: "ProductionPartsLog");

            migrationBuilder.DropColumn(
                name: "CutterChangeInfo",
                table: "ProductionPartsLog");

            migrationBuilder.AddColumn<bool>(
                name: "HasAttachedMoves",
                table: "InfoUpdates",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
