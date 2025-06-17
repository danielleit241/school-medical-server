namespace SchoolMedicalServer.Abstractions.Dtos.HealthCheck.Schedules
{
    public class HealthCheckScheduleResponse
    {
        public HealthCheckScheduleResponseDto HealthCheckScheduleResponseDto { get; set; } = new();
        //public HealthCheckRoundInformationResponse HealthCheckRoundInformation { get; set; } = new();
        //public HealthCheckRoundNurseInformationResponse Nurse { get; set; } = new();
    }

    public class HealthCheckScheduleResponseDto
    {
        public Guid ScheduleId { get; set; }
        public Guid? VaccineId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateOnly? ParentNotificationStartDate { get; set; }
        public DateOnly? ParentNotificationEndDate { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public bool Status { get; set; }
    }


    //public class HealthCheckRoundInformationResponse
    //{
    //    public Guid RoundId { get; set; }
    //    public string RoundName { get; set; } = string.Empty;
    //    public string TargetGrade { get; set; } = string.Empty;
    //    public DateTime? StartTime { get; set; }
    //    public DateTime? EndTime { get; set; }
    //    public string Description { get; set; } = string.Empty;
    //    public bool Status { get; set; }
    //}

    //public class HealthCheckRoundNurseInformationResponse
    //{
    //    public Guid NurseId { get; set; }
    //    public string NurseName { get; set; } = string.Empty;
    //    public string PhoneNumber { get; set; } = string.Empty;
    //    public string AvatarUrl { get; set; } = string.Empty;
    //}
}
