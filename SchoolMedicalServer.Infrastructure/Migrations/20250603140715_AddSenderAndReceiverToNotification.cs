using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolMedicalServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSenderAndReceiverToNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Notificat__UserI__7E37BEF6",
                table: "Notification");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Notification",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_UserID",
                table: "Notification",
                newName: "IX_Notification_UserId");

            migrationBuilder.AddColumn<Guid>(
                name: "ReceiverID",
                table: "Notification",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SenderID",
                table: "Notification",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_User_UserId",
                table: "Notification",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_User_UserId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "ReceiverID",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "SenderID",
                table: "Notification");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Notification",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_UserId",
                table: "Notification",
                newName: "IX_Notification_UserID");

            migrationBuilder.AddForeignKey(
                name: "FK__Notificat__UserI__7E37BEF6",
                table: "Notification",
                column: "UserID",
                principalTable: "User",
                principalColumn: "UserID");
        }
    }
}
