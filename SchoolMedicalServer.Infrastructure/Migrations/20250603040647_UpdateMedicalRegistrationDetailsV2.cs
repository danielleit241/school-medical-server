using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolMedicalServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMedicalRegistrationDetailsV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StaffNurseID",
                table: "MedicalRegistration");

            migrationBuilder.DropColumn(
                name: "StaffNurseNotes",
                table: "MedicalRegistration");

            migrationBuilder.AddColumn<Guid>(
                name: "StaffNurseID",
                table: "MedicalRegistrationDetails",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StaffNurseID",
                table: "MedicalRegistrationDetails");

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
        }
    }
}
