namespace SchoolMedicalServer.Abstractions.Dtos.Student
{
    public class ParentStudentResponse(IEnumerable<StudentDto> students)
    {
        public IEnumerable<StudentDto> Students { get; set; } = students;
    }
}
