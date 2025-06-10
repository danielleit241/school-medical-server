using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolMedicalServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAttributesDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VaccinationSchedule_VaccineDetails_VaccineID",
                table: "VaccinationSchedule");

            migrationBuilder.DropTable(
                name: "VaccineDetails");

            migrationBuilder.DropColumn(
                name: "IsConfirmed",
                table: "Notification");

            migrationBuilder.RenameColumn(
                name: "ConfirmedAt",
                table: "Notification",
                newName: "ReadDate");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OtpExpiryTime",
                table: "User",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "User",
                type: "datetime",
                nullable: true,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateAt",
                table: "User",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "Student",
                type: "datetime",
                nullable: true,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateAt",
                table: "Student",
                type: "datetime",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Notification",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Notification",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RecordedAt",
                table: "HealthCheckResult",
                type: "datetime",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "StaffNurseId",
                table: "Appointment",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletionAt",
                table: "Appointment",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ConfirmationAt",
                table: "Appointment",
                type: "datetime",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VaccinationDetails",
                columns: table => new
                {
                    VaccineID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VaccineCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VaccineName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VaccineType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AgeRecommendation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BatchNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExpirationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ContraindicationNotes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__VaccinationDetail__VaccineId", x => x.VaccineID);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_VaccinationSchedule_VaccinationDetails_VaccineID",
                table: "VaccinationSchedule",
                column: "VaccineID",
                principalTable: "VaccinationDetails",
                principalColumn: "VaccineID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VaccinationSchedule_VaccinationDetails_VaccineID",
                table: "VaccinationSchedule");

            migrationBuilder.DropTable(
                name: "VaccinationDetails");

            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UpdateAt",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "UpdateAt",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "RecordedAt",
                table: "HealthCheckResult");

            migrationBuilder.DropColumn(
                name: "CompletionAt",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "ConfirmationAt",
                table: "Appointment");

            migrationBuilder.RenameColumn(
                name: "ReadDate",
                table: "Notification",
                newName: "ConfirmedAt");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OtpExpiryTime",
                table: "User",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Notification",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Notification",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmed",
                table: "Notification",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<Guid>(
                name: "StaffNurseId",
                table: "Appointment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "VaccineDetails",
                columns: table => new
                {
                    VaccineID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AgeRecommendation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BatchNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContraindicationNotes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DoseNumber = table.Column<int>(type: "int", nullable: true),
                    ExpirationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VaccineCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VaccineName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VaccineType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__VaccineD__45DC68E9F459710F", x => x.VaccineID);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_VaccinationSchedule_VaccineDetails_VaccineID",
                table: "VaccinationSchedule",
                column: "VaccineID",
                principalTable: "VaccineDetails",
                principalColumn: "VaccineID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
