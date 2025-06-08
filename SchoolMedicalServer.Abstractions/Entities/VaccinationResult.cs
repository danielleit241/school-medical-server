namespace SchoolMedicalServer.Abstractions.Entities;

public partial class VaccinationResult
{
    public Guid VaccinationResultId { get; set; }
    public Guid? ScheduleId { get; set; }
    public int? DoseNumber { get; set; }
    public DateOnly? VaccinationDate { get; set; }
    public string? InjectionSite { get; set; }
    public string? ImmediateReaction { get; set; }
    public DateTime? ReactionStartTime { get; set; }
    public string? ReactionType { get; set; }
    public string? SeverityLevel { get; set; }
    public string? Notes { get; set; }
    public Guid? RecordedId { get; set; }
    public Guid HealthProfileId { get; set; }

    public virtual VaccinationSchedule? Schedule { get; set; }
    public virtual HealthProfile? HealthProfile { get; set; }
}
