using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolMedicalServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNotificationWithRemoveStudentIdV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Notificat__Stude__7D439ABD",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_StudentId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Notification");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StudentId",
                table: "Notification",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notification_StudentId",
                table: "Notification",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Student_StudentId",
                table: "Notification",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "StudentID");
        }
    }
}
