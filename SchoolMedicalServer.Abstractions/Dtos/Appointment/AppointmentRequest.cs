namespace SchoolMedicalServer.Abstractions.Dtos.Appointment
{
    public class AppointmentRequest
    {
        public Guid? StudentId { get; set; }

        public Guid? StaffNurseId { get; set; }

        public string? Topic { get; set; }

        public DateOnly? AppointmentDate { get; set; }

        public TimeOnly? AppointmentStartTime { get; set; }

        public TimeOnly? AppointmentEndTime { get; set; }

        public string? AppointmentReason { get; set; }
    }
}
