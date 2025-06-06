using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolMedicalServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNotificationUserRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReceiverID",
                table: "Notification",
                newName: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_UserID",
                table: "Notification",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_User",
                table: "Notification",
                column: "UserID",
                principalTable: "User",
                principalColumn: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_User",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_UserID",
                table: "Notification");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Notification",
                newName: "ReceiverID");
        }
    }
}
