using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CutterManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Machines_MachineDataModelId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_MachineDataModelId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "UserDataModelId",
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
                name: "IX_MachineDataModelUserDataModel_UsersId",
                table: "MachineDataModelUserDataModel",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MachineDataModelUserDataModel");

            migrationBuilder.DropColumn(
                name: "UserDataModelId",
                table: "Machines");

            migrationBuilder.CreateIndex(
                name: "IX_Users_MachineDataModelId",
                table: "Users",
                column: "MachineDataModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Machines_MachineDataModelId",
                table: "Users",
                column: "MachineDataModelId",
                principalTable: "Machines",
                principalColumn: "Id");
        }
    }
}
