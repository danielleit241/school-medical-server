using SchoolMedicalServer.Abstractions.Dtos.MedicalEvent;
using SchoolMedicalServer.Abstractions.Dtos.MedicalRegistration;
using SchoolMedicalServer.Abstractions.Entities;

namespace SchoolMedicalServer.Abstractions.IRepositories
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllAsync();
        Task AddAsync(Student student);
        void UpdateStudent(Student student);
        Task<List<string>> GetParentsPhoneNumber();
        Task<List<Student>> GetStudentsWithParentPhoneAsync();
        Task<int> CountAsync();
        Task<List<Student>> GetPagedAsync(int skip, int take);
        Task<List<Student>> GetByParentIdAsync(Guid parentId);
        Task<Student?> GetByParentIdAndStudentIdAsync(Guid parentId, Guid studentId);
        Task<MedicalRegistrationStudentResponse?> GetStudentInfoAsync(Guid? studentId);
        Task<Student?> GetStudentByIdAsync(Guid? studentId);
        Task<Guid?> GetParentUserIdAsync(Guid? studentId);
        Task<Student?> FindByStudentCodeAsync(string studentCode);
        Task<string> GenerateStudentCodeAsync();
    }
}
