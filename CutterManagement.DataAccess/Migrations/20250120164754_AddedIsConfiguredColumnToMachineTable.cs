using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CutterManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsConfiguredColumnToMachineTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsConfigured",
                table: "Machines",
                type: "bit",
                maxLength: 100,
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsConfigured",
                table: "Machines");
        }
    }
}
