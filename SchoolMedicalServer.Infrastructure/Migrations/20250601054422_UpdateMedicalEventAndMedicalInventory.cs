using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolMedicalServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMedicalEventAndMedicalInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__MedicalEv__Stude__59FA5E80",
                table: "MedicalEvent");

            migrationBuilder.DropForeignKey(
                name: "FK__MedicalEv__UserI__5AEE82B9",
                table: "MedicalEvent");

            migrationBuilder.DropForeignKey(
                name: "FK__MedicalRe__Event__5FB337D6",
                table: "MedicalRequest");

            migrationBuilder.DropForeignKey(
                name: "FK__MedicalRe__ItemI__60A75C0F",
                table: "MedicalRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK__MedicalR__3F51AD77BE2FAFF0",
                table: "MedicalRequest");

            migrationBuilder.DropColumn(
                name: "CurrentQuantity",
                table: "MedicalInventory");

            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "MedicalInventory");

            migrationBuilder.DropColumn(
                name: "EventDateTime",
                table: "MedicalEvent");

            migrationBuilder.DropColumn(
                name: "RecordedID",
                table: "MedicalEvent");

            migrationBuilder.RenameColumn(
                name: "BatchNumber",
                table: "VaccinationDeclaration",
                newName: "DoseNumber");

            migrationBuilder.RenameColumn(
                name: "RequestedQuantity",
                table: "MedicalRequest",
                newName: "RequestQuantity");

            migrationBuilder.RenameColumn(
                name: "EventID",
                table: "MedicalRequest",
                newName: "MedicalEventID");

            migrationBuilder.RenameColumn(
                name: "RequestItemID",
                table: "MedicalRequest",
                newName: "RequestID");

            migrationBuilder.RenameIndex(
                name: "IX_MedicalRequest_EventID",
                table: "MedicalRequest",
                newName: "IX_MedicalRequest_MedicalEventID");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "MedicalEvent",
                newName: "StaffNurseID");

            migrationBuilder.RenameIndex(
                name: "IX_MedicalEvent_UserID",
                table: "MedicalEvent",
                newName: "IX_MedicalEvent_StaffNurseID");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "MedicalInventory",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastExportDate",
                table: "MedicalInventory",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastImportDate",
                table: "MedicalInventory",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaximumStockLevel",
                table: "MedicalInventory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinimumStockLevel",
                table: "MedicalInventory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuantityInStock",
                table: "MedicalInventory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "MedicalInventory",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "EventDate",
                table: "MedicalEvent",
                type: "date",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK__MedicalR__RequestId",
                table: "MedicalRequest",
                column: "RequestID");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalEvent_Student",
                table: "MedicalEvent",
                column: "StudentID",
                principalTable: "Student",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalEvent_User",
                table: "MedicalEvent",
                column: "StaffNurseID",
                principalTable: "User",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRequest_MedicalEvent",
                table: "MedicalRequest",
                column: "MedicalEventID",
                principalTable: "MedicalEvent",
                principalColumn: "EventID");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRequest_MedicalInventory",
                table: "MedicalRequest",
                column: "ItemID",
                principalTable: "MedicalInventory",
                principalColumn: "ItemID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalEvent_Student",
                table: "MedicalEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalEvent_User",
                table: "MedicalEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRequest_MedicalEvent",
                table: "MedicalRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRequest_MedicalInventory",
                table: "MedicalRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK__MedicalR__RequestId",
                table: "MedicalRequest");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "MedicalInventory");

            migrationBuilder.DropColumn(
                name: "LastExportDate",
                table: "MedicalInventory");

            migrationBuilder.DropColumn(
                name: "LastImportDate",
                table: "MedicalInventory");

            migrationBuilder.DropColumn(
                name: "MaximumStockLevel",
                table: "MedicalInventory");

            migrationBuilder.DropColumn(
                name: "MinimumStockLevel",
                table: "MedicalInventory");

            migrationBuilder.DropColumn(
                name: "QuantityInStock",
                table: "MedicalInventory");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "MedicalInventory");

            migrationBuilder.DropColumn(
                name: "EventDate",
                table: "MedicalEvent");

            migrationBuilder.RenameColumn(
                name: "DoseNumber",
                table: "VaccinationDeclaration",
                newName: "BatchNumber");

            migrationBuilder.RenameColumn(
                name: "RequestQuantity",
                table: "MedicalRequest",
                newName: "RequestedQuantity");

            migrationBuilder.RenameColumn(
                name: "MedicalEventID",
                table: "MedicalRequest",
                newName: "EventID");

            migrationBuilder.RenameColumn(
                name: "RequestID",
                table: "MedicalRequest",
                newName: "RequestItemID");

            migrationBuilder.RenameIndex(
                name: "IX_MedicalRequest_MedicalEventID",
                table: "MedicalRequest",
                newName: "IX_MedicalRequest_EventID");

            migrationBuilder.RenameColumn(
                name: "StaffNurseID",
                table: "MedicalEvent",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_MedicalEvent_StaffNurseID",
                table: "MedicalEvent",
                newName: "IX_MedicalEvent_UserID");

            migrationBuilder.AddColumn<int>(
                name: "CurrentQuantity",
                table: "MedicalInventory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "ExpirationDate",
                table: "MedicalInventory",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EventDateTime",
                table: "MedicalEvent",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RecordedID",
                table: "MedicalEvent",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK__MedicalR__3F51AD77BE2FAFF0",
                table: "MedicalRequest",
                column: "RequestItemID");

            migrationBuilder.AddForeignKey(
                name: "FK__MedicalEv__Stude__59FA5E80",
                table: "MedicalEvent",
                column: "StudentID",
                principalTable: "Student",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK__MedicalEv__UserI__5AEE82B9",
                table: "MedicalEvent",
                column: "UserID",
                principalTable: "User",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK__MedicalRe__Event__5FB337D6",
                table: "MedicalRequest",
                column: "EventID",
                principalTable: "MedicalEvent",
                principalColumn: "EventID");

            migrationBuilder.AddForeignKey(
                name: "FK__MedicalRe__ItemI__60A75C0F",
                table: "MedicalRequest",
                column: "ItemID",
                principalTable: "MedicalInventory",
                principalColumn: "ItemID");
        }
    }
}
