using SchoolMedicalServer.Abstractions.Dtos.Student;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IParentStudentService
    {
        Task<StudentDto?> GetParentStudentAsync(Guid parentId, Guid studentId);
        Task<IEnumerable<StudentDto>?> GetParentStudentsAsync(Guid parentId);
    }
}
