using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolMedicalServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DesignMainFlowRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VaccinationSchedule_Student_StudentId",
                table: "VaccinationSchedule");

            migrationBuilder.DropIndex(
                name: "IX_VaccinationSchedule_StudentId",
                table: "VaccinationSchedule");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "VaccinationSchedule");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StudentId",
                table: "VaccinationSchedule",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationSchedule_StudentId",
                table: "VaccinationSchedule",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_VaccinationSchedule_Student_StudentId",
                table: "VaccinationSchedule",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "StudentID");
        }
    }
}
