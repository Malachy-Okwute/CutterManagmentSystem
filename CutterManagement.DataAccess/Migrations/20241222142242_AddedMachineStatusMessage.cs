using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CutterManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedMachineStatusMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StatusMessage",
                table: "Machines",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusMessage",
                table: "Machines");
        }
    }
}
