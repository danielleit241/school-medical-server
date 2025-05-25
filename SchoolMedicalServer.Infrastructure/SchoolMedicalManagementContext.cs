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

    public virtual DbSet<HealthDeclaration> HealthDeclarations { get; set; }

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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(local);Database= SchoolMedicalManagement;UID=sa;PWD=12345;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCA22BCB7532");

            entity.ToTable("Appointment");

            entity.Property(e => e.AppointmentId)
                .ValueGeneratedNever()
                .HasColumnName("AppointmentID");
            entity.Property(e => e.AppointmentReason).HasMaxLength(255);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Student).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__Appointme__Stude__52593CB8");

            entity.HasOne(d => d.User).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Appointme__UserI__534D60F1");
        });

        modelBuilder.Entity<HealthCheckResult>(entity =>
        {
            entity.HasKey(e => e.ResultId).HasName("PK__HealthCh__976902280D4BD666");

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
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Schedule).WithMany(p => p.HealthCheckResults)
                .HasForeignKey(d => d.ScheduleId)
                .HasConstraintName("FK__HealthChe__Sched__75A278F5");

            entity.HasOne(d => d.Student).WithMany(p => p.HealthCheckResults)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__HealthChe__Stude__74AE54BC");
        });

        modelBuilder.Entity<HealthCheckSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__HealthCh__9C8A5B69FEDAE84D");

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
                .HasConstraintName("FK__HealthChe__Stude__70DDC3D8");

            entity.HasOne(d => d.User).WithMany(p => p.HealthCheckSchedules)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__HealthChe__UserI__71D1E811");
        });

        modelBuilder.Entity<HealthDeclaration>(entity =>
        {
            entity.HasKey(e => e.HealthDeclarationId).HasName("PK__HealthDe__327AAD7D84759B75");

            entity.ToTable("HealthDeclaration");

            entity.Property(e => e.HealthDeclarationId)
                .ValueGeneratedNever()
                .HasColumnName("HealthDeclarationID");
            entity.Property(e => e.AdministeredVaccines).HasMaxLength(255);
            entity.Property(e => e.ChronicDiseases).HasMaxLength(255);
            entity.Property(e => e.DrugAllergies).HasMaxLength(255);
            entity.Property(e => e.FoodAllergies).HasMaxLength(255);
            entity.Property(e => e.Notes).HasMaxLength(255);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.VaccineId).HasColumnName("VaccineID");

            entity.HasOne(d => d.Student).WithMany(p => p.HealthDeclarations)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__HealthDec__Stude__66603565");

            entity.HasOne(d => d.Vaccine).WithMany(p => p.HealthDeclarations)
                .HasForeignKey(d => d.VaccineId)
                .HasConstraintName("FK__HealthDec__Vacci__656C112C");
        });

        modelBuilder.Entity<MedicalEvent>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__MedicalE__7944C87040FECA9E");

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
            entity.HasKey(e => e.ItemId).HasName("PK__MedicalI__727E83EB613E5005");

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
            entity.HasKey(e => e.RegistrationId).HasName("PK__MedicalR__6EF58830002FCD0C");

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
            entity.HasKey(e => e.RequestItemId).HasName("PK__MedicalR__3F51AD779A9EAEC2");

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
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E32D2FD02B9");

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
                .HasConstraintName("FK__Notificat__Stude__787EE5A0");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Notificat__UserI__797309D9");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE3AB9E100EC");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Student__32C52A79E34805F8");

            entity.ToTable("Student");

            entity.Property(e => e.StudentId)
                .ValueGeneratedNever()
                .HasColumnName("StudentID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(3);
            entity.Property(e => e.Grade)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ParentEmailAddress)
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.ParentPhoneNumber)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.StudentCode)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Students)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Student__UserID__4F7CD00D");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCACE402F678");

            entity.ToTable("User");

            entity.HasIndex(e => e.PhoneNumber, "UQ__User__85FB4E3808A6DCD1").IsUnique();

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("UserID");
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
            entity.Property(e => e.RefreshTokenExpiryTime).HasColumnType("datetime");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__User__RoleID__4CA06362");
        });

        modelBuilder.Entity<VaccinationResult>(entity =>
        {
            entity.HasKey(e => e.VaccinationResultId).HasName("PK__Vaccinat__12DE8FD94A304570");

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
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Schedule).WithMany(p => p.VaccinationResults)
                .HasForeignKey(d => d.ScheduleId)
                .HasConstraintName("FK__Vaccinati__Sched__6E01572D");

            entity.HasOne(d => d.Student).WithMany(p => p.VaccinationResults)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__Vaccinati__Stude__6D0D32F4");
        });

        modelBuilder.Entity<VaccinationSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__Vaccinat__9C8A5B6975E3525E");

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
                .HasConstraintName("FK__Vaccinati__Stude__693CA210");

            entity.HasOne(d => d.Vaccine).WithMany(p => p.VaccinationSchedules)
                .HasForeignKey(d => d.VaccineId)
                .HasConstraintName("FK__Vaccinati__Vacci__6A30C649");
        });

        modelBuilder.Entity<VaccineDetail>(entity =>
        {
            entity.HasKey(e => e.VaccineId).HasName("PK__VaccineD__45DC68E9E9168660");

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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
