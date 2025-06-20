using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolMedicalServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMedicalRegistrationDetailsRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Dosage",
                table: "MedicalRegistration",
                newName: "TotalDosages");

            migrationBuilder.CreateTable(
                name: "MedicalRegistrationDetails",
                columns: table => new
                {
                    MedicalRegistrationDetailsID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DoseTime = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MedicalRegistrationDetails", x => x.MedicalRegistrationDetailsID);
                    table.ForeignKey(
                        name: "FK_MedicalRegistrationDetails_MedicalRegistration",
                        column: x => x.RegistrationID,
                        principalTable: "MedicalRegistration",
                        principalColumn: "RegistrationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRegistrationDetails_RegistrationID",
                table: "MedicalRegistrationDetails",
                column: "RegistrationID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicalRegistrationDetails");

            migrationBuilder.RenameColumn(
                name: "TotalDosages",
                table: "MedicalRegistration",
                newName: "Dosage");
        }
    }
}
