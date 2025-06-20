using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolMedicalServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMedicalRegistrationDetailsV4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateApproved",
                table: "MedicalRegistrationDetails",
                newName: "DateCompleted");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateApproved",
                table: "MedicalRegistration");

            migrationBuilder.DropColumn(
                name: "StaffNurseID",
                table: "MedicalRegistration");

            migrationBuilder.RenameColumn(
                name: "DateCompleted",
                table: "MedicalRegistrationDetails",
                newName: "DateApproved");
        }
    }
}
