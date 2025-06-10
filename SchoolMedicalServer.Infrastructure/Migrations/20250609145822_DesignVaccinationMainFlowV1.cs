using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolMedicalServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DesignVaccinationMainFlowV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthCheckResult_Student_StudentId",
                table: "HealthCheckResult");

            migrationBuilder.DropForeignKey(
                name: "FK__HealthChe__Sched__74AE54BC",
                table: "HealthCheckResult");

            migrationBuilder.DropForeignKey(
                name: "FK__HealthChe__UserI__70DDC3D8",
                table: "HealthCheckSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_VaccinationResult_Student_StudentId",
                table: "VaccinationResult");

            migrationBuilder.DropForeignKey(
                name: "FK__Vaccinati__Sched__6D0D32F4",
                table: "VaccinationResult");

            migrationBuilder.DropForeignKey(
                name: "FK__Vaccinati__Stude__68487DD7",
                table: "VaccinationSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK__Vaccinati__Vacci__693CA210",
                table: "VaccinationSchedule");

            migrationBuilder.DropIndex(
                name: "IX_VaccinationResult_StudentId",
                table: "VaccinationResult");

            migrationBuilder.DropIndex(
                name: "IX_HealthCheckResult_StudentId",
                table: "HealthCheckResult");

            migrationBuilder.DropColumn(
                name: "Disease",
                table: "VaccineDetails");

            migrationBuilder.DropColumn(
                name: "Round",
                table: "VaccinationSchedule");

            migrationBuilder.DropColumn(
                name: "DoseNumber",
                table: "VaccinationResult");

            migrationBuilder.DropColumn(
                name: "ImmediateReaction",
                table: "VaccinationResult");

            migrationBuilder.DropColumn(
                name: "InjectionSite",
                table: "VaccinationResult");

            migrationBuilder.DropColumn(
                name: "ReactionStartTime",
                table: "VaccinationResult");

            migrationBuilder.DropColumn(
                name: "ReactionType",
                table: "VaccinationResult");

            migrationBuilder.DropColumn(
                name: "RecordedID",
                table: "VaccinationResult");

            migrationBuilder.DropColumn(
                name: "SeverityLevel",
                table: "VaccinationResult");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "HealthCheckResult");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "VaccinationSchedule",
                newName: "ParentNotificationStartDate");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "VaccinationSchedule",
                newName: "ParentNotificationEndDate");

            migrationBuilder.RenameColumn(
                name: "ScheduleID",
                table: "VaccinationResult",
                newName: "ScheduleId");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "VaccinationResult",
                newName: "RecorderID");

            migrationBuilder.RenameIndex(
                name: "IX_VaccinationResult_ScheduleID",
                table: "VaccinationResult",
                newName: "IX_VaccinationResult_ScheduleId");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "HealthCheckSchedule",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_HealthCheckSchedule_UserID",
                table: "HealthCheckSchedule",
                newName: "IX_HealthCheckSchedule_UserId");

            migrationBuilder.AddColumn<string>(
                name: "VaccineCode",
                table: "VaccineDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "ParentConfirmed",
                table: "VaccinationSchedule",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<Guid>(
                name: "ScheduleId",
                table: "VaccinationResult",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RoundID",
                table: "VaccinationResult",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "VaccinationResult",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VaccinationObservations",
                columns: table => new
                {
                    VaccinationObservationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VaccinationResultID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReactionStartTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    ReactionType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SeverityLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ImmediateReaction = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccinationObservations", x => x.VaccinationObservationID);
                    table.ForeignKey(
                        name: "FK_VaccinationObservation_VaccinationResult",
                        column: x => x.VaccinationResultID,
                        principalTable: "VaccinationResult",
                        principalColumn: "VaccinationResultID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VaccinationRound",
                columns: table => new
                {
                    RoundID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoundName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccinationRound", x => x.RoundID);
                    table.ForeignKey(
                        name: "FK_VaccinationRound_VaccinationSchedule",
                        column: x => x.ScheduleID,
                        principalTable: "VaccinationSchedule",
                        principalColumn: "ScheduleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationObservations_VaccinationResultID",
                table: "VaccinationObservations",
                column: "VaccinationResultID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationRound_ScheduleID",
                table: "VaccinationRound",
                column: "ScheduleID");

            migrationBuilder.AddForeignKey(
                name: "FK__HealthChe__Sched__74AE54BC",
                table: "HealthCheckResult",
                column: "ScheduleID",
                principalTable: "HealthCheckSchedule",
                principalColumn: "ScheduleID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthCheckSchedule_User_UserId",
                table: "HealthCheckSchedule",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_VaccinationResult_VaccinationSchedule_ScheduleId",
                table: "VaccinationResult",
                column: "ScheduleId",
                principalTable: "VaccinationSchedule",
                principalColumn: "ScheduleID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VaccinationSchedule_Student_StudentID",
                table: "VaccinationSchedule",
                column: "StudentID",
                principalTable: "Student",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_VaccinationSchedule_VaccineDetails_VaccineID",
                table: "VaccinationSchedule",
                column: "VaccineID",
                principalTable: "VaccineDetails",
                principalColumn: "VaccineID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__HealthChe__Sched__74AE54BC",
                table: "HealthCheckResult");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthCheckSchedule_User_UserId",
                table: "HealthCheckSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_VaccinationResult_VaccinationSchedule_ScheduleId",
                table: "VaccinationResult");

            migrationBuilder.DropForeignKey(
                name: "FK_VaccinationSchedule_Student_StudentID",
                table: "VaccinationSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_VaccinationSchedule_VaccineDetails_VaccineID",
                table: "VaccinationSchedule");

            migrationBuilder.DropTable(
                name: "VaccinationObservations");

            migrationBuilder.DropTable(
                name: "VaccinationRound");

            migrationBuilder.DropColumn(
                name: "VaccineCode",
                table: "VaccineDetails");

            migrationBuilder.DropColumn(
                name: "ParentConfirmed",
                table: "VaccinationSchedule");

            migrationBuilder.DropColumn(
                name: "RoundID",
                table: "VaccinationResult");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "VaccinationResult");

            migrationBuilder.RenameColumn(
                name: "ParentNotificationStartDate",
                table: "VaccinationSchedule",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "ParentNotificationEndDate",
                table: "VaccinationSchedule",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "ScheduleId",
                table: "VaccinationResult",
                newName: "ScheduleID");

            migrationBuilder.RenameColumn(
                name: "RecorderID",
                table: "VaccinationResult",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_VaccinationResult_ScheduleId",
                table: "VaccinationResult",
                newName: "IX_VaccinationResult_ScheduleID");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "HealthCheckSchedule",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_HealthCheckSchedule_UserId",
                table: "HealthCheckSchedule",
                newName: "IX_HealthCheckSchedule_UserID");

            migrationBuilder.AddColumn<string>(
                name: "Disease",
                table: "VaccineDetails",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Round",
                table: "VaccinationSchedule",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ScheduleID",
                table: "VaccinationResult",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<int>(
                name: "DoseNumber",
                table: "VaccinationResult",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImmediateReaction",
                table: "VaccinationResult",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InjectionSite",
                table: "VaccinationResult",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReactionStartTime",
                table: "VaccinationResult",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReactionType",
                table: "VaccinationResult",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RecordedID",
                table: "VaccinationResult",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeverityLevel",
                table: "VaccinationResult",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StudentId",
                table: "HealthCheckResult",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationResult_StudentId",
                table: "VaccinationResult",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthCheckResult_StudentId",
                table: "HealthCheckResult",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthCheckResult_Student_StudentId",
                table: "HealthCheckResult",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK__HealthChe__Sched__74AE54BC",
                table: "HealthCheckResult",
                column: "ScheduleID",
                principalTable: "HealthCheckSchedule",
                principalColumn: "ScheduleID");

            migrationBuilder.AddForeignKey(
                name: "FK__HealthChe__UserI__70DDC3D8",
                table: "HealthCheckSchedule",
                column: "UserID",
                principalTable: "User",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_VaccinationResult_Student_StudentId",
                table: "VaccinationResult",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK__Vaccinati__Sched__6D0D32F4",
                table: "VaccinationResult",
                column: "ScheduleID",
                principalTable: "VaccinationSchedule",
                principalColumn: "ScheduleID");

            migrationBuilder.AddForeignKey(
                name: "FK__Vaccinati__Stude__68487DD7",
                table: "VaccinationSchedule",
                column: "StudentID",
                principalTable: "Student",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK__Vaccinati__Vacci__693CA210",
                table: "VaccinationSchedule",
                column: "VaccineID",
                principalTable: "VaccineDetails",
                principalColumn: "VaccineID");
        }
    }
}
