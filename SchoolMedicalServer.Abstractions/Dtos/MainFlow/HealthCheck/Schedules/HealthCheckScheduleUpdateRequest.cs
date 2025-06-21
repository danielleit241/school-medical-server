namespace SchoolMedicalServer.Abstractions.Dtos.MainFlow.HealthCheck.Schedules
{
    public class HealthCheckScheduleUpdateRequest
    {
        public Guid ScheduleId { get; set; }
        public string? Title { get; set; }
        public string? HealthCheckType { get; set; }
        public string? Description { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
