using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolMedicalServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationshipScheduleAndVaccineDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VaccinationSchedule_VaccineID",
                table: "VaccinationSchedule");

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationSchedule_VaccineID",
                table: "VaccinationSchedule",
                column: "VaccineID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VaccinationSchedule_VaccineID",
                table: "VaccinationSchedule");

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationSchedule_VaccineID",
                table: "VaccinationSchedule",
                column: "VaccineID",
                unique: true,
                filter: "[VaccineID] IS NOT NULL");
        }
    }
}
