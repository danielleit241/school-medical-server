using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolMedicalServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVaccinationResultAndObservationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InjectionSite",
                table: "VaccinationResult",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Intervention",
                table: "VaccinationObservation",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ObservationEndTime",
                table: "VaccinationObservation",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ObservationStartTime",
                table: "VaccinationObservation",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObservedBy",
                table: "VaccinationObservation",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InjectionSite",
                table: "VaccinationResult");

            migrationBuilder.DropColumn(
                name: "Intervention",
                table: "VaccinationObservation");

            migrationBuilder.DropColumn(
                name: "ObservationEndTime",
                table: "VaccinationObservation");

            migrationBuilder.DropColumn(
                name: "ObservationStartTime",
                table: "VaccinationObservation");

            migrationBuilder.DropColumn(
                name: "ObservedBy",
                table: "VaccinationObservation");
        }
    }
}
