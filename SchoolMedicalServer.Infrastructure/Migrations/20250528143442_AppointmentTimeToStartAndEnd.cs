using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolMedicalServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AppointmentTimeToStartAndEnd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AppointmentTime",
                table: "Appointment",
                newName: "AppointmentStartTime");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "AppointmentEndTime",
                table: "Appointment",
                type: "time",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentEndTime",
                table: "Appointment");

            migrationBuilder.RenameColumn(
                name: "AppointmentStartTime",
                table: "Appointment",
                newName: "AppointmentTime");
        }
    }
}
