namespace SchoolMedicalServer.Abstractions.Dtos.Appointment
{
    public class AppointmentResponse
    {
        public Guid AppointmentId { get; set; }
        public StudentInfo? Student { get; set; }
        public UserInfo? User { get; set; }
        public StaffNurseInfo? StaffNurse { get; set; }
        public string? Topic { get; set; }
        public DateOnly? AppointmentDate { get; set; }
        public TimeOnly? AppointmentStartTime { get; set; }
        public TimeOnly? AppointmentEndTime { get; set; }
        public string? AppointmentReason { get; set; }
        public bool? ConfirmationStatus { get; set; }
        public bool? CompletionStatus { get; set; }
    }

    public class StudentInfo
    {
        public Guid? StudentId { get; set; }
        public string? StudentCode { get; set; }
        public string? FullName { get; set; }
    }

    public class UserInfo
    {
        public Guid? UserId { get; set; }
        public string? FullName { get; set; }
    }

    public class StaffNurseInfo
    {
        public Guid? StaffNurseId { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
    }
}