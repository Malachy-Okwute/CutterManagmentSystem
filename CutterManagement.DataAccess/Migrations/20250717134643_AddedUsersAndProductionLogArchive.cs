using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CutterManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedUsersAndProductionLogArchive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserDataArchiveId",
                table: "MachineUserInteractions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserDataArchiveId",
                table: "InfoUpdateUserRelations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "InfoUpdates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ProductionPartsLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    MachineNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CutterNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PartNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ToothCount = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PieceCount = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ToothSize = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserFullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FrequencyCheckResult = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CurrentShift = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateTimeOfCheck = table.Column<DateTime>(type: "datetime2", maxLength: 100, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionPartsLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductionPartsLogDataArchive",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    MachineNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CutterNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PartNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ToothCount = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PieceCount = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ToothSize = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserFullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FrequencyCheckResult = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CurrentShift = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateTimeOfCheck = table.Column<DateTime>(type: "datetime2", maxLength: 100, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionPartsLogDataArchive", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersArchive",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Shift = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersArchive", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MachineUserInteractions_UserDataArchiveId",
                table: "MachineUserInteractions",
                column: "UserDataArchiveId");

            migrationBuilder.CreateIndex(
                name: "IX_InfoUpdateUserRelations_UserDataArchiveId",
                table: "InfoUpdateUserRelations",
                column: "UserDataArchiveId");

            migrationBuilder.AddForeignKey(
                name: "FK_InfoUpdateUserRelations_UsersArchive_UserDataArchiveId",
                table: "InfoUpdateUserRelations",
                column: "UserDataArchiveId",
                principalTable: "UsersArchive",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MachineUserInteractions_UsersArchive_UserDataArchiveId",
                table: "MachineUserInteractions",
                column: "UserDataArchiveId",
                principalTable: "UsersArchive",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InfoUpdateUserRelations_UsersArchive_UserDataArchiveId",
                table: "InfoUpdateUserRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_MachineUserInteractions_UsersArchive_UserDataArchiveId",
                table: "MachineUserInteractions");

            migrationBuilder.DropTable(
                name: "ProductionPartsLog");

            migrationBuilder.DropTable(
                name: "ProductionPartsLogDataArchive");

            migrationBuilder.DropTable(
                name: "UsersArchive");

            migrationBuilder.DropIndex(
                name: "IX_MachineUserInteractions_UserDataArchiveId",
                table: "MachineUserInteractions");

            migrationBuilder.DropIndex(
                name: "IX_InfoUpdateUserRelations_UserDataArchiveId",
                table: "InfoUpdateUserRelations");

            migrationBuilder.DropColumn(
                name: "UserDataArchiveId",
                table: "MachineUserInteractions");

            migrationBuilder.DropColumn(
                name: "UserDataArchiveId",
                table: "InfoUpdateUserRelations");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "InfoUpdates");
        }
    }
}
