using System.ComponentModel.DataAnnotations;

namespace SchoolMedicalServer.Abstractions.Dtos.MainFlows.HealthCheck.Schedules
{
    public class HealthCheckScheduleRequest
    {
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? HealthCheckType { get; set; }
        public string? Description { get; set; }
        [Required]
        public DateOnly? StartDate { get; set; }
        [Required]
        public DateOnly? EndDate { get; set; }
        [Required]
        public Guid CreatedBy { get; set; }
        public IEnumerable<HealthCheckRoundRequestDto> HealthCheckRounds { get; set; } = default!;

    }

    public class HealthCheckRoundRequestDto
    {
        [Required]
        public string? RoundName { get; set; }
        [Required]
        public string? TargetGrade { get; set; }
        public string? Description { get; set; }
        [Required]
        public DateTime? StartTime { get; set; }
        [Required]
        public DateTime? EndTime { get; set; }
        [Required]
        public Guid NurseId { get; set; }
    }
}
