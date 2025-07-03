using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CutterManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddInformationUpdatesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InfoUpdates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Author = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PublishDate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastUpdatedDate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Information = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoUpdates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InfoUpdateUserRelations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InfoUpdatesDataModelId = table.Column<int>(type: "int", nullable: true),
                    UserDataModelId = table.Column<int>(type: "int", nullable: true),
                    LastEntryDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoUpdateUserRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InfoUpdateUserRelations_InfoUpdates_InfoUpdatesDataModelId",
                        column: x => x.InfoUpdatesDataModelId,
                        principalTable: "InfoUpdates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InfoUpdateUserRelations_Users_UserDataModelId",
                        column: x => x.UserDataModelId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_InfoUpdateUserRelations_InfoUpdatesDataModelId",
                table: "InfoUpdateUserRelations",
                column: "InfoUpdatesDataModelId");

            migrationBuilder.CreateIndex(
                name: "IX_InfoUpdateUserRelations_UserDataModelId",
                table: "InfoUpdateUserRelations",
                column: "UserDataModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InfoUpdateUserRelations");

            migrationBuilder.DropTable(
                name: "InfoUpdates");
        }
    }
}
