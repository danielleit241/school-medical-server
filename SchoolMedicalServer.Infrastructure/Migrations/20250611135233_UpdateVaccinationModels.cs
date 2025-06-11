using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolMedicalServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVaccinationModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VaccinationObservations",
                table: "VaccinationObservations");

            migrationBuilder.DropColumn(
                name: "HealthQualified",
                table: "VaccinationResult");

            migrationBuilder.RenameTable(
                name: "VaccinationObservations",
                newName: "VaccinationObservation");

            migrationBuilder.RenameIndex(
                name: "IX_VaccinationObservations_VaccinationResultID",
                table: "VaccinationObservation",
                newName: "IX_VaccinationObservation_VaccinationResultID");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "VaccinationSchedule",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "VaccinationSchedule",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateOnly>(
                name: "EndDate",
                table: "VaccinationSchedule",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "StartDate",
                table: "VaccinationSchedule",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NurseId",
                table: "VaccinationRound",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "VaccinationResult",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VaccinationObservation",
                table: "VaccinationObservation",
                column: "VaccinationObservationID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VaccinationObservation",
                table: "VaccinationObservation");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "VaccinationSchedule");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "VaccinationSchedule");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "VaccinationSchedule");

            migrationBuilder.DropColumn(
                name: "NurseId",
                table: "VaccinationRound");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "VaccinationResult");

            migrationBuilder.RenameTable(
                name: "VaccinationObservation",
                newName: "VaccinationObservations");

            migrationBuilder.RenameIndex(
                name: "IX_VaccinationObservation_VaccinationResultID",
                table: "VaccinationObservations",
                newName: "IX_VaccinationObservations_VaccinationResultID");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "VaccinationSchedule",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<bool>(
                name: "HealthQualified",
                table: "VaccinationResult",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VaccinationObservations",
                table: "VaccinationObservations",
                column: "VaccinationObservationID");
        }
    }
}
