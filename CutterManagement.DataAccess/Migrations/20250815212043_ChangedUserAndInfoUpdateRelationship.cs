using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CutterManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangedUserAndInfoUpdateRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserDataModelId",
                table: "InfoUpdates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_InfoUpdates_UserDataModelId",
                table: "InfoUpdates",
                column: "UserDataModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_InfoUpdates_Users_UserDataModelId",
                table: "InfoUpdates",
                column: "UserDataModelId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InfoUpdates_Users_UserDataModelId",
                table: "InfoUpdates");

            migrationBuilder.DropIndex(
                name: "IX_InfoUpdates_UserDataModelId",
                table: "InfoUpdates");

            migrationBuilder.DropColumn(
                name: "UserDataModelId",
                table: "InfoUpdates");
        }
    }
}
