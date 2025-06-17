namespace SchoolMedicalServer.Abstractions.Dtos.HealthCheck.Rounds
{
    public class HealthCheckRoundParentResponse
    {
        public HealthCheckRoundInformationResponse HealthCheckRoundInformationResponse { get; set; } = new();
        public HealthCheckRoundNurseInformationResponse HealthCheckRoundNurseInformationResponse { get; set; } = new();
        public StudentsOfRoundResponse Student { get; set; } = new();
        public ParentOfStudentResponse Parent { get; set; } = new();
    }

    public class HealthCheckRoundInformationResponse
    {
        public Guid RoundId { get; set; }
        public string RoundName { get; set; } = string.Empty;
        public string TargetGrade { get; set; } = string.Empty;
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool Status { get; set; }
    }

    public class HealthCheckRoundNurseInformationResponse
    {
        public Guid NurseId { get; set; }
        public string NurseName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
    }
}
