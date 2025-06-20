using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolMedicalServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MergeHealthDeclarationAndAddUserAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VaccinationDeclaration_HealthDeclaration",
                table: "VaccinationDeclaration");

            migrationBuilder.DropTable(
                name: "HealthDeclaration");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "VaccinationDeclaration");

            migrationBuilder.DropColumn(
                name: "RecordedID",
                table: "HealthProfile");

            migrationBuilder.RenameColumn(
                name: "HealthDeclarationID",
                table: "VaccinationDeclaration",
                newName: "HealthProfileID");

            migrationBuilder.RenameIndex(
                name: "IX_VaccinationDeclaration_HealthDeclarationID",
                table: "VaccinationDeclaration",
                newName: "IX_VaccinationDeclaration_HealthProfileID");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "User",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChronicDiseases",
                table: "HealthProfile",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "DeclarationDate",
                table: "HealthProfile",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DrugAllergies",
                table: "HealthProfile",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FoodAllergies",
                table: "HealthProfile",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VaccinationDeclaration_HealthProfile",
                table: "VaccinationDeclaration",
                column: "HealthProfileID",
                principalTable: "HealthProfile",
                principalColumn: "HealthProfileID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VaccinationDeclaration_HealthProfile",
                table: "VaccinationDeclaration");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ChronicDiseases",
                table: "HealthProfile");

            migrationBuilder.DropColumn(
                name: "DeclarationDate",
                table: "HealthProfile");

            migrationBuilder.DropColumn(
                name: "DrugAllergies",
                table: "HealthProfile");

            migrationBuilder.DropColumn(
                name: "FoodAllergies",
                table: "HealthProfile");

            migrationBuilder.RenameColumn(
                name: "HealthProfileID",
                table: "VaccinationDeclaration",
                newName: "HealthDeclarationID");

            migrationBuilder.RenameIndex(
                name: "IX_VaccinationDeclaration_HealthProfileID",
                table: "VaccinationDeclaration",
                newName: "IX_VaccinationDeclaration_HealthDeclarationID");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "VaccinationDeclaration",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RecordedID",
                table: "HealthProfile",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HealthDeclaration",
                columns: table => new
                {
                    HealthDeclarationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HealthProfileID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChronicDiseases = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DeclarationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    DrugAllergies = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FoodAllergies = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HealthDe__327AAD7D8F9E8268", x => x.HealthDeclarationID);
                    table.ForeignKey(
                        name: "FK_HealthDeclaration_HealthProfile",
                        column: x => x.HealthProfileID,
                        principalTable: "HealthProfile",
                        principalColumn: "HealthProfileID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HealthDeclaration_Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "StudentID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_HealthDeclaration_HealthProfileID",
                table: "HealthDeclaration",
                column: "HealthProfileID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HealthDeclaration_StudentId",
                table: "HealthDeclaration",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_VaccinationDeclaration_HealthDeclaration",
                table: "VaccinationDeclaration",
                column: "HealthDeclarationID",
                principalTable: "HealthDeclaration",
                principalColumn: "HealthDeclarationID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
