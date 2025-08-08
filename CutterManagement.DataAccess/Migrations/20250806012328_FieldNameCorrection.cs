using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CutterManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FieldNameCorrection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionPartsLog_CMMData_CMMDataId",
                table: "ProductionPartsLog");

            migrationBuilder.AlterColumn<int>(
                name: "CMMDataId",
                table: "ProductionPartsLog",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionPartsLog_CMMData_CMMDataId",
                table: "ProductionPartsLog",
                column: "CMMDataId",
                principalTable: "CMMData",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionPartsLog_CMMData_CMMDataId",
                table: "ProductionPartsLog");

            migrationBuilder.AlterColumn<int>(
                name: "CMMDataId",
                table: "ProductionPartsLog",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionPartsLog_CMMData_CMMDataId",
                table: "ProductionPartsLog",
                column: "CMMDataId",
                principalTable: "CMMData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
