namespace SchoolMedicalServer.Abstractions.Dtos.Student
{
    public class ParentStudentDTO
    {
       public IEnumerable<StudentDTO> Students { get; set; } = new List<StudentDTO>();
    }
}
