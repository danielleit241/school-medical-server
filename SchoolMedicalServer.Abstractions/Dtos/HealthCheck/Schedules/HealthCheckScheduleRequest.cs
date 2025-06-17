namespace SchoolMedicalServer.Abstractions.Dtos.HealthCheck.Schedules
{
    public class HealthCheckScheduleRequest
    {
        public string? Title { get; set; }
        public string? HealthCheckType { get; set; }
        public string? Description { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public Guid CreatedBy { get; set; }
        public IEnumerable<HealthCheckRoundRequestDto> HealthCheckRounds { get; set; } = default!;

    }

    public class HealthCheckRoundRequestDto
    {
        public string? RoundName { get; set; }
        public string? TargetGrade { get; set; }
        public string? Description { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Guid NurseId { get; set; }
    }
}
