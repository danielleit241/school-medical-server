namespace SchoolMedicalServer.Abstractions.Entities;

public partial class Student
{
    public Guid StudentId { get; set; }

    public Guid? UserId { get; set; }

    public string? StudentCode { get; set; }

    public string FullName { get; set; } = null!;

    public DateOnly? DayOfBirth { get; set; }

    public string? Gender { get; set; }

    public string? Grade { get; set; }

    public string? Address { get; set; }

    public string? ParentPhoneNumber { get; set; }

    public string? ParentEmailAddress { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<HealthCheckResult> HealthCheckResults { get; set; } = new List<HealthCheckResult>();

    public virtual ICollection<HealthCheckSchedule> HealthCheckSchedules { get; set; } = new List<HealthCheckSchedule>();

    public virtual HealthProfile? HealthProfile { get; set; }

    public virtual ICollection<MedicalEvent> MedicalEvents { get; set; } = new List<MedicalEvent>();

    public virtual ICollection<MedicalRegistration> MedicalRegistrations { get; set; } = new List<MedicalRegistration>();

    public virtual User? User { get; set; }

    public virtual ICollection<VaccinationResult> VaccinationResults { get; set; } = new List<VaccinationResult>();

    public virtual ICollection<VaccinationSchedule> VaccinationSchedules { get; set; } = new List<VaccinationSchedule>();
}
