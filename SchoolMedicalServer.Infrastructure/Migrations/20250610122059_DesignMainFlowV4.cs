using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolMedicalServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DesignMainFlowV4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VaccinationResult_VaccinationSchedule_ScheduleId",
                table: "VaccinationResult");

            migrationBuilder.DropForeignKey(
                name: "FK_VaccinationSchedule_Student_StudentID",
                table: "VaccinationSchedule");

            migrationBuilder.DropIndex(
                name: "IX_VaccinationResult_ScheduleId",
                table: "VaccinationResult");

            migrationBuilder.DropColumn(
                name: "ParentConfirmed",
                table: "VaccinationSchedule");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "VaccinationResult");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "VaccinationResult");

            migrationBuilder.RenameColumn(
                name: "StudentID",
                table: "VaccinationSchedule",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_VaccinationSchedule_StudentID",
                table: "VaccinationSchedule",
                newName: "IX_VaccinationSchedule_StudentId");

            migrationBuilder.RenameColumn(
                name: "VaccinationDate",
                table: "VaccinationResult",
                newName: "VaccinatedDate");

            migrationBuilder.AlterColumn<Guid>(
                name: "RecorderID",
                table: "VaccinationResult",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HealthQualified",
                table: "VaccinationResult",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ParentConfirmed",
                table: "VaccinationResult",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Vaccinated",
                table: "VaccinationResult",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationResult_RoundID",
                table: "VaccinationResult",
                column: "RoundID");

            migrationBuilder.AddForeignKey(
                name: "FK_VaccinationResult_VaccinationRound_RoundID",
                table: "VaccinationResult",
                column: "RoundID",
                principalTable: "VaccinationRound",
                principalColumn: "RoundID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VaccinationSchedule_Student_StudentId",
                table: "VaccinationSchedule",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "StudentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VaccinationResult_VaccinationRound_RoundID",
                table: "VaccinationResult");

            migrationBuilder.DropForeignKey(
                name: "FK_VaccinationSchedule_Student_StudentId",
                table: "VaccinationSchedule");

            migrationBuilder.DropIndex(
                name: "IX_VaccinationResult_RoundID",
                table: "VaccinationResult");

            migrationBuilder.DropColumn(
                name: "HealthQualified",
                table: "VaccinationResult");

            migrationBuilder.DropColumn(
                name: "ParentConfirmed",
                table: "VaccinationResult");

            migrationBuilder.DropColumn(
                name: "Vaccinated",
                table: "VaccinationResult");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "VaccinationSchedule",
                newName: "StudentID");

            migrationBuilder.RenameIndex(
                name: "IX_VaccinationSchedule_StudentId",
                table: "VaccinationSchedule",
                newName: "IX_VaccinationSchedule_StudentID");

            migrationBuilder.RenameColumn(
                name: "VaccinatedDate",
                table: "VaccinationResult",
                newName: "VaccinationDate");

            migrationBuilder.AddColumn<bool>(
                name: "ParentConfirmed",
                table: "VaccinationSchedule",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<Guid>(
                name: "RecorderID",
                table: "VaccinationResult",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "ScheduleId",
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

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationResult_ScheduleId",
                table: "VaccinationResult",
                column: "ScheduleId");

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
        }
    }
}
