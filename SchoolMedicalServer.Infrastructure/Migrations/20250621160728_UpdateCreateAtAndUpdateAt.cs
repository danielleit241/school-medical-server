using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolMedicalServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCreateAtAndUpdateAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                table: "User",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "User",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                table: "Student",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "Student",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "VaccinationRound",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "VaccinationRound",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "VaccinationResult",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "VaccinationResult",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "VaccinationObservation",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "HealthCheckRound",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "HealthCheckRound",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "HealthCheckResult",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "HealthCheckResult",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "VaccinationRound");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "VaccinationRound");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "VaccinationResult");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "VaccinationResult");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "VaccinationObservation");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "HealthCheckRound");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "HealthCheckRound");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "HealthCheckResult");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "HealthCheckResult");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "User",
                newName: "UpdateAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "User",
                newName: "CreateAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Student",
                newName: "UpdateAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Student",
                newName: "CreateAt");
        }
    }
}
