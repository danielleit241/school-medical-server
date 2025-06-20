using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolMedicalServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStartAndEndDateInRound : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "VaccinationRound");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "VaccinationRound");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "EndDate",
                table: "VaccinationRound",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "StartDate",
                table: "VaccinationRound",
                type: "date",
                nullable: true);
        }
    }
}
