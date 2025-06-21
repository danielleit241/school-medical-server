using System.Text.Json.Serialization;

namespace SchoolMedicalServer.Abstractions.Entities;

public partial class User
{
    public Guid UserId { get; set; }
    public int? RoleId { get; set; }
    public string? FullName { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string? EmailAddress { get; set; }
    public string? AvatarUrl { get; set; }
    public DateOnly? DayOfBirth { get; set; }
    public string? RefreshToken { get; set; }
    public bool? Status { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public string? Address { get; set; }
    public string? Otp { get; set; }
    public DateTime? OtpExpiryTime { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    [JsonIgnore]
    public virtual ICollection<MedicalEvent> MedicalEvents { get; set; } = new List<MedicalEvent>();
    [JsonIgnore]
    public virtual ICollection<MedicalRegistration> MedicalRegistrations { get; set; } = new List<MedicalRegistration>();
    [JsonIgnore]
    public virtual Role? Role { get; set; }
    [JsonIgnore]
    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    [JsonIgnore]
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
