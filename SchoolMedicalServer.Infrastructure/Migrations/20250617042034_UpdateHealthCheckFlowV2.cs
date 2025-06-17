using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolMedicalServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHealthCheckFlowV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthCheckSchedule_HealthCheckRound_HealthCheckRoundRoundId",
                table: "HealthCheckSchedule");

            migrationBuilder.DropIndex(
                name: "IX_HealthCheckSchedule_HealthCheckRoundRoundId",
                table: "HealthCheckSchedule");

            migrationBuilder.DropColumn(
                name: "HealthCheckRoundRoundId",
                table: "HealthCheckSchedule");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthCheckResult_HealthCheckRound_HealthCheckRoundRoundId",
                table: "HealthCheckResult");

            migrationBuilder.DropIndex(
                name: "IX_HealthCheckResult_HealthCheckRoundRoundId",
                table: "HealthCheckResult");

            migrationBuilder.DropColumn(
                name: "HealthCheckRoundRoundId",
                table: "HealthCheckResult");

            migrationBuilder.AddColumn<Guid>(
                name: "HealthCheckRoundRoundId",
                table: "HealthCheckSchedule",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HealthCheckSchedule_HealthCheckRoundRoundId",
                table: "HealthCheckSchedule",
                column: "HealthCheckRoundRoundId");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthCheckSchedule_HealthCheckRound_HealthCheckRoundRoundId",
                table: "HealthCheckSchedule",
                column: "HealthCheckRoundRoundId",
                principalTable: "HealthCheckRound",
                principalColumn: "RoundId");
        }
    }
}
