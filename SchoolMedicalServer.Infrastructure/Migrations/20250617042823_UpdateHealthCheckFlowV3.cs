using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolMedicalServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHealthCheckFlowV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthCheckResult_HealthCheckRound_HealthCheckRoundRoundId",
                table: "HealthCheckResult");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthCheckResult_HealthCheckRound_RoundId",
                table: "HealthCheckResult");

            migrationBuilder.DropIndex(
                name: "IX_HealthCheckResult_HealthCheckRoundRoundId",
                table: "HealthCheckResult");

            migrationBuilder.DropColumn(
                name: "HealthCheckRoundRoundId",
                table: "HealthCheckResult");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthCheckResult_HealthCheckRound_RoundId",
                table: "HealthCheckResult",
                column: "RoundId",
                principalTable: "HealthCheckRound",
                principalColumn: "RoundId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthCheckResult_HealthCheckRound_RoundId",
                table: "HealthCheckResult");

            migrationBuilder.AddColumn<Guid>(
                name: "HealthCheckRoundRoundId",
                table: "HealthCheckResult",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HealthCheckResult_HealthCheckRoundRoundId",
                table: "HealthCheckResult",
                column: "HealthCheckRoundRoundId");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthCheckResult_HealthCheckRound_HealthCheckRoundRoundId",
                table: "HealthCheckResult",
                column: "HealthCheckRoundRoundId",
                principalTable: "HealthCheckRound",
                principalColumn: "RoundId");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthCheckResult_HealthCheckRound_RoundId",
                table: "HealthCheckResult",
                column: "RoundId",
                principalTable: "HealthCheckRound",
                principalColumn: "RoundId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
