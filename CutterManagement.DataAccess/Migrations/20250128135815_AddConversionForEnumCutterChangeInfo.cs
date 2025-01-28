using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CutterManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddConversionForEnumCutterChangeInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CutterChangeInfo",
                table: "Cutters",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CutterChangeInfo",
                table: "Cutters",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
