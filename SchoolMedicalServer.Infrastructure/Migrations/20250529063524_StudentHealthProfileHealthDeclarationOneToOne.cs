using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolMedicalServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StudentHealthProfileHealthDeclarationOneToOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__HealthPro__Stude__778AC167",
                table: "HealthProfile");

            migrationBuilder.DropIndex(
                name: "IX_HealthProfile_StudentID",
                table: "HealthProfile");

            migrationBuilder.AddColumn<Guid>(
                name: "HealthProfileID",
                table: "HealthDeclaration",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "HealthProfileId1",
                table: "HealthDeclaration",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_HealthProfile_StudentID",
                table: "HealthProfile",
                column: "StudentID",
                unique: true,
                filter: "[StudentID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_HealthDeclaration_HealthProfileID",
                table: "HealthDeclaration",
                column: "HealthProfileID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HealthDeclaration_HealthProfileId1",
                table: "HealthDeclaration",
                column: "HealthProfileId1");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthDeclaration_HealthProfile",
                table: "HealthDeclaration",
                column: "HealthProfileID",
                principalTable: "HealthProfile",
                principalColumn: "HealthProfileID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthDeclaration_HealthProfile_HealthProfileId1",
                table: "HealthDeclaration",
                column: "HealthProfileId1",
                principalTable: "HealthProfile",
                principalColumn: "HealthProfileID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthProfile_Student",
                table: "HealthProfile",
                column: "StudentID",
                principalTable: "Student",
                principalColumn: "StudentID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthDeclaration_HealthProfile",
                table: "HealthDeclaration");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthDeclaration_HealthProfile_HealthProfileId1",
                table: "HealthDeclaration");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthProfile_Student",
                table: "HealthProfile");

            migrationBuilder.DropIndex(
                name: "IX_HealthProfile_StudentID",
                table: "HealthProfile");

            migrationBuilder.DropIndex(
                name: "IX_HealthDeclaration_HealthProfileID",
                table: "HealthDeclaration");

            migrationBuilder.DropIndex(
                name: "IX_HealthDeclaration_HealthProfileId1",
                table: "HealthDeclaration");

            migrationBuilder.DropColumn(
                name: "HealthProfileID",
                table: "HealthDeclaration");

            migrationBuilder.DropColumn(
                name: "HealthProfileId1",
                table: "HealthDeclaration");

            migrationBuilder.CreateIndex(
                name: "IX_HealthProfile_StudentID",
                table: "HealthProfile",
                column: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK__HealthPro__Stude__778AC167",
                table: "HealthProfile",
                column: "StudentID",
                principalTable: "Student",
                principalColumn: "StudentID");
        }
    }
}
