using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CutterManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CMMDataModelIdFieldRemovedFromCutterDataModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CMMDataModelId",
                table: "Cutters");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CMMDataModelId",
                table: "Cutters",
                type: "int",
                nullable: true);
        }
    }
}
