using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Dtos.Student;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class StudentService(IBaseRepository baseRepository, IStudentRepository studentRepository) : IStudentService
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

        public async  Task<StudentInformationResponse?> UpdateStudentInformationAsync(Guid studentId, StudentInformationResponse dto)
        {
            var student = await studentRepository.GetStudentByIdAsync(studentId);
            if (student == null) return null;

            student.StudentCode = dto.StudentCode;
            student.FullName = dto.FullName!;
            student.DayOfBirth = dto.DayOfBirth;
            student.Gender = dto.Gender;
            student.Grade = dto.Grade;
            student.Address = dto.Address;
            student.ParentPhoneNumber = dto.ParentPhoneNumber;
            student.ParentEmailAddress = dto.ParentEmailAddress;

            studentRepository.UpdateStudent(student);
            await baseRepository.SaveChangesAsync();

            var response = new StudentInformationResponse
            {
                StudentId = student.StudentId,
                StudentCode = student.StudentCode,
                FullName = student.FullName,
                DayOfBirth = student.DayOfBirth ?? default,
                Gender = student.Gender,
                Grade = student.Grade,
                Address = student.Address,
                ParentPhoneNumber = student.ParentPhoneNumber,
                ParentEmailAddress = student.ParentEmailAddress
            };

            return response;
        }
    }
}
