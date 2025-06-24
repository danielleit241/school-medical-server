namespace SchoolMedicalServer.Abstractions.Dtos.MainFlows.Vaccination.Rounds
{
    public class VaccinationRoundStudentResponse
    {
        public StudentsOfRoundResponse StudentsOfRoundResponse { get; set; } = new StudentsOfRoundResponse();
        public ParentOfStudentResponse ParentsOfStudent { get; set; } = new ParentOfStudentResponse();
    }

    public class StudentsOfRoundResponse
    {
        public Guid? StudentId { get; set; }

        public Guid? VaccinationResultId { get; set; }

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
