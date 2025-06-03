using SchoolMedicalServer.Abstractions.Dtos.Student;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IParentStudentService
    {
        Task<StudentInformationResponse?> GetParentStudentAsync(Guid parentId, Guid studentId);
        Task<IEnumerable<StudentInformationResponse>?> GetParentStudentsAsync(Guid parentId);
    }
}
