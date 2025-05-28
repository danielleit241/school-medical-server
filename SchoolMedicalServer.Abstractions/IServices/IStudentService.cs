using SchoolMedicalServer.Abstractions.Dtos.Student;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IStudentService
    {
        Task<StudentDto?> GetParentStudentAsync(Guid parentId, Guid studentId);
        Task<IEnumerable<StudentDto>?> GetParentStudentsAsync(Guid parentId);
    }
}
