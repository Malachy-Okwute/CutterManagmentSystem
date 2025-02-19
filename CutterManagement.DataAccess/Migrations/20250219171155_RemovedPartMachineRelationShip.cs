using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CutterManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemovedPartMachineRelationShip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MachineDataModelPartDataModel");

            migrationBuilder.AddColumn<int>(
                name: "PartDataModelId",
                table: "Machines",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Machines_PartDataModelId",
                table: "Machines",
                column: "PartDataModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Parts_PartDataModelId",
                table: "Machines",
                column: "PartDataModelId",
                principalTable: "Parts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Machines_Parts_PartDataModelId",
                table: "Machines");

            migrationBuilder.DropIndex(
                name: "IX_Machines_PartDataModelId",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "PartDataModelId",
                table: "Machines");

            migrationBuilder.CreateTable(
                name: "MachineDataModelPartDataModel",
                columns: table => new
                {
                    MachineDataModelId = table.Column<int>(type: "int", nullable: false),
                    PartsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineDataModelPartDataModel", x => new { x.MachineDataModelId, x.PartsId });
                    table.ForeignKey(
                        name: "FK_MachineDataModelPartDataModel_Machines_MachineDataModelId",
                        column: x => x.MachineDataModelId,
                        principalTable: "Machines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MachineDataModelPartDataModel_Parts_PartsId",
                        column: x => x.PartsId,
                        principalTable: "Parts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MachineDataModelPartDataModel_PartsId",
                table: "MachineDataModelPartDataModel",
                column: "PartsId");
        }
    }
}
