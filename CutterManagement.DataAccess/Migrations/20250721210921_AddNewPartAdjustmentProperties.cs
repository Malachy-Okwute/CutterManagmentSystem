using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CutterManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddNewPartAdjustmentProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Kind",
                table: "InfoUpdates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PartNumberWithMove",
                table: "InfoUpdates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PressureAngleCoast",
                table: "InfoUpdates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PressureAngleDrive",
                table: "InfoUpdates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SpiralAngleCoast",
                table: "InfoUpdates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SpiralAngleDrive",
                table: "InfoUpdates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Kind",
                table: "InfoUpdates");

            migrationBuilder.DropColumn(
                name: "PartNumberWithMove",
                table: "InfoUpdates");

            migrationBuilder.DropColumn(
                name: "PressureAngleCoast",
                table: "InfoUpdates");

            migrationBuilder.DropColumn(
                name: "PressureAngleDrive",
                table: "InfoUpdates");

            migrationBuilder.DropColumn(
                name: "SpiralAngleCoast",
                table: "InfoUpdates");

            migrationBuilder.DropColumn(
                name: "SpiralAngleDrive",
                table: "InfoUpdates");
        }
    }
}
