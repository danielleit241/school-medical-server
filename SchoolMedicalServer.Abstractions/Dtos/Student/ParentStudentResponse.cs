namespace SchoolMedicalServer.Abstractions.Dtos.Student
{
    public class ParentStudentResponse(IEnumerable<StudentInformationResponse> students)
    {
        public IEnumerable<StudentInformationResponse> Students { get; set; } = students;
    }
}
