namespace SchoolMedicalServer.Abstractions.Entities;

public partial class HealthCheckResult
{
    public Guid ResultId { get; set; }
    public Guid RoundId { get; set; }
    public Guid HealthProfileId { get; set; }
    public bool? ParentConfirmed { get; set; } = null;
    public DateOnly? DatePerformed { get; set; }
    public double? Height { get; set; }
    public double? Weight { get; set; }
    public double? VisionLeft { get; set; }
    public double? VisionRight { get; set; }
    public string? Hearing { get; set; }
    public string? Nose { get; set; }
    public string? BloodPressure { get; set; }
    public string? Status { get; set; }
    public string? Notes { get; set; }
    public Guid? RecordedId { get; set; }
    public DateTime? RecordedAt { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public virtual HealthCheckRound? Round { get; set; }
    public virtual HealthProfile? HealthProfile { get; set; }
}
