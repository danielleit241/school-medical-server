using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolMedicalServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MedicalInventory",
                columns: table => new
                {
                    ItemID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CurrentQuantity = table.Column<int>(type: "int", nullable: true),
                    UnitOfMeasure = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ExpirationDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MedicalI__727E83EBABA65EC3", x => x.ItemID);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Role__8AFACE3A0F6D7D18", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "VaccineDetails",
                columns: table => new
                {
                    VaccineID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VaccineName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Disease = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VaccineType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AgeRecommendation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BatchNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExpirationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    DoseNumber = table.Column<int>(type: "int", nullable: true),
                    ContraindicationNotes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__VaccineD__45DC68E9F459710F", x => x.VaccineID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleID = table.Column<int>(type: "int", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    EmailAddress = table.Column<string>(type: "varchar(70)", unicode: false, maxLength: 70, nullable: true),
                    AvatarUrl = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    DayOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    RefreshToken = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__User__1788CCAC7059EAEE", x => x.UserID);
                    table.ForeignKey(
                        name: "FK__User__RoleID__4CA06362",
                        column: x => x.RoleID,
                        principalTable: "Role",
                        principalColumn: "RoleID");
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    StudentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StudentCode = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DayOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    Grade = table.Column<string>(type: "char(12)", unicode: false, fixedLength: true, maxLength: 12, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ParentPhoneNumber = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: true),
                    ParentEmailAddress = table.Column<string>(type: "varchar(70)", unicode: false, maxLength: 70, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Student__32C52A79AED3062C", x => x.StudentID);
                    table.ForeignKey(
                        name: "FK__Student__UserID__4F7CD00D",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Appointment",
                columns: table => new
                {
                    AppointmentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AppointmentDate = table.Column<DateOnly>(type: "date", nullable: true),
                    AppointmentTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    AppointmentReason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ConfirmationStatus = table.Column<bool>(type: "bit", nullable: true),
                    CompletionStatus = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Appointm__8ECDFCA27FD29BEA", x => x.AppointmentID);
                    table.ForeignKey(
                        name: "FK__Appointme__Stude__52593CB8",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID");
                    table.ForeignKey(
                        name: "FK__Appointme__UserI__534D60F1",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "HealthCheckSchedule",
                columns: table => new
                {
                    ScheduleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    TargetGrade = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    HealthCheckType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HealthCh__9C8A5B69BE2ABA03", x => x.ScheduleID);
                    table.ForeignKey(
                        name: "FK__HealthChe__Stude__6FE99F9F",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID");
                    table.ForeignKey(
                        name: "FK__HealthChe__UserI__70DDC3D8",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "HealthDeclaration",
                columns: table => new
                {
                    HealthDeclarationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeclarationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ChronicDiseases = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DrugAllergies = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FoodAllergies = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AdministeredVaccines = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HealthDe__327AAD7D8F9E8268", x => x.HealthDeclarationID);
                    table.ForeignKey(
                        name: "FK__HealthDec__Stude__656C112C",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID");
                });

            migrationBuilder.CreateTable(
                name: "MedicalEvent",
                columns: table => new
                {
                    EventID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EventDateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    EventType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    EventDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    RecordedID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SeverityLevel = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ParentNotified = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MedicalE__7944C8702997A560", x => x.EventID);
                    table.ForeignKey(
                        name: "FK__MedicalEv__Stude__59FA5E80",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID");
                    table.ForeignKey(
                        name: "FK__MedicalEv__UserI__5AEE82B9",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "MedicalRegistration",
                columns: table => new
                {
                    RegistrationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateSubmitted = table.Column<DateOnly>(type: "date", nullable: true),
                    MedicationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Dosage = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ParentalConsent = table.Column<bool>(type: "bit", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MedicalR__6EF58830C8922221", x => x.RegistrationID);
                    table.ForeignKey(
                        name: "FK__MedicalRe__Stude__5629CD9C",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID");
                    table.ForeignKey(
                        name: "FK__MedicalRe__UserI__571DF1D5",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    NotificationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VaccineScheduleID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HealthCheckScheduleID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EventID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SendDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Notifica__20CF2E32256F768A", x => x.NotificationID);
                    table.ForeignKey(
                        name: "FK__Notificat__Stude__7D439ABD",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID");
                    table.ForeignKey(
                        name: "FK__Notificat__UserI__7E37BEF6",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "VaccinationSchedule",
                columns: table => new
                {
                    ScheduleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VaccineID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Round = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    TargetGrade = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Vaccinat__9C8A5B694492A7A5", x => x.ScheduleID);
                    table.ForeignKey(
                        name: "FK__Vaccinati__Stude__68487DD7",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID");
                    table.ForeignKey(
                        name: "FK__Vaccinati__Vacci__693CA210",
                        column: x => x.VaccineID,
                        principalTable: "VaccineDetails",
                        principalColumn: "VaccineID");
                });

            migrationBuilder.CreateTable(
                name: "HealthCheckResult",
                columns: table => new
                {
                    ResultID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ScheduleID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DatePerformed = table.Column<DateOnly>(type: "date", nullable: true),
                    Height = table.Column<double>(type: "float", nullable: true),
                    Weight = table.Column<double>(type: "float", nullable: true),
                    VisionLeft = table.Column<double>(type: "float", nullable: true),
                    VisionRight = table.Column<double>(type: "float", nullable: true),
                    Hearing = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Nose = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BloodPressure = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RecordedID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HealthCh__976902283B65C17C", x => x.ResultID);
                    table.ForeignKey(
                        name: "FK__HealthChe__Sched__74AE54BC",
                        column: x => x.ScheduleID,
                        principalTable: "HealthCheckSchedule",
                        principalColumn: "ScheduleID");
                    table.ForeignKey(
                        name: "FK__HealthChe__Stude__73BA3083",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID");
                });

            migrationBuilder.CreateTable(
                name: "MedicalRequest",
                columns: table => new
                {
                    RequestItemID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ItemID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RequestedQuantity = table.Column<int>(type: "int", nullable: true),
                    Purpose = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RequestDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MedicalR__3F51AD77BE2FAFF0", x => x.RequestItemID);
                    table.ForeignKey(
                        name: "FK__MedicalRe__Event__5FB337D6",
                        column: x => x.EventID,
                        principalTable: "MedicalEvent",
                        principalColumn: "EventID");
                    table.ForeignKey(
                        name: "FK__MedicalRe__ItemI__60A75C0F",
                        column: x => x.ItemID,
                        principalTable: "MedicalInventory",
                        principalColumn: "ItemID");
                });

            migrationBuilder.CreateTable(
                name: "VaccinationResult",
                columns: table => new
                {
                    VaccinationResultID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ScheduleID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DoseNumber = table.Column<int>(type: "int", nullable: true),
                    VaccinationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    InjectionSite = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ImmediateReaction = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReactionStartTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    ReactionType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SeverityLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RecordedID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Vaccinat__12DE8FD91DB6B240", x => x.VaccinationResultID);
                    table.ForeignKey(
                        name: "FK__Vaccinati__Sched__6D0D32F4",
                        column: x => x.ScheduleID,
                        principalTable: "VaccinationSchedule",
                        principalColumn: "ScheduleID");
                    table.ForeignKey(
                        name: "FK__Vaccinati__Stude__6C190EBB",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID");
                });

            migrationBuilder.CreateTable(
                name: "HealthProfile",
                columns: table => new
                {
                    HealthProfileID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VaccinationResultID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HealthCheckResultID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    RecordedID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HealthPr__73C2C2B51D74B658", x => x.HealthProfileID);
                    table.ForeignKey(
                        name: "FK__HealthPro__Healt__797309D9",
                        column: x => x.HealthCheckResultID,
                        principalTable: "HealthCheckResult",
                        principalColumn: "ResultID");
                    table.ForeignKey(
                        name: "FK__HealthPro__Stude__778AC167",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID");
                    table.ForeignKey(
                        name: "FK__HealthPro__Vacci__787EE5A0",
                        column: x => x.VaccinationResultID,
                        principalTable: "VaccinationResult",
                        principalColumn: "VaccinationResultID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_StudentID",
                table: "Appointment",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_UserID",
                table: "Appointment",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_HealthCheckResult_ScheduleID",
                table: "HealthCheckResult",
                column: "ScheduleID");

            migrationBuilder.CreateIndex(
                name: "IX_HealthCheckResult_StudentID",
                table: "HealthCheckResult",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_HealthCheckSchedule_StudentID",
                table: "HealthCheckSchedule",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_HealthCheckSchedule_UserID",
                table: "HealthCheckSchedule",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_HealthDeclaration_StudentID",
                table: "HealthDeclaration",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_HealthProfile_HealthCheckResultID",
                table: "HealthProfile",
                column: "HealthCheckResultID");

            migrationBuilder.CreateIndex(
                name: "IX_HealthProfile_StudentID",
                table: "HealthProfile",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_HealthProfile_VaccinationResultID",
                table: "HealthProfile",
                column: "VaccinationResultID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalEvent_StudentID",
                table: "MedicalEvent",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalEvent_UserID",
                table: "MedicalEvent",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRegistration_StudentID",
                table: "MedicalRegistration",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRegistration_UserID",
                table: "MedicalRegistration",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRequest_EventID",
                table: "MedicalRequest",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRequest_ItemID",
                table: "MedicalRequest",
                column: "ItemID");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_StudentID",
                table: "Notification",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_UserID",
                table: "Notification",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Student_UserID",
                table: "Student",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleID",
                table: "User",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "UQ__User__85FB4E38CF69A5AF",
                table: "User",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationResult_ScheduleID",
                table: "VaccinationResult",
                column: "ScheduleID");

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationResult_StudentID",
                table: "VaccinationResult",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationSchedule_StudentID",
                table: "VaccinationSchedule",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationSchedule_VaccineID",
                table: "VaccinationSchedule",
                column: "VaccineID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointment");

            migrationBuilder.DropTable(
                name: "HealthDeclaration");

            migrationBuilder.DropTable(
                name: "HealthProfile");

            migrationBuilder.DropTable(
                name: "MedicalRegistration");

            migrationBuilder.DropTable(
                name: "MedicalRequest");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "HealthCheckResult");

            migrationBuilder.DropTable(
                name: "VaccinationResult");

            migrationBuilder.DropTable(
                name: "MedicalEvent");

            migrationBuilder.DropTable(
                name: "MedicalInventory");

            migrationBuilder.DropTable(
                name: "HealthCheckSchedule");

            migrationBuilder.DropTable(
                name: "VaccinationSchedule");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "VaccineDetails");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
