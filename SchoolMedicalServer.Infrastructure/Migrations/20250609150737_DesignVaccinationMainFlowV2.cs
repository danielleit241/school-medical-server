using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolMedicalServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DesignVaccinationMainFlowV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthCheckSchedule_User_UserId",
                table: "HealthCheckSchedule");

            migrationBuilder.DropIndex(
                name: "IX_HealthCheckSchedule_UserId",
                table: "HealthCheckSchedule");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "HealthCheckSchedule");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "HealthCheckSchedule",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HealthCheckSchedule_UserId",
                table: "HealthCheckSchedule",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthCheckSchedule_User_UserId",
                table: "HealthCheckSchedule",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserID");
        }
    }
}
