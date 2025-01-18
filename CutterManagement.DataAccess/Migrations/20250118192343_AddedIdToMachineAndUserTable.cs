using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CutterManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedIdToMachineAndUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MachineDataModelUserDataModels",
                table: "MachineDataModelUserDataModels");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "MachineDataModelUserDataModels",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "MachineDataModelUserDataModels",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_MachineDataModelUserDataModels",
                table: "MachineDataModelUserDataModels",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MachineDataModelUserDataModels_MachineDataModelId",
                table: "MachineDataModelUserDataModels",
                column: "MachineDataModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MachineDataModelUserDataModels",
                table: "MachineDataModelUserDataModels");

            migrationBuilder.DropIndex(
                name: "IX_MachineDataModelUserDataModels_MachineDataModelId",
                table: "MachineDataModelUserDataModels");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MachineDataModelUserDataModels");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "MachineDataModelUserDataModels");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MachineDataModelUserDataModels",
                table: "MachineDataModelUserDataModels",
                columns: new[] { "MachineDataModelId", "UserDataModelId" });
        }
    }
}
