using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolMedicalServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNotificationWithSourceIdAndConfirmed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventID",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "HealthCheckScheduleID",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "VaccineScheduleID",
                table: "Notification");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SendDate",
                table: "Notification",
                type: "datetime",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ConfirmedAt",
                table: "Notification",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Notification",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmed",
                table: "Notification",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "Notification",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "SourceID",
                table: "Notification",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Notification",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Notification",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmedAt",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "IsConfirmed",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "SourceID",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Notification");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SendDate",
                table: "Notification",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AddColumn<Guid>(
                name: "EventID",
                table: "Notification",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "HealthCheckScheduleID",
                table: "Notification",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Notification",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "VaccineScheduleID",
                table: "Notification",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
