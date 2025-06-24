using System.ComponentModel.DataAnnotations;

namespace SchoolMedicalServer.Abstractions.Dtos.MainFlows.HealthCheck.Rounds
{
    public class HealthCheckRoundRequest
    {
        [Required]
        public Guid? ScheduleId { get; set; }
        [Required]
        public string? RoundName { get; set; }
        [Required]
        public string? TargetGrade { get; set; }
        [Required]
        public DateTime? StartTime { get; set; }
        [Required]
        public DateTime? EndTime { get; set; }
        [Required]
        public Guid NurseId { get; set; }
        public string? Description { get; set; }
    }
}
