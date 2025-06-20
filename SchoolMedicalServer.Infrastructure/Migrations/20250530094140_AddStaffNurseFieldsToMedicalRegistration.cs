using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolMedicalServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStaffNurseFieldsToMedicalRegistration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "DateApproved",
                table: "MedicalRegistration",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StaffNurseID",
                table: "MedicalRegistration",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StaffNurseNotes",
                table: "MedicalRegistration",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Student_StudentCode_Unique",
                table: "Student",
                column: "StudentCode",
                unique: true,
                filter: "[StudentCode] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Student_StudentCode_Unique",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "DateApproved",
                table: "MedicalRegistration");

            migrationBuilder.DropColumn(
                name: "StaffNurseID",
                table: "MedicalRegistration");

            migrationBuilder.DropColumn(
                name: "StaffNurseNotes",
                table: "MedicalRegistration");
        }
    }
}
