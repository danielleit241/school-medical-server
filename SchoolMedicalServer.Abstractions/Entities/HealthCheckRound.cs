namespace SchoolMedicalServer.Abstractions.Entities
{
    public class HealthCheckRound
    {
        public Guid RoundId { get; set; }
        public Guid ScheduleId { get; set; }
        public string? RoundName { get; set; }
        public string? TargetGrade { get; set; }
        public string? Description { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool Status { get; set; }
        public Guid NurseId { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual HealthCheckSchedule? Schedule { get; set; }
        public virtual ICollection<HealthCheckResult> HealthCheckResults { get; set; } = [];
    }
}