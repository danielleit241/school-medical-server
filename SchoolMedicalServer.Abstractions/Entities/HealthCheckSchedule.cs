namespace SchoolMedicalServer.Abstractions.Entities
{
    public partial class HealthCheckSchedule
    {
        public Guid ScheduleId { get; set; }
        public string? Title { get; set; }
        public string? HealthCheckType { get; set; }
        public string? Description { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateOnly? ParentNotificationStartDate { get; set; }
        public DateOnly? ParentNotificationEndDate { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual ICollection<HealthCheckRound> Rounds { get; set; } = [];
    }
}