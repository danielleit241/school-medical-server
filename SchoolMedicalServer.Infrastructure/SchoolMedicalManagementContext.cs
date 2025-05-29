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

    public virtual DbSet<VaccineDetail> VaccineDetails { get; set; }

    public virtual DbSet<VaccinationDeclaration> VaccinationDeclarations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCA27FD29BEA");

            entity.ToTable("Appointment");

            entity.Property(e => e.AppointmentId)
                .ValueGeneratedNever()
                .HasColumnName("AppointmentID");
            entity.Property(e => e.AppointmentReason).HasMaxLength(255);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            // Add this line for StaffNurseId
            entity.Property(e => e.StaffNurseId)
                  .HasColumnName("StaffNurseId")
                  .IsRequired();
            //Adding AppointmentTopic
            entity.Property(e => e.Topic).HasMaxLength(40);
            //Adding startime, and endtime
            entity.Property(e => e.AppointmentStartTime)
                .HasColumnType("time")
                .HasColumnName("AppointmentStartTime");

            entity.Property(e => e.AppointmentEndTime)
                  .HasColumnType("time")
                  .HasColumnName("AppointmentEndTime");

            entity.HasOne(d => d.Student).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__Appointme__Stude__52593CB8");

            entity.HasOne(d => d.User).WithMany(p => p.Appointments)
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
            entity.Property(e => e.BloodPressure).HasMaxLength(50);
            entity.Property(e => e.Hearing).HasMaxLength(50);
            entity.Property(e => e.Nose).HasMaxLength(50);
            entity.Property(e => e.Notes).HasMaxLength(255);
            entity.Property(e => e.RecordedId).HasColumnName("RecordedID");
            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");

            entity.HasOne(d => d.Schedule).WithMany(p => p.HealthCheckResults)
                .HasForeignKey(d => d.ScheduleId)
                .HasConstraintName("FK__HealthChe__Sched__74AE54BC");

            entity.Property(e => e.HealthProfileId).HasColumnName("HealthProfileID");

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
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Student).WithMany(p => p.HealthCheckSchedules)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__HealthChe__Stude__6FE99F9F");

            entity.HasOne(d => d.User).WithMany(p => p.HealthCheckSchedules)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__HealthChe__UserI__70DDC3D8");
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
            entity.Property(e => e.EventDateTime).HasColumnType("datetime");
            entity.Property(e => e.EventDescription).HasMaxLength(255);
            entity.Property(e => e.EventType).HasMaxLength(30);
            entity.Property(e => e.Location).HasMaxLength(60);
            entity.Property(e => e.RecordedId).HasColumnName("RecordedID");
            entity.Property(e => e.SeverityLevel).HasMaxLength(30);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Student).WithMany(p => p.MedicalEvents)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__MedicalEv__Stude__59FA5E80");

            entity.HasOne(d => d.User).WithMany(p => p.MedicalEvents)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__MedicalEv__UserI__5AEE82B9");
        });

        modelBuilder.Entity<MedicalInventory>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__MedicalI__727E83EBABA65EC3");

            entity.ToTable("MedicalInventory");

            entity.Property(e => e.ItemId)
                .ValueGeneratedNever()
                .HasColumnName("ItemID");
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.ItemName).HasMaxLength(100);
            entity.Property(e => e.UnitOfMeasure).HasMaxLength(20);
        });

        modelBuilder.Entity<MedicalRegistration>(entity =>
        {
            entity.HasKey(e => e.RegistrationId).HasName("PK__MedicalR__6EF58830C8922221");

            entity.ToTable("MedicalRegistration");

            entity.Property(e => e.RegistrationId)
                .ValueGeneratedNever()
                .HasColumnName("RegistrationID");
            entity.Property(e => e.Dosage).HasMaxLength(30);
            entity.Property(e => e.MedicationName).HasMaxLength(100);
            entity.Property(e => e.Notes).HasMaxLength(255);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Student).WithMany(p => p.MedicalRegistrations)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__MedicalRe__Stude__5629CD9C");

            entity.HasOne(d => d.User).WithMany(p => p.MedicalRegistrations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__MedicalRe__UserI__571DF1D5");
        });

        modelBuilder.Entity<MedicalRequest>(entity =>
        {
            entity.HasKey(e => e.RequestItemId).HasName("PK__MedicalR__3F51AD77BE2FAFF0");

            entity.ToTable("MedicalRequest");

            entity.Property(e => e.RequestItemId)
                .ValueGeneratedNever()
                .HasColumnName("RequestItemID");
            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.Notes).HasMaxLength(255);
            entity.Property(e => e.Purpose).HasMaxLength(255);

            entity.HasOne(d => d.Event).WithMany(p => p.MedicalRequests)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK__MedicalRe__Event__5FB337D6");

            entity.HasOne(d => d.Item).WithMany(p => p.MedicalRequests)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__MedicalRe__ItemI__60A75C0F");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E32256F768A");

            entity.ToTable("Notification");

            entity.Property(e => e.NotificationId)
                .ValueGeneratedNever()
                .HasColumnName("NotificationID");
            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.HealthCheckScheduleId).HasColumnName("HealthCheckScheduleID");
            entity.Property(e => e.SendDate).HasColumnType("datetime");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.VaccineScheduleId).HasColumnName("VaccineScheduleID");

            entity.HasOne(d => d.Student).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__Notificat__Stude__7D439ABD");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Notificat__UserI__7E37BEF6");
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
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.DayOfBirth).HasColumnName("DayOfBirth");
            entity.Property(e => e.Gender).HasMaxLength(3);
            entity.Property(e => e.Grade)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.ParentPhoneNumber)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.ParentEmailAddress)
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User)
                .WithMany(p => p.Students)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Student__UserID__4F7CD00D");
            // 1-1 Student-HealthProfile đã cấu hình ở HealthProfile
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCAC7059EAEE");

            entity.ToTable("User");

            // Unique constraint on PhoneNumber
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
            entity.Property(e => e.RefreshTokenExpiryTime).HasColumnType("datetime");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Address).HasMaxLength(255);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
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
            entity.Property(e => e.ImmediateReaction).HasMaxLength(100);
            entity.Property(e => e.InjectionSite).HasMaxLength(100);
            entity.Property(e => e.Notes).HasMaxLength(255);
            entity.Property(e => e.ReactionStartTime).HasColumnType("datetime");
            entity.Property(e => e.ReactionType).HasMaxLength(100);
            entity.Property(e => e.RecordedId).HasColumnName("RecordedID");
            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.SeverityLevel).HasMaxLength(50);

            entity.HasOne(d => d.Schedule).WithMany(p => p.VaccinationResults)
                .HasForeignKey(d => d.ScheduleId)
                .HasConstraintName("FK__Vaccinati__Sched__6D0D32F4");

            entity.Property(e => e.HealthProfileId).HasColumnName("HealthProfileID");

            entity.HasOne(vr => vr.HealthProfile)
                .WithMany(hp => hp.VaccinationResults)
                .HasForeignKey(vr => vr.HealthProfileId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<VaccinationSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__Vaccinat__9C8A5B694492A7A5");

            entity.ToTable("VaccinationSchedule");

            entity.Property(e => e.ScheduleId)
                .ValueGeneratedNever()
                .HasColumnName("ScheduleID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Round).HasMaxLength(10);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.TargetGrade).HasMaxLength(12);
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.VaccineId).HasColumnName("VaccineID");

            entity.HasOne(d => d.Student).WithMany(p => p.VaccinationSchedules)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__Vaccinati__Stude__68487DD7");

            entity.HasOne(d => d.Vaccine).WithMany(p => p.VaccinationSchedules)
                .HasForeignKey(d => d.VaccineId)
                .HasConstraintName("FK__Vaccinati__Vacci__693CA210");
        });

        modelBuilder.Entity<VaccineDetail>(entity =>
        {
            entity.HasKey(e => e.VaccineId).HasName("PK__VaccineD__45DC68E9F459710F");

            entity.Property(e => e.VaccineId)
                .ValueGeneratedNever()
                .HasColumnName("VaccineID");
            entity.Property(e => e.AgeRecommendation).HasMaxLength(50);
            entity.Property(e => e.BatchNumber).HasMaxLength(50);
            entity.Property(e => e.ContraindicationNotes).HasMaxLength(255);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Disease).HasMaxLength(100);
            entity.Property(e => e.Manufacturer).HasMaxLength(100);
            entity.Property(e => e.VaccineName).HasMaxLength(100);
            entity.Property(e => e.VaccineType).HasMaxLength(50);
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
            entity.Property(e => e.BatchNumber).HasMaxLength(50);
            entity.Property(e => e.VaccinatedDate).HasColumnType("date");

            entity.HasOne(d => d.HealthProfile)
                .WithMany(p => p.VaccinationDeclarations)
                .HasForeignKey(d => d.HealthProfileId)
                .HasConstraintName("FK_VaccinationDeclaration_HealthProfile");
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
