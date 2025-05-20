using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CutterManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseRelationshipRestructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CMMData_Machines_MachineDataModelId",
                table: "CMMData");

            migrationBuilder.DropTable(
                name: "MachineDataModelUserDataModel");

            migrationBuilder.DropIndex(
                name: "IX_CMMData_MachineDataModelId",
                table: "CMMData");

            migrationBuilder.DropColumn(
                name: "CMMDataModelId",
                table: "Machines");

            migrationBuilder.RenameColumn(
                name: "MachineDataModelId",
                table: "CMMData",
                newName: "CutterDataModelId");

            migrationBuilder.AddColumn<int>(
                name: "CMMDataModelId",
                table: "Cutters",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MachineUserInteractions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineDataModelId = table.Column<int>(type: "int", nullable: true),
                    UserDataModelId = table.Column<int>(type: "int", nullable: true),
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
                        name: "FK_MachineUserInteractions_Users_UserDataModelId",
                        column: x => x.UserDataModelId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CMMData_CutterDataModelId",
                table: "CMMData",
                column: "CutterDataModelId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineUserInteractions_MachineDataModelId",
                table: "MachineUserInteractions",
                column: "MachineDataModelId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineUserInteractions_UserDataModelId",
                table: "MachineUserInteractions",
                column: "UserDataModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_CMMData_Cutters_CutterDataModelId",
                table: "CMMData",
                column: "CutterDataModelId",
                principalTable: "Cutters",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CMMData_Cutters_CutterDataModelId",
                table: "CMMData");

            migrationBuilder.DropTable(
                name: "MachineUserInteractions");

            migrationBuilder.DropIndex(
                name: "IX_CMMData_CutterDataModelId",
                table: "CMMData");

            migrationBuilder.DropColumn(
                name: "CMMDataModelId",
                table: "Cutters");

            migrationBuilder.RenameColumn(
                name: "CutterDataModelId",
                table: "CMMData",
                newName: "MachineDataModelId");

            migrationBuilder.AddColumn<int>(
                name: "CMMDataModelId",
                table: "Machines",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MachineDataModelUserDataModel",
                columns: table => new
                {
                    MachineDataModelId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineDataModelUserDataModel", x => new { x.MachineDataModelId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_MachineDataModelUserDataModel_Machines_MachineDataModelId",
                        column: x => x.MachineDataModelId,
                        principalTable: "Machines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MachineDataModelUserDataModel_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CMMData_MachineDataModelId",
                table: "CMMData",
                column: "MachineDataModelId",
                unique: true,
                filter: "[MachineDataModelId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MachineDataModelUserDataModel_UsersId",
                table: "MachineDataModelUserDataModel",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_CMMData_Machines_MachineDataModelId",
                table: "CMMData",
                column: "MachineDataModelId",
                principalTable: "Machines",
                principalColumn: "Id");
        }
    }
}
