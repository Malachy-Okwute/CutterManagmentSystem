using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CutterManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedCMMDataModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CMMDataModelId",
                table: "Machines",
                type: "int",
                nullable: true);

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
                    MachineDataModelId = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMMData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CMMData_Machines_MachineDataModelId",
                        column: x => x.MachineDataModelId,
                        principalTable: "Machines",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CMMData_MachineDataModelId",
                table: "CMMData",
                column: "MachineDataModelId",
                unique: true,
                filter: "[MachineDataModelId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CMMData");

            migrationBuilder.DropColumn(
                name: "CMMDataModelId",
                table: "Machines");
        }
    }
}
