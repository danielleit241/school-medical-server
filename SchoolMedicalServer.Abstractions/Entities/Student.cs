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
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = [];
    public virtual HealthProfile? HealthProfile { get; set; }
    public virtual ICollection<MedicalEvent> MedicalEvents { get; set; } = [];
    public virtual ICollection<MedicalRegistration> MedicalRegistrations { get; set; } = [];
    public virtual User? User { get; set; }
}
