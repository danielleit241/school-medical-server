namespace SchoolMedicalServer.Abstractions.Dtos.Student
{
    public class StudentDto
    {
        public Guid StudentId { get; set; }

        public string? StudentCode { get; set; }

        public string? FullName { get; set; }

        public DateOnly DayOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? Grade { get; set; }

        public string? Address { get; set; }

        public string? ParentPhoneNumber { get; set; }

        public string? ParentEmailAddress { get; set; }
    }
}
