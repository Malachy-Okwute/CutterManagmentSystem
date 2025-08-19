using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CutterManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InfoUpdateDataModelNavigationRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InfoUpdateUserRelations_InfoUpdates_InfoUpdatesDataModelId",
                table: "InfoUpdateUserRelations");

            migrationBuilder.RenameColumn(
                name: "InfoUpdatesDataModelId",
                table: "InfoUpdateUserRelations",
                newName: "InfoUpdateDataModelId");

            migrationBuilder.RenameIndex(
                name: "IX_InfoUpdateUserRelations_InfoUpdatesDataModelId",
                table: "InfoUpdateUserRelations",
                newName: "IX_InfoUpdateUserRelations_InfoUpdateDataModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_InfoUpdateUserRelations_InfoUpdates_InfoUpdateDataModelId",
                table: "InfoUpdateUserRelations",
                column: "InfoUpdateDataModelId",
                principalTable: "InfoUpdates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InfoUpdateUserRelations_InfoUpdates_InfoUpdateDataModelId",
                table: "InfoUpdateUserRelations");

            migrationBuilder.RenameColumn(
                name: "InfoUpdateDataModelId",
                table: "InfoUpdateUserRelations",
                newName: "InfoUpdatesDataModelId");

            migrationBuilder.RenameIndex(
                name: "IX_InfoUpdateUserRelations_InfoUpdateDataModelId",
                table: "InfoUpdateUserRelations",
                newName: "IX_InfoUpdateUserRelations_InfoUpdatesDataModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_InfoUpdateUserRelations_InfoUpdates_InfoUpdatesDataModelId",
                table: "InfoUpdateUserRelations",
                column: "InfoUpdatesDataModelId",
                principalTable: "InfoUpdates",
                principalColumn: "Id");
        }
    }
}
