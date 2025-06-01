using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolMedicalServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMedicalEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "MedicalRequest");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "MedicalEvent",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "MedicalEvent");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "MedicalRequest",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }
    }
}
