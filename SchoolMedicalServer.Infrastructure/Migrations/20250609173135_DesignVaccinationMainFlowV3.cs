using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolMedicalServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DesignVaccinationMainFlowV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VaccinationSchedule_VaccineDetails_VaccineID",
                table: "VaccinationSchedule");

            migrationBuilder.DropIndex(
                name: "IX_VaccinationSchedule_VaccineID",
                table: "VaccinationSchedule");

            migrationBuilder.DropColumn(
                name: "TargetGrade",
                table: "VaccinationSchedule");

            migrationBuilder.AddColumn<string>(
                name: "TargetGrade",
                table: "VaccinationRound",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationSchedule_VaccineID",
                table: "VaccinationSchedule",
                column: "VaccineID",
                unique: true,
                filter: "[VaccineID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_VaccinationSchedule_VaccineDetails_VaccineID",
                table: "VaccinationSchedule",
                column: "VaccineID",
                principalTable: "VaccineDetails",
                principalColumn: "VaccineID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VaccinationSchedule_VaccineDetails_VaccineID",
                table: "VaccinationSchedule");

            migrationBuilder.DropIndex(
                name: "IX_VaccinationSchedule_VaccineID",
                table: "VaccinationSchedule");

            migrationBuilder.DropColumn(
                name: "TargetGrade",
                table: "VaccinationRound");

            migrationBuilder.AddColumn<string>(
                name: "TargetGrade",
                table: "VaccinationSchedule",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationSchedule_VaccineID",
                table: "VaccinationSchedule",
                column: "VaccineID");

            migrationBuilder.AddForeignKey(
                name: "FK_VaccinationSchedule_VaccineDetails_VaccineID",
                table: "VaccinationSchedule",
                column: "VaccineID",
                principalTable: "VaccineDetails",
                principalColumn: "VaccineID");
        }
    }
}
