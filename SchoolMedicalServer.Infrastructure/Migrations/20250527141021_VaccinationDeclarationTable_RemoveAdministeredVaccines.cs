using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolMedicalServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class VaccinationDeclarationTable_RemoveAdministeredVaccines : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdministeredVaccines",
                table: "HealthDeclaration");

            migrationBuilder.CreateTable(
                name: "VaccinationDeclaration",
                columns: table => new
                {
                    VaccinationDeclarationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HealthDeclarationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VaccineName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BatchNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VaccinatedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__VaccinationDeclaration", x => x.VaccinationDeclarationID);
                    table.ForeignKey(
                        name: "FK_VaccinationDeclaration_HealthDeclaration",
                        column: x => x.HealthDeclarationID,
                        principalTable: "HealthDeclaration",
                        principalColumn: "HealthDeclarationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "RoleID",
                keyValue: 2,
                column: "RoleName",
                value: "nurse");

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationDeclaration_HealthDeclarationID",
                table: "VaccinationDeclaration",
                column: "HealthDeclarationID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VaccinationDeclaration");

            migrationBuilder.AddColumn<string>(
                name: "AdministeredVaccines",
                table: "HealthDeclaration",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "RoleID",
                keyValue: 2,
                column: "RoleName",
                value: "staff");
        }
    }
}
