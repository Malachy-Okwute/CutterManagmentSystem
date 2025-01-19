using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CutterManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialDatabaseMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Machines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MachineSetId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Count = table.Column<int>(type: "int", maxLength: 100, nullable: false),
                    PartToothSize = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DateTimeLastModified = table.Column<DateTime>(type: "datetime2", maxLength: 100, nullable: false),
                    DateTimeLastSetup = table.Column<DateTime>(type: "datetime2", maxLength: 100, nullable: false),
                    Owner = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StatusMessage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CutterChangeInfo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CutterChangeComment = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    FrequencyCheckResult = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CutterDataModelId = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Machines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cutters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CutterNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Kind = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Owner = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Condition = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastUsedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MachineDataModelId = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cutters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cutters_Machines_MachineDataModelId",
                        column: x => x.MachineDataModelId,
                        principalTable: "Machines",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Parts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PartToothCount = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Kind = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MachineDataModelId = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parts_Machines_MachineDataModelId",
                        column: x => x.MachineDataModelId,
                        principalTable: "Machines",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Shift = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MachineDataModelId = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Machines_MachineDataModelId",
                        column: x => x.MachineDataModelId,
                        principalTable: "Machines",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cutters_MachineDataModelId",
                table: "Cutters",
                column: "MachineDataModelId",
                unique: true,
                filter: "[MachineDataModelId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Parts_MachineDataModelId",
                table: "Parts",
                column: "MachineDataModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_MachineDataModelId",
                table: "Users",
                column: "MachineDataModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cutters");

            migrationBuilder.DropTable(
                name: "Parts");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Machines");
        }
    }
}
