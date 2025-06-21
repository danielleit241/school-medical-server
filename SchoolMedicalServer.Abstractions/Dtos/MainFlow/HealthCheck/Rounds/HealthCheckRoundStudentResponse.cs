namespace SchoolMedicalServer.Abstractions.Dtos.MainFlow.HealthCheck.Rounds
{
    public class HealthCheckRoundStudentResponse
    {
        public StudentsOfRoundResponse StudentsOfRoundResponse { get; set; } = new();
        public ParentOfStudentResponse ParentOfStudent { get; set; } = new();
    }

    public class StudentsOfRoundResponse
    {
        public Guid? StudentId { get; set; }

        public Guid? HealthCheckResultId { get; set; }

        public string? StudentCode { get; set; }

        public string? FullName { get; set; }

        public DateOnly? DayOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? Grade { get; set; }
    }

    public class ParentOfStudentResponse
    {
        public Guid? UserId { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? ParentConfirm { get; set; }
    }
}
