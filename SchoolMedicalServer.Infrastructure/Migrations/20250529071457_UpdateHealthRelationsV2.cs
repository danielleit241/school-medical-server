using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolMedicalServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHealthRelationsV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthDeclaration_HealthProfile_HealthProfileId1",
                table: "HealthDeclaration");

            migrationBuilder.DropIndex(
                name: "IX_HealthDeclaration_HealthProfileId1",
                table: "HealthDeclaration");

            migrationBuilder.DropColumn(
                name: "HealthCheckResultID",
                table: "HealthProfile");

            migrationBuilder.DropColumn(
                name: "VaccinationResultID",
                table: "HealthProfile");

            migrationBuilder.DropColumn(
                name: "HealthProfileId1",
                table: "HealthDeclaration");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "HealthCheckResultID",
                table: "HealthProfile",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "VaccinationResultID",
                table: "HealthProfile",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "HealthProfileId1",
                table: "HealthDeclaration",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_HealthDeclaration_HealthProfileId1",
                table: "HealthDeclaration",
                column: "HealthProfileId1");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthDeclaration_HealthProfile_HealthProfileId1",
                table: "HealthDeclaration",
                column: "HealthProfileId1",
                principalTable: "HealthProfile",
                principalColumn: "HealthProfileID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
