using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Dtos.Student;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class StudentService(IStudentRepository studentRepository) : IStudentService
    {
        public async Task<PaginationResponse<StudentInformationResponse>> GetAllStudentsAsync(PaginationRequest? paginationRequest)
        {
            var totalCount = await studentRepository.CountAsync();

            int skip = (paginationRequest!.PageIndex - 1) * paginationRequest.PageSize;
            var students = await studentRepository.GetPagedAsync(skip, paginationRequest.PageSize);

            var studentDtos = students.Select(s => new StudentInformationResponse
            {
                StudentId = s.StudentId,
                StudentCode = s.StudentCode,
                FullName = s.FullName,
                DayOfBirth = s.DayOfBirth ?? default,
                Gender = s.Gender,
                Grade = s.Grade,
                Address = s.Address,
                ParentPhoneNumber = s.ParentPhoneNumber,
                ParentEmailAddress = s.ParentEmailAddress
            }).ToList();

            return new PaginationResponse<StudentInformationResponse>(
                paginationRequest.PageIndex,
                paginationRequest.PageSize,
                totalCount,
                studentDtos
            );
        }
    }
}
