using SchoolMedicalServer.Abstractions.Dtos.Appointment;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Dtos.Student;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IStudentService
    {
        Task<PaginationResponse<StudentInformationResponse>> GetAllStudentsAsync(PaginationRequest? paginationRequest);
        Task<StudentInformationResponse?> UpdateStudentInformationAsync(Guid studentId, StudentInformationRequest request);
        Task<StudentInformationResponse?> GetStudentByIdAsync(Guid studentId);
        Task<IEnumerable<StudentInfo>> GetAllStudentsNoPaginationAsync();
    }

}

