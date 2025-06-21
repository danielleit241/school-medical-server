namespace SchoolMedicalServer.Abstractions.Dtos.MainFlow.HealthCheck.Schedules
{
    public class HealthCheckScheduleResponse
    {
        public HealthCheckScheduleResponseDto HealthCheckScheduleResponseDto { get; set; } = new();

    }

    public class HealthCheckScheduleResponseDto
    {
        public Guid ScheduleId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateOnly? ParentNotificationStartDate { get; set; }
        public DateOnly? ParentNotificationEndDate { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
