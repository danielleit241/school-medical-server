using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Entities;

namespace SchoolMedicalServer.Infrastructure;

public partial class SchoolMedicalManagementContext : DbContext
{
    public SchoolMedicalManagementContext()
    {
    }

    public SchoolMedicalManagementContext(DbContextOptions<SchoolMedicalManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<HealthCheckResult> HealthCheckResults { get; set; }

    public virtual DbSet<HealthCheckSchedule> HealthCheckSchedules { get; set; }

    public virtual DbSet<HealthProfile> HealthProfiles { get; set; }

    public virtual DbSet<MedicalEvent> MedicalEvents { get; set; }

    public virtual DbSet<MedicalInventory> MedicalInventories { get; set; }

    public virtual DbSet<MedicalRegistration> MedicalRegistrations { get; set; }

    public virtual DbSet<MedicalRequest> MedicalRequests { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VaccinationResult> VaccinationResults { get; set; }

    public virtual DbSet<VaccinationSchedule> VaccinationSchedules { get; set; }

    public virtual DbSet<VaccinationDetail> VaccinationDetails { get; set; }

    public virtual DbSet<VaccinationDeclaration> VaccinationDeclarations { get; set; }

    public virtual DbSet<MedicalRegistrationDetails> MedicalRegistrationDetails { get; set; }

    public virtual DbSet<VaccinationObservation> VaccinationObservations { get; set; }

    public virtual DbSet<VaccinationRound> VaccinationRounds { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCA27FD29BEA");

            entity.ToTable("Appointment");

            entity.Property(e => e.AppointmentId)
                .ValueGeneratedNever()
                .HasColumnName("AppointmentID");
            entity.Property(e => e.AppointmentReason)
                .HasMaxLength(255);
            entity.Property(e => e.StudentId)
                .HasColumnName("StudentID");
            entity.Property(e => e.UserId)
                .HasColumnName("UserID");
            entity.Property(e => e.StaffNurseId)
                .HasColumnName("StaffNurseId");
            entity.Property(e => e.Topic)
                .HasMaxLength(40);
            entity.Property(e => e.AppointmentDate)
                .HasColumnName("AppointmentDate")
                .HasColumnType("date");
            entity.Property(e => e.AppointmentStartTime)
                .HasColumnType("time")
                .HasColumnName("AppointmentStartTime");
            entity.Property(e => e.AppointmentEndTime)
                .HasColumnType("time")
                .HasColumnName("AppointmentEndTime");

            entity.Property(e => e.ConfirmationStatus)
                .HasColumnType("bit");
            entity.Property(e => e.ConfirmationAt)
                .HasColumnType("datetime");
            entity.Property(e => e.CompletionStatus)
                .HasColumnType("bit");
            entity.Property(e => e.CompletionAt)
                .HasColumnType("datetime");

            entity.HasOne(d => d.Student)
                .WithMany(p => p.Appointments)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__Appointme__Stude__52593CB8");

            entity.HasOne(d => d.User)
                .WithMany(p => p.Appointments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Appointme__UserI__534D60F1");
        });

        modelBuilder.Entity<HealthCheckResult>(entity =>
        {
            entity.HasKey(e => e.ResultId).HasName("PK__HealthCh__976902283B65C17C");

            entity.ToTable("HealthCheckResult");

            entity.Property(e => e.ResultId)
                .ValueGeneratedNever()
                .HasColumnName("ResultID");
            entity.Property(e => e.ScheduleId)
                .HasColumnName("ScheduleID");
            entity.Property(e => e.HealthProfileId)
                .HasColumnName("HealthProfileID");

            entity.Property(e => e.DatePerformed)
                .HasColumnName("DatePerformed")
                .HasColumnType("date");
            entity.Property(e => e.Height)
                .HasColumnType("float");
            entity.Property(e => e.Weight)
                .HasColumnType("float");
            entity.Property(e => e.VisionLeft)
                .HasColumnType("float");
            entity.Property(e => e.VisionRight)
                .HasColumnType("float");
            entity.Property(e => e.BloodPressure)
                .HasMaxLength(50);
            entity.Property(e => e.Hearing)
                .HasMaxLength(50);
            entity.Property(e => e.Nose)
                .HasMaxLength(50);
            entity.Property(e => e.Notes)
                .HasMaxLength(255);
            entity.Property(e => e.RecordedId)
                .HasColumnName("RecordedID");
            entity.Property(e => e.RecordedAt)
                .HasColumnType("datetime");

            entity.HasOne(d => d.Schedule)
                .WithMany(p => p.HealthCheckResults)
                .HasForeignKey(d => d.ScheduleId)
                .HasConstraintName("FK__HealthChe__Sched__74AE54BC");

            entity.HasOne(hcr => hcr.HealthProfile)
                .WithMany(hp => hp.HealthCheckResults)
                .HasForeignKey(hcr => hcr.HealthProfileId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<HealthCheckSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__HealthCh__9C8A5B69BE2ABA03");

            entity.ToTable("HealthCheckSchedule");

            entity.Property(e => e.ScheduleId)
                .ValueGeneratedNever()
                .HasColumnName("ScheduleID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.HealthCheckType).HasMaxLength(30);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.TargetGrade).HasMaxLength(12);
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.Student)
                .WithMany(p => p.HealthCheckSchedules)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__HealthChe__Stude__6FE99F9F");

            entity.HasMany(s => s.HealthCheckResults)
                .WithOne(r => r.Schedule)
                .HasForeignKey(r => r.ScheduleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<HealthProfile>(entity =>
        {
            entity.HasKey(e => e.HealthProfileId).HasName("PK__HealthPr__73C2C2B51D74B658");

            entity.ToTable("HealthProfile");

            entity.Property(e => e.HealthProfileId)
                .ValueGeneratedNever()
                .HasColumnName("HealthProfileID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Notes).HasMaxLength(255);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.Property(e => e.DeclarationDate).HasColumnType("date");
            entity.Property(e => e.ChronicDiseases).HasMaxLength(255);
            entity.Property(e => e.DrugAllergies).HasMaxLength(255);
            entity.Property(e => e.FoodAllergies).HasMaxLength(255);

            entity.HasOne(d => d.Student)
                .WithOne(s => s.HealthProfile)
                .HasForeignKey<HealthProfile>(hp => hp.StudentId)
                .HasConstraintName("FK_HealthProfile_Student")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(hp => hp.VaccinationResults)
                .WithOne(vr => vr.HealthProfile)
                .HasForeignKey(vr => vr.HealthProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(hp => hp.HealthCheckResults)
                .WithOne(hcr => hcr.HealthProfile)
                .HasForeignKey(hcr => hcr.HealthProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(hp => hp.VaccinationDeclarations)
                .WithOne(vd => vd.HealthProfile)
                .HasForeignKey(vd => vd.HealthProfileId)
                .HasConstraintName("FK_VaccinationDeclaration_HealthProfile")
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<MedicalEvent>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__MedicalE__7944C8702997A560");
            entity.ToTable("MedicalEvent");

            entity.Property(e => e.EventId)
                .ValueGeneratedNever()
                .HasColumnName("EventID");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.StaffNurseId).HasColumnName("StaffNurseID");
            entity.Property(e => e.EventDate).HasColumnName("EventDate").HasColumnType("date");
            entity.Property(e => e.EventType).HasMaxLength(30);
            entity.Property(e => e.EventDescription).HasMaxLength(255);
            entity.Property(e => e.Location).HasMaxLength(60);
            entity.Property(e => e.SeverityLevel).HasMaxLength(30);
            entity.Property(e => e.ParentNotified).HasColumnName("ParentNotified");
            entity.Property(e => e.Notes).HasMaxLength(255);

            entity.HasOne(d => d.Student)
                .WithMany(p => p.MedicalEvents)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_MedicalEvent_Student");

            entity.HasOne(d => d.User)
                .WithMany(p => p.MedicalEvents)
                .HasForeignKey(d => d.StaffNurseId)
                .HasConstraintName("FK_MedicalEvent_User");
        });

        modelBuilder.Entity<MedicalInventory>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__MedicalI__727E83EBABA65EC3");

            entity.ToTable("MedicalInventory");

            entity.Property(e => e.ItemId)
                .ValueGeneratedNever()
                .HasColumnName("ItemID");
            entity.Property(e => e.ItemName)
                .HasMaxLength(100)
                .HasColumnName("ItemName");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .HasColumnName("Category");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("Description");
            entity.Property(e => e.QuantityInStock)
                .HasColumnName("QuantityInStock");
            entity.Property(e => e.UnitOfMeasure)
                .HasMaxLength(20)
                .HasColumnName("UnitOfMeasure");
            entity.Property(e => e.MinimumStockLevel)
                .HasColumnName("MinimumStockLevel");
            entity.Property(e => e.MaximumStockLevel)
                .HasColumnName("MaximumStockLevel");
            entity.Property(e => e.LastImportDate)
                .HasColumnType("datetime")
                .HasColumnName("LastImportDate");
            entity.Property(e => e.LastExportDate)
                .HasColumnType("datetime")
                .HasColumnName("LastExportDate");
            entity.Property(e => e.ExpiryDate)
                .HasColumnType("datetime")
                .HasColumnName("ExpiryDate");
            entity.Property(e => e.Status)
                .HasColumnType("bit")
                .HasDefaultValue(true)
                .HasColumnName("Status");
        });

        modelBuilder.Entity<MedicalRegistration>(entity =>
        {
            entity.HasKey(e => e.RegistrationId).HasName("PK__MedicalR__6EF58830C8922221");

            entity.ToTable("MedicalRegistration");

            entity.Property(e => e.RegistrationId)
                .ValueGeneratedNever()
                .HasColumnName("RegistrationID");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.MedicationName).HasMaxLength(100);
            entity.Property(e => e.TotalDosages).HasMaxLength(30);
            entity.Property(e => e.Notes).HasMaxLength(255);
            entity.Property(e => e.ParentalConsent);
            entity.Property(e => e.StaffNurseId).HasColumnName("StaffNurseID");
            entity.Property(e => e.DateApproved).HasColumnName("DateApproved");
            entity.Property(e => e.Status)
                .HasColumnType("bit")
                .HasDefaultValue(false)
                .HasColumnName("Status");
            entity.HasOne(d => d.Student)
                .WithMany(p => p.MedicalRegistrations)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__MedicalRe__Stude__5629CD9C");

            entity.HasOne(d => d.User)
                .WithMany(p => p.MedicalRegistrations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__MedicalRe__UserI__571DF1D5");

            entity.HasMany(mr => mr.Details)
                .WithOne(d => d.MedicalRegistration)
                .HasForeignKey(d => d.RegistrationId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<MedicalRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__MedicalR__RequestId");
            entity.ToTable("MedicalRequest");

            entity.Property(e => e.RequestId)
                .ValueGeneratedNever()
                .HasColumnName("RequestID");
            entity.Property(e => e.MedicalEventId).HasColumnName("MedicalEventID");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.RequestQuantity).HasColumnName("RequestQuantity");
            entity.Property(e => e.Purpose).HasMaxLength(255);
            entity.Property(e => e.RequestDate).HasColumnName("RequestDate").HasColumnType("date");


            entity.HasOne(d => d.Event)
                .WithMany(p => p.MedicalRequests)
                .HasForeignKey(d => d.MedicalEventId)
                .HasConstraintName("FK_MedicalRequest_MedicalEvent");

            entity.HasOne(d => d.Item)
                .WithMany(p => p.MedicalRequests)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK_MedicalRequest_MedicalInventory");
        });

        modelBuilder.Entity<MedicalRegistrationDetails>(entity =>
        {
            entity.HasKey(e => e.MedicalRegistrationDetailsId).HasName("PK__MedicalRegistrationDetails");
            entity.ToTable("MedicalRegistrationDetails");

            entity.Property(e => e.MedicalRegistrationDetailsId)
                .ValueGeneratedNever()
                .HasColumnName("MedicalRegistrationDetailsID");

            entity.Property(e => e.StaffNurseId).HasColumnName("StaffNurseID");
            entity.Property(e => e.RegistrationId).HasColumnName("RegistrationID");
            entity.Property(e => e.DoseTime).HasMaxLength(30);
            entity.Property(e => e.Notes).HasMaxLength(255);
            entity.Property(e => e.IsCompleted);
            entity.Property(e => e.DoseNumber)
            .HasColumnName("DoseNumber")
            .IsRequired();
            entity.Property(e => e.DateCompleted)
            .HasColumnType("datetime")
            .HasColumnName("DateCompleted");


            entity.HasOne(d => d.MedicalRegistration)
                .WithMany(p => p.Details)
                .HasForeignKey(d => d.RegistrationId)
                .HasConstraintName("FK_MedicalRegistrationDetails_MedicalRegistration");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E32256F768A");

            entity.ToTable("Notification");

            entity.Property(e => e.NotificationId)
                .ValueGeneratedNever()
                .HasColumnName("NotificationID");

            entity.Property(e => e.UserId)
                .HasColumnName("UserID");

            entity.Property(e => e.SenderId)
                .HasColumnName("SenderID");

            entity.Property(e => e.Type)
                .HasColumnName("Type")
                .HasConversion<int>()
                .IsRequired();

            entity.Property(e => e.SourceId)
                .HasColumnName("SourceID");

            entity.Property(e => e.Title)
                .HasMaxLength(255);

            entity.Property(e => e.Content)
                .HasMaxLength(1000);

            entity.Property(e => e.SendDate)
                .HasColumnType("datetime");

            entity.Property(e => e.IsRead)
                .HasColumnType("bit")
                .HasDefaultValue(false)
                .HasColumnName("IsRead");

            entity.Property(e => e.ReadDate)
                .HasColumnType("datetime")
                .HasColumnName("ReadDate");

            entity.HasOne(d => d.User)
                .WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Notification_User");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE3A0F6D7D18");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Student__32C52A79AED3062C");

            entity.ToTable("Student");

            entity.Property(e => e.StudentId)
                .ValueGeneratedNever()
                .HasColumnName("StudentID");
            entity.Property(e => e.StudentCode)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.HasIndex(e => e.StudentCode)
                .IsUnique()
                .HasDatabaseName("IX_Student_StudentCode_Unique");

            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.DayOfBirth)
                .HasColumnName("DayOfBirth")
                .HasColumnType("date");
            entity.Property(e => e.Gender)
                .HasMaxLength(3);
            entity.Property(e => e.Grade)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Address)
                .HasMaxLength(255);
            entity.Property(e => e.ParentPhoneNumber)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.ParentEmailAddress)
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.UserId)
                .HasColumnName("UserID");

            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.UpdateAt)
                .HasColumnType("datetime");

            entity.HasOne(d => d.User)
                .WithMany(p => p.Students)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Student__UserID__4F7CD00D");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCAC7059EAEE");

            entity.ToTable("User");

            entity.HasIndex(e => e.PhoneNumber).IsUnique();

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("UserID");
            entity.Property(e => e.AvatarUrl)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.EmailAddress)
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.RefreshToken)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasColumnType("bit")
                .HasDefaultValue(true);
            entity.Property(e => e.RefreshTokenExpiryTime)
                .HasColumnType("datetime");
            entity.Property(e => e.RoleId)
                .HasColumnName("RoleID");
            entity.Property(e => e.Address)
                .HasMaxLength(255);
            entity.Property(e => e.Otp)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.OtpExpiryTime)
                .HasColumnType("datetime");

            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.UpdateAt)
                .HasColumnType("datetime");

            entity.Property(e => e.DayOfBirth)
                .HasColumnType("date");

            entity.HasOne(d => d.Role)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__User__RoleID__4CA06362");
        });

        modelBuilder.Entity<VaccinationResult>(entity =>
        {
            entity.HasKey(e => e.VaccinationResultId).HasName("PK__Vaccinat__12DE8FD91DB6B240");
            entity.ToTable("VaccinationResult");
            entity.Property(e => e.VaccinationResultId)
                .ValueGeneratedNever()
                .HasColumnName("VaccinationResultID");
            entity.Property(e => e.Notes).HasMaxLength(255);
            entity.Property(e => e.RecorderId).HasColumnName("RecorderID");
            entity.Property(e => e.HealthProfileId).HasColumnName("HealthProfileID");
            entity.Property(e => e.RoundId).HasColumnName("RoundID");
            entity.Property(e => e.ParentConfirmed).HasColumnName("ParentConfirmed").HasDefaultValue(null);
            entity.Property(e => e.Vaccinated).HasColumnName("Vaccinated");
            entity.Property(e => e.VaccinatedDate).HasColumnName("VaccinatedDate").HasColumnType("date");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.InjectionSite).HasMaxLength(50);
            entity.Property(e => e.VaccinatedTime);
            entity.HasOne(vr => vr.HealthProfile)
                .WithMany(hp => hp.VaccinationResults)
                .HasForeignKey(vr => vr.HealthProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(vr => vr.Round)
                .WithMany(r => r.VaccinationResults)
                .HasForeignKey(vr => vr.RoundId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(vr => vr.VaccinationObservation)
                .WithOne(obs => obs.VaccinationResult)
                .HasForeignKey<VaccinationObservation>(obs => obs.VaccinationResultId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<VaccinationObservation>(entity =>
        {
            entity.HasKey(e => e.VaccinationObservationId);
            entity.ToTable("VaccinationObservation");

            entity.Property(e => e.VaccinationObservationId)
                .ValueGeneratedNever()
                .HasColumnName("VaccinationObservationID");

            entity.Property(e => e.VaccinationResultId)
                .HasColumnName("VaccinationResultID");

            entity.Property(e => e.ObservationStartTime)
                .HasColumnType("datetime");

            entity.Property(e => e.ObservationEndTime)
                .HasColumnType("datetime");

            entity.Property(e => e.ReactionStartTime)
                .HasColumnType("datetime");

            entity.Property(e => e.ReactionType)
                .HasMaxLength(100);

            entity.Property(e => e.SeverityLevel)
                .HasMaxLength(50);

            entity.Property(e => e.ImmediateReaction)
                .HasMaxLength(100);

            entity.Property(e => e.Intervention)
                .HasMaxLength(255);

            entity.Property(e => e.ObservedBy)
                .HasMaxLength(100);

            entity.Property(e => e.Notes)
                .HasMaxLength(255);

            entity.HasOne(e => e.VaccinationResult)
                .WithOne(r => r.VaccinationObservation)
                .HasForeignKey<VaccinationObservation>(e => e.VaccinationResultId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_VaccinationObservation_VaccinationResult");
        });


        modelBuilder.Entity<VaccinationSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__Vaccinat__9C8A5B694492A7A5");
            entity.ToTable("VaccinationSchedule");
            entity.Property(e => e.ScheduleId)
                .ValueGeneratedNever()
                .HasColumnName("ScheduleID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.VaccineId).HasColumnName("VaccineID");
            entity.Property(e => e.ParentNotificationStartDate).HasColumnType("date");
            entity.Property(e => e.ParentNotificationEndDate).HasColumnType("date");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.Status).HasColumnType("bit");

            entity.HasOne(sch => sch.Vaccine)
                .WithMany(vd => vd.VaccinationSchedules)
                .HasForeignKey(sch => sch.VaccineId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(sch => sch.Rounds)
                .WithOne(round => round.Schedule)
                .HasForeignKey(round => round.ScheduleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<VaccinationDetail>(entity =>
        {
            entity.HasKey(e => e.VaccineId).HasName("PK__VaccinationDetail__VaccineId");

            entity.ToTable("VaccinationDetails");

            entity.Property(e => e.VaccineId)
                .ValueGeneratedNever()
                .HasColumnName("VaccineID");
            entity.Property(e => e.VaccineCode)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.VaccineName)
                .HasMaxLength(100);
            entity.Property(e => e.Manufacturer)
                .HasMaxLength(100);
            entity.Property(e => e.VaccineType)
                .HasMaxLength(50);
            entity.Property(e => e.AgeRecommendation)
                .HasMaxLength(50);
            entity.Property(e => e.BatchNumber)
                .HasMaxLength(50);
            entity.Property(e => e.ExpirationDate)
                .HasColumnType("date");
            entity.Property(e => e.ContraindicationNotes)
                .HasMaxLength(255);
            entity.Property(e => e.Description)
                .HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasColumnType("bit")
                .HasDefaultValue(true)
                .HasColumnName("Status");
        });

        modelBuilder.Entity<Role>().HasData(
            new Role { RoleId = 1, RoleName = "admin" },
            new Role { RoleId = 2, RoleName = "nurse" },
            new Role { RoleId = 3, RoleName = "manager" },
            new Role { RoleId = 4, RoleName = "parent" }
        );

        modelBuilder.Entity<VaccinationDeclaration>(entity =>
        {
            entity.HasKey(e => e.VaccinationDeclarationId).HasName("PK__VaccinationDeclaration");

            entity.ToTable("VaccinationDeclaration");

            entity.Property(e => e.VaccinationDeclarationId)
                .ValueGeneratedNever()
                .HasColumnName("VaccinationDeclarationID");
            entity.Property(e => e.HealthProfileId).HasColumnName("HealthProfileID");
            entity.Property(e => e.VaccineName).HasMaxLength(100);
            entity.Property(e => e.DoseNumber).HasMaxLength(50);
            entity.Property(e => e.VaccinatedDate).HasColumnType("date");

            entity.HasOne(d => d.HealthProfile)
                .WithMany(p => p.VaccinationDeclarations)
                .HasForeignKey(d => d.HealthProfileId)
                .HasConstraintName("FK_VaccinationDeclaration_HealthProfile");
        });

        modelBuilder.Entity<VaccinationRound>(entity =>
        {
            entity.HasKey(e => e.RoundId);
            entity.ToTable("VaccinationRound");
            entity.Property(e => e.RoundId)
                .ValueGeneratedNever()
                .HasColumnName("RoundID");
            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.RoundName).HasMaxLength(100);
            entity.Property(e => e.TargetGrade).HasMaxLength(12);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Status).HasColumnName("Status");
            entity.Property(e => e.StartTime);
            entity.Property(e => e.EndTime);
            entity.HasOne(e => e.Schedule)
                .WithMany(s => s.Rounds)
                .HasForeignKey(e => e.ScheduleId)
                .HasConstraintName("FK_VaccinationRound_VaccinationSchedule")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(r => r.VaccinationResults)
                .WithOne(vr => vr.Round)
                .HasForeignKey(vr => vr.RoundId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
