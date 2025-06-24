using System.ComponentModel.DataAnnotations;

namespace SchoolMedicalServer.Abstractions.Dtos.MainFlows.HealthCheck.Schedules
{
    public class HealthCheckScheduleUpdateRequest
    {
        [Required]
        public Guid ScheduleId { get; set; }
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
    }
}
