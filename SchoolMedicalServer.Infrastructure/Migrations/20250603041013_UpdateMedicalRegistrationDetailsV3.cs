using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolMedicalServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMedicalRegistrationDetailsV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateApproved",
                table: "MedicalRegistration");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateApproved",
                table: "MedicalRegistrationDetails",
                type: "datetime",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateApproved",
                table: "MedicalRegistrationDetails");

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateApproved",
                table: "MedicalRegistration",
                type: "date",
                nullable: true);
        }
    }
}
