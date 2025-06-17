using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolMedicalServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRelationshipBetweenHealthCheckSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthCheckSchedule_Student_StudentId",
                table: "HealthCheckSchedule");

            migrationBuilder.DropIndex(
                name: "IX_HealthCheckSchedule_StudentId",
                table: "HealthCheckSchedule");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "HealthCheckSchedule");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StudentId",
                table: "HealthCheckSchedule",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HealthCheckSchedule_StudentId",
                table: "HealthCheckSchedule",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthCheckSchedule_Student_StudentId",
                table: "HealthCheckSchedule",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "StudentID");
        }
    }
}
