using System.Text.Json.Serialization;

namespace SchoolMedicalServer.Abstractions.Entities;

public partial class MedicalRegistration
{
    public Guid RegistrationId { get; set; }

    public Guid? StudentId { get; set; }

    public Guid? UserId { get; set; }

    public DateOnly? DateSubmitted { get; set; }

    public string? MedicationName { get; set; }

    public string? TotalDosages { get; set; }

    public string? Notes { get; set; }

    public bool? ParentalConsent { get; set; }

    public Guid? StaffNurseId { get; set; }

    public DateOnly? DateApproved { get; set; }

    public bool Status { get; set; }

    [JsonIgnore]
    public virtual Student? Student { get; set; }
    [JsonIgnore]
    public virtual User? User { get; set; }
    [JsonIgnore]
    public virtual ICollection<MedicalRegistrationDetails> Details { get; set; } = new List<MedicalRegistrationDetails>();

}
