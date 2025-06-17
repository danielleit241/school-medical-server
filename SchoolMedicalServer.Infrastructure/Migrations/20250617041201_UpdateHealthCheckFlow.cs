using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolMedicalServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHealthCheckFlow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthCheckResult_HealthProfile_HealthProfileID",
                table: "HealthCheckResult");

            migrationBuilder.DropForeignKey(
                name: "FK__HealthChe__Sched__74AE54BC",
                table: "HealthCheckResult");

            migrationBuilder.DropForeignKey(
                name: "FK__HealthChe__Stude__6FE99F9F",
                table: "HealthCheckSchedule");

            migrationBuilder.DropPrimaryKey(
                name: "PK__HealthCh__9C8A5B69BE2ABA03",
                table: "HealthCheckSchedule");

            migrationBuilder.DropPrimaryKey(
                name: "PK__HealthCh__976902283B65C17C",
                table: "HealthCheckResult");

            migrationBuilder.DropIndex(
                name: "IX_HealthCheckResult_ScheduleID",
                table: "HealthCheckResult");

            migrationBuilder.DropColumn(
                name: "TargetGrade",
                table: "HealthCheckSchedule");

            migrationBuilder.DropColumn(
                name: "ScheduleID",
                table: "HealthCheckResult");

            migrationBuilder.RenameColumn(
                name: "StudentID",
                table: "HealthCheckSchedule",
                newName: "StudentId");

            migrationBuilder.RenameColumn(
                name: "ScheduleID",
                table: "HealthCheckSchedule",
                newName: "ScheduleId");

            migrationBuilder.RenameIndex(
                name: "IX_HealthCheckSchedule_StudentID",
                table: "HealthCheckSchedule",
                newName: "IX_HealthCheckSchedule_StudentId");

            migrationBuilder.RenameColumn(
                name: "RecordedID",
                table: "HealthCheckResult",
                newName: "RecordedId");

            migrationBuilder.RenameColumn(
                name: "HealthProfileID",
                table: "HealthCheckResult",
                newName: "HealthProfileId");

            migrationBuilder.RenameColumn(
                name: "ResultID",
                table: "HealthCheckResult",
                newName: "ResultId");

            migrationBuilder.RenameIndex(
                name: "IX_HealthCheckResult_HealthProfileID",
                table: "HealthCheckResult",
                newName: "IX_HealthCheckResult_HealthProfileId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "HealthCheckSchedule",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "HealthCheckSchedule",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "HealthCheckRoundRoundId",
                table: "HealthCheckSchedule",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "ParentNotificationEndDate",
                table: "HealthCheckSchedule",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "ParentNotificationStartDate",
                table: "HealthCheckSchedule",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "HealthCheckSchedule",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "HealthCheckSchedule",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "ParentConfirmed",
                table: "HealthCheckResult",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RoundId",
                table: "HealthCheckResult",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "HealthCheckResult",
                type: "bit",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_HealthCheckSchedule",
                table: "HealthCheckSchedule",
                column: "ScheduleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HealthCheckResult",
                table: "HealthCheckResult",
                column: "ResultId");

            migrationBuilder.CreateTable(
                name: "HealthCheckRound",
                columns: table => new
                {
                    RoundId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoundName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TargetGrade = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    NurseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthCheckRound", x => x.RoundId);
                    table.ForeignKey(
                        name: "FK_HealthCheckRound_HealthCheckSchedule_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "HealthCheckSchedule",
                        principalColumn: "ScheduleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HealthCheckSchedule_HealthCheckRoundRoundId",
                table: "HealthCheckSchedule",
                column: "HealthCheckRoundRoundId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthCheckResult_RoundId",
                table: "HealthCheckResult",
                column: "RoundId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthCheckRound_ScheduleId",
                table: "HealthCheckRound",
                column: "ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthCheckResult_HealthCheckRound_RoundId",
                table: "HealthCheckResult",
                column: "RoundId",
                principalTable: "HealthCheckRound",
                principalColumn: "RoundId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthCheckResult_HealthProfile_HealthProfileId",
                table: "HealthCheckResult",
                column: "HealthProfileId",
                principalTable: "HealthProfile",
                principalColumn: "HealthProfileID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthCheckSchedule_HealthCheckRound_HealthCheckRoundRoundId",
                table: "HealthCheckSchedule",
                column: "HealthCheckRoundRoundId",
                principalTable: "HealthCheckRound",
                principalColumn: "RoundId");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthCheckSchedule_Student_StudentId",
                table: "HealthCheckSchedule",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "StudentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthCheckResult_HealthCheckRound_RoundId",
                table: "HealthCheckResult");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthCheckResult_HealthProfile_HealthProfileId",
                table: "HealthCheckResult");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthCheckSchedule_HealthCheckRound_HealthCheckRoundRoundId",
                table: "HealthCheckSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthCheckSchedule_Student_StudentId",
                table: "HealthCheckSchedule");

            migrationBuilder.DropTable(
                name: "HealthCheckRound");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HealthCheckSchedule",
                table: "HealthCheckSchedule");

            migrationBuilder.DropIndex(
                name: "IX_HealthCheckSchedule_HealthCheckRoundRoundId",
                table: "HealthCheckSchedule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HealthCheckResult",
                table: "HealthCheckResult");

            migrationBuilder.DropIndex(
                name: "IX_HealthCheckResult_RoundId",
                table: "HealthCheckResult");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "HealthCheckSchedule");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "HealthCheckSchedule");

            migrationBuilder.DropColumn(
                name: "HealthCheckRoundRoundId",
                table: "HealthCheckSchedule");

            migrationBuilder.DropColumn(
                name: "ParentNotificationEndDate",
                table: "HealthCheckSchedule");

            migrationBuilder.DropColumn(
                name: "ParentNotificationStartDate",
                table: "HealthCheckSchedule");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "HealthCheckSchedule");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "HealthCheckSchedule");

            migrationBuilder.DropColumn(
                name: "ParentConfirmed",
                table: "HealthCheckResult");

            migrationBuilder.DropColumn(
                name: "RoundId",
                table: "HealthCheckResult");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "HealthCheckResult");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "HealthCheckSchedule",
                newName: "StudentID");

            migrationBuilder.RenameColumn(
                name: "ScheduleId",
                table: "HealthCheckSchedule",
                newName: "ScheduleID");

            migrationBuilder.RenameIndex(
                name: "IX_HealthCheckSchedule_StudentId",
                table: "HealthCheckSchedule",
                newName: "IX_HealthCheckSchedule_StudentID");

            migrationBuilder.RenameColumn(
                name: "RecordedId",
                table: "HealthCheckResult",
                newName: "RecordedID");

            migrationBuilder.RenameColumn(
                name: "HealthProfileId",
                table: "HealthCheckResult",
                newName: "HealthProfileID");

            migrationBuilder.RenameColumn(
                name: "ResultId",
                table: "HealthCheckResult",
                newName: "ResultID");

            migrationBuilder.RenameIndex(
                name: "IX_HealthCheckResult_HealthProfileId",
                table: "HealthCheckResult",
                newName: "IX_HealthCheckResult_HealthProfileID");

            migrationBuilder.AddColumn<string>(
                name: "TargetGrade",
                table: "HealthCheckSchedule",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ScheduleID",
                table: "HealthCheckResult",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK__HealthCh__9C8A5B69BE2ABA03",
                table: "HealthCheckSchedule",
                column: "ScheduleID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__HealthCh__976902283B65C17C",
                table: "HealthCheckResult",
                column: "ResultID");

            migrationBuilder.CreateIndex(
                name: "IX_HealthCheckResult_ScheduleID",
                table: "HealthCheckResult",
                column: "ScheduleID");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthCheckResult_HealthProfile_HealthProfileID",
                table: "HealthCheckResult",
                column: "HealthProfileID",
                principalTable: "HealthProfile",
                principalColumn: "HealthProfileID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__HealthChe__Sched__74AE54BC",
                table: "HealthCheckResult",
                column: "ScheduleID",
                principalTable: "HealthCheckSchedule",
                principalColumn: "ScheduleID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__HealthChe__Stude__6FE99F9F",
                table: "HealthCheckSchedule",
                column: "StudentID",
                principalTable: "Student",
                principalColumn: "StudentID");
        }
    }
}
