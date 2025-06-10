namespace SchoolMedicalServer.Abstractions.Entities;

public partial class VaccinationResult
{
    public Guid VaccinationResultId { get; set; }
    public Guid ScheduleId { get; set; }
    public Guid RoundId { get; set; }
    public Guid HealthProfileId { get; set; }
    public DateOnly? VaccinationDate { get; set; }
    public string? Status { get; set; }
    public string? Notes { get; set; }
    public Guid? RecorderId { get; set; }

    public virtual VaccinationSchedule? Schedule { get; set; }
    public virtual HealthProfile? HealthProfile { get; set; }
    public virtual VaccinationObservation? VaccinationObservation { get; set; }
}
