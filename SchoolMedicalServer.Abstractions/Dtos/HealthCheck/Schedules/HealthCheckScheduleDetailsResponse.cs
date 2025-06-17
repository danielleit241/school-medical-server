namespace SchoolMedicalServer.Abstractions.Dtos.HealthCheck.Schedules
{
    public class HealthCheckScheduleDetailsResponse
    {
        public IEnumerable<HealthCheckRoundResponseDto> HealthCheckRounds { get; set; } = default!;
    }

    public class HealthCheckRoundResponseDto
    {
        public Guid RoundId { get; set; }
        public string? RoundName { get; set; }
        public string? TargetGrade { get; set; }
        public string? Description { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Guid NurseId { get; set; }
        public bool Status { get; set; }
    }
}
