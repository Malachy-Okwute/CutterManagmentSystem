using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CutterManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemovedInfoUpdatesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InfoUpdateUserRelations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InfoUpdateUserRelations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InfoUpdateDataModelId = table.Column<int>(type: "int", nullable: true),
                    UserDataModelId = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastEntryDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserDataArchiveId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoUpdateUserRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InfoUpdateUserRelations_InfoUpdates_InfoUpdateDataModelId",
                        column: x => x.InfoUpdateDataModelId,
                        principalTable: "InfoUpdates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InfoUpdateUserRelations_UsersArchive_UserDataArchiveId",
                        column: x => x.UserDataArchiveId,
                        principalTable: "UsersArchive",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InfoUpdateUserRelations_Users_UserDataModelId",
                        column: x => x.UserDataModelId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_InfoUpdateUserRelations_InfoUpdateDataModelId",
                table: "InfoUpdateUserRelations",
                column: "InfoUpdateDataModelId");

            migrationBuilder.CreateIndex(
                name: "IX_InfoUpdateUserRelations_UserDataArchiveId",
                table: "InfoUpdateUserRelations",
                column: "UserDataArchiveId");

            migrationBuilder.CreateIndex(
                name: "IX_InfoUpdateUserRelations_UserDataModelId",
                table: "InfoUpdateUserRelations",
                column: "UserDataModelId");
        }
    }
}
