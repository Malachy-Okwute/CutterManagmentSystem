using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CutterManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class IncludePartSummaryInDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Machines_Parts_PartDataModelId",
                table: "Machines");

            migrationBuilder.DropIndex(
                name: "IX_Machines_PartDataModelId",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "MachineDataModelId",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "PartDataModelId",
                table: "Machines");

            migrationBuilder.AlterColumn<string>(
                name: "SummaryNumber",
                table: "Parts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SummaryNumber",
                table: "Parts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "MachineDataModelId",
                table: "Parts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PartDataModelId",
                table: "Machines",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Machines_PartDataModelId",
                table: "Machines",
                column: "PartDataModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Parts_PartDataModelId",
                table: "Machines",
                column: "PartDataModelId",
                principalTable: "Parts",
                principalColumn: "Id");
        }
    }
}
