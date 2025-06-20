using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolMedicalServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHealthRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__HealthChe__Stude__73BA3083",
                table: "HealthCheckResult");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthDeclaration_HealthProfile",
                table: "HealthDeclaration");

            migrationBuilder.DropForeignKey(
                name: "FK__HealthDec__Stude__656C112C",
                table: "HealthDeclaration");

            migrationBuilder.DropForeignKey(
                name: "FK__HealthPro__Healt__797309D9",
                table: "HealthProfile");

            migrationBuilder.DropForeignKey(
                name: "FK__HealthPro__Vacci__787EE5A0",
                table: "HealthProfile");

            migrationBuilder.DropForeignKey(
                name: "FK__Vaccinati__Stude__6C190EBB",
                table: "VaccinationResult");

            migrationBuilder.DropIndex(
                name: "IX_HealthProfile_HealthCheckResultID",
                table: "HealthProfile");

            migrationBuilder.DropIndex(
                name: "IX_HealthProfile_VaccinationResultID",
                table: "HealthProfile");

            migrationBuilder.RenameColumn(
                name: "StudentID",
                table: "VaccinationResult",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_VaccinationResult_StudentID",
                table: "VaccinationResult",
                newName: "IX_VaccinationResult_StudentId");

            migrationBuilder.RenameColumn(
                name: "StudentID",
                table: "HealthDeclaration",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_HealthDeclaration_StudentID",
                table: "HealthDeclaration",
                newName: "IX_HealthDeclaration_StudentId");

            migrationBuilder.RenameColumn(
                name: "StudentID",
                table: "HealthCheckResult",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_HealthCheckResult_StudentID",
                table: "HealthCheckResult",
                newName: "IX_HealthCheckResult_StudentId");

            migrationBuilder.AddColumn<Guid>(
                name: "HealthProfileID",
                table: "VaccinationResult",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "HealthProfileID",
                table: "HealthCheckResult",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationResult_HealthProfileID",
                table: "VaccinationResult",
                column: "HealthProfileID");

            migrationBuilder.CreateIndex(
                name: "IX_HealthCheckResult_HealthProfileID",
                table: "HealthCheckResult",
                column: "HealthProfileID");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthCheckResult_HealthProfile_HealthProfileID",
                table: "HealthCheckResult",
                column: "HealthProfileID",
                principalTable: "HealthProfile",
                principalColumn: "HealthProfileID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthCheckResult_Student_StudentId",
                table: "HealthCheckResult",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthDeclaration_HealthProfile",
                table: "HealthDeclaration",
                column: "HealthProfileID",
                principalTable: "HealthProfile",
                principalColumn: "HealthProfileID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthDeclaration_Student_StudentId",
                table: "HealthDeclaration",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_VaccinationResult_HealthProfile_HealthProfileID",
                table: "VaccinationResult",
                column: "HealthProfileID",
                principalTable: "HealthProfile",
                principalColumn: "HealthProfileID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VaccinationResult_Student_StudentId",
                table: "VaccinationResult",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "StudentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthCheckResult_HealthProfile_HealthProfileID",
                table: "HealthCheckResult");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthCheckResult_Student_StudentId",
                table: "HealthCheckResult");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthDeclaration_HealthProfile",
                table: "HealthDeclaration");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthDeclaration_Student_StudentId",
                table: "HealthDeclaration");

            migrationBuilder.DropForeignKey(
                name: "FK_VaccinationResult_HealthProfile_HealthProfileID",
                table: "VaccinationResult");

            migrationBuilder.DropForeignKey(
                name: "FK_VaccinationResult_Student_StudentId",
                table: "VaccinationResult");

            migrationBuilder.DropIndex(
                name: "IX_VaccinationResult_HealthProfileID",
                table: "VaccinationResult");

            migrationBuilder.DropIndex(
                name: "IX_HealthCheckResult_HealthProfileID",
                table: "HealthCheckResult");

            migrationBuilder.DropColumn(
                name: "HealthProfileID",
                table: "VaccinationResult");

            migrationBuilder.DropColumn(
                name: "HealthProfileID",
                table: "HealthCheckResult");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "VaccinationResult",
                newName: "StudentID");

            migrationBuilder.RenameIndex(
                name: "IX_VaccinationResult_StudentId",
                table: "VaccinationResult",
                newName: "IX_VaccinationResult_StudentID");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "HealthDeclaration",
                newName: "StudentID");

            migrationBuilder.RenameIndex(
                name: "IX_HealthDeclaration_StudentId",
                table: "HealthDeclaration",
                newName: "IX_HealthDeclaration_StudentID");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "HealthCheckResult",
                newName: "StudentID");

            migrationBuilder.RenameIndex(
                name: "IX_HealthCheckResult_StudentId",
                table: "HealthCheckResult",
                newName: "IX_HealthCheckResult_StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_HealthProfile_HealthCheckResultID",
                table: "HealthProfile",
                column: "HealthCheckResultID");

            migrationBuilder.CreateIndex(
                name: "IX_HealthProfile_VaccinationResultID",
                table: "HealthProfile",
                column: "VaccinationResultID");

            migrationBuilder.AddForeignKey(
                name: "FK__HealthChe__Stude__73BA3083",
                table: "HealthCheckResult",
                column: "StudentID",
                principalTable: "Student",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthDeclaration_HealthProfile",
                table: "HealthDeclaration",
                column: "HealthProfileID",
                principalTable: "HealthProfile",
                principalColumn: "HealthProfileID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__HealthDec__Stude__656C112C",
                table: "HealthDeclaration",
                column: "StudentID",
                principalTable: "Student",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK__HealthPro__Healt__797309D9",
                table: "HealthProfile",
                column: "HealthCheckResultID",
                principalTable: "HealthCheckResult",
                principalColumn: "ResultID");

            migrationBuilder.AddForeignKey(
                name: "FK__HealthPro__Vacci__787EE5A0",
                table: "HealthProfile",
                column: "VaccinationResultID",
                principalTable: "VaccinationResult",
                principalColumn: "VaccinationResultID");

            migrationBuilder.AddForeignKey(
                name: "FK__Vaccinati__Stude__6C190EBB",
                table: "VaccinationResult",
                column: "StudentID",
                principalTable: "Student",
                principalColumn: "StudentID");
        }
    }
}
