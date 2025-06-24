using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolMedicalServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRoleIdIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Step 1: Drop the foreign key constraint first
            migrationBuilder.DropForeignKey(
                name: "FK__User__RoleID__4CA06362",
                table: "User");

            // Step 2: Create a temporary column
            migrationBuilder.AddColumn<int>(
                name: "RoleID_Temp",
                table: "Role",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Step 3: Copy existing data to the temporary column
            migrationBuilder.Sql("UPDATE Role SET RoleID_Temp = RoleID");

            // Step 4: Drop the primary key constraint
            migrationBuilder.DropPrimaryKey(
                name: "PK__Role__8AFACE3A0F6D7D18",
                table: "Role");

            // Step 5: Drop the original IDENTITY column
            migrationBuilder.DropColumn(
                name: "RoleID",
                table: "Role");

            // Step 6: Rename the temporary column to the original name
            migrationBuilder.RenameColumn(
                name: "RoleID_Temp",
                table: "Role",
                newName: "RoleID");

            // Step 7: Recreate the primary key
            migrationBuilder.AddPrimaryKey(
                name: "PK__Role__8AFACE3A0F6D7D18",
                table: "Role",
                column: "RoleID");

            // Step 8: Recreate the foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK__User__RoleID__4CA06362",
                table: "User",
                column: "RoleID",
                principalTable: "Role",
                principalColumn: "RoleID",
                onDelete: ReferentialAction.Restrict); // Adjust this based on your needs
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Step 1: Drop foreign key constraint
            migrationBuilder.DropForeignKey(
                name: "FK__User__RoleID__4CA06362",
                table: "User");

            // Step 2: Create temporary IDENTITY column
            migrationBuilder.AddColumn<int>(
                name: "RoleID_Temp",
                table: "Role",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            // Step 3: Copy data back (need to enable IDENTITY_INSERT)
            migrationBuilder.Sql("SET IDENTITY_INSERT Role ON");
            migrationBuilder.Sql("UPDATE Role SET RoleID_Temp = RoleID");
            migrationBuilder.Sql("SET IDENTITY_INSERT Role OFF");

            // Step 4: Drop primary key
            migrationBuilder.DropPrimaryKey(
                name: "PK__Role__8AFACE3A0F6D7D18",
                table: "Role");

            // Step 5: Drop non-identity column
            migrationBuilder.DropColumn(
                name: "RoleID",
                table: "Role");

            // Step 6: Rename temp column back
            migrationBuilder.RenameColumn(
                name: "RoleID_Temp",
                table: "Role",
                newName: "RoleID");

            // Step 7: Recreate primary key
            migrationBuilder.AddPrimaryKey(
                name: "PK__Role__8AFACE3A0F6D7D18",
                table: "Role",
                column: "RoleID");

            // Step 8: Recreate foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK__User__RoleID__4CA06362",
                table: "User",
                column: "RoleID",
                principalTable: "Role",
                principalColumn: "RoleID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}