using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CutterManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangedModelPropertyToSummaryNumberInProdLogAndProdLogArchiveDataModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
               name: "Model",
               table: "ProductionPartsLog",
               newName: "SummaryNumber");

            migrationBuilder.RenameColumn(
               name: "Model",
               table: "ProductionPartsLogDataArchive",
               newName: "SummaryNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
               name: "SummaryNumber",
               table: "ProductionPartsLog",
               newName: "Model");

            migrationBuilder.RenameColumn(
               name: "SummaryNumber",
               table: "ProductionPartsLogDataArchive",
               newName: "Model");
        }
    }
}
