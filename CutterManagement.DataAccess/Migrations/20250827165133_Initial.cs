using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CutterManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
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
                    PartNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StatusMessage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CutterChangeComment = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    IsConfigured = table.Column<bool>(type: "bit", maxLength: 100, nullable: false),
                    DateTimeLastModified = table.Column<DateTime>(type: "datetime2", maxLength: 100, nullable: false),
                    DateTimeLastSetup = table.Column<DateTime>(type: "datetime2", maxLength: 100, nullable: false),
                    Owner = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CutterChangeInfo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FrequencyCheckResult = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CutterDataModelId = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Machines", x => x.Id);
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
                    SummaryNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Kind = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parts", x => x.Id);
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
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "Cutters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CutterNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CutterChangeComment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kind = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Owner = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Condition = table.Column<int>(type: "int", maxLength: 100, nullable: false),
                    CutterChangeInfo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
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
                name: "InfoUpdates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PublishDate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastUpdatedDate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Information = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kind = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PartNumberWithMove = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PressureAngleCoast = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PressureAngleDrive = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SpiralAngleCoast = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SpiralAngleDrive = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserDataModelId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoUpdates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InfoUpdates_Users_UserDataModelId",
                        column: x => x.UserDataModelId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MachineUserInteractions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineDataModelId = table.Column<int>(type: "int", nullable: true),
                    UserDataModelId = table.Column<int>(type: "int", nullable: true),
                    LastEntryDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserDataArchiveId = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineUserInteractions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MachineUserInteractions_Machines_MachineDataModelId",
                        column: x => x.MachineDataModelId,
                        principalTable: "Machines",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MachineUserInteractions_UsersArchive_UserDataArchiveId",
                        column: x => x.UserDataArchiveId,
                        principalTable: "UsersArchive",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MachineUserInteractions_Users_UserDataModelId",
                        column: x => x.UserDataModelId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CMMData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BeforeCorrections = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AfterCorrections = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PressureAngleCoast = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PressureAngleDrive = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SpiralAngleCoast = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SpiralAngleDrive = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Fr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Size = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Count = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CutterDataModelId = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMMData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CMMData_Cutters_CutterDataModelId",
                        column: x => x.CutterDataModelId,
                        principalTable: "Cutters",
                        principalColumn: "Id");
                });

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
                    UserFullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FrequencyCheckResult = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CurrentShift = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CutterChangeInfo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CMMDataId = table.Column<int>(type: "int", nullable: true),
                    DateTimeOfCheck = table.Column<DateTime>(type: "datetime2", maxLength: 100, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionPartsLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionPartsLog_CMMData_CMMDataId",
                        column: x => x.CMMDataId,
                        principalTable: "CMMData",
                        principalColumn: "Id");
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
                    CutterChangeInfo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CMMDataId = table.Column<int>(type: "int", nullable: true),
                    DateTimeOfCheck = table.Column<DateTime>(type: "datetime2", maxLength: 100, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionPartsLogDataArchive", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionPartsLogDataArchive_CMMData_CMMDataId",
                        column: x => x.CMMDataId,
                        principalTable: "CMMData",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CMMData_CutterDataModelId",
                table: "CMMData",
                column: "CutterDataModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Cutters_MachineDataModelId",
                table: "Cutters",
                column: "MachineDataModelId",
                unique: true,
                filter: "[MachineDataModelId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_InfoUpdates_UserDataModelId",
                table: "InfoUpdates",
                column: "UserDataModelId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineUserInteractions_MachineDataModelId",
                table: "MachineUserInteractions",
                column: "MachineDataModelId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineUserInteractions_UserDataArchiveId",
                table: "MachineUserInteractions",
                column: "UserDataArchiveId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineUserInteractions_UserDataModelId",
                table: "MachineUserInteractions",
                column: "UserDataModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPartsLog_CMMDataId",
                table: "ProductionPartsLog",
                column: "CMMDataId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPartsLogDataArchive_CMMDataId",
                table: "ProductionPartsLogDataArchive",
                column: "CMMDataId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InfoUpdates");

            migrationBuilder.DropTable(
                name: "MachineUserInteractions");

            migrationBuilder.DropTable(
                name: "Parts");

            migrationBuilder.DropTable(
                name: "ProductionPartsLog");

            migrationBuilder.DropTable(
                name: "ProductionPartsLogDataArchive");

            migrationBuilder.DropTable(
                name: "UsersArchive");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "CMMData");

            migrationBuilder.DropTable(
                name: "Cutters");

            migrationBuilder.DropTable(
                name: "Machines");
        }
    }
}
