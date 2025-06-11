 using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Dtos.Student;
using SchoolMedicalServer.Abstractions.Entities;
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
            var students = await studentRepository.GetPagedAsync(
                paginationRequest.Search!,
                paginationRequest.SortBy!,
                paginationRequest.SortOrder!,
                skip, paginationRequest.PageSize);

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

        public async Task<StudentInformationResponse?> UpdateStudentInformationAsync(Guid studentId, StudentInformationRequest request)
        {
            var student = await studentRepository.GetStudentByIdAsync(studentId);
            if (student == null) return null;

            student.StudentCode = request.StudentCode;
            student.FullName = request.FullName!;
            student.DayOfBirth = request.DayOfBirth;
            student.Gender = request.Gender;
            student.Grade = request.Grade;
            student.Address = request.Address;
            student.ParentPhoneNumber = request.ParentPhoneNumber;
            student.ParentEmailAddress = request.ParentEmailAddress;

            studentRepository.UpdateStudent(student);
            await baseRepository.SaveChangesAsync();

            return ToStudentInformationResponse(student);
        }


        private static StudentInformationResponse ToStudentInformationResponse(Student student)
        {
            return new StudentInformationResponse
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
        }

        public async Task<StudentInformationResponse?> GetStudentByIdAsync(Guid studentId)
        {
            var student = await studentRepository.GetStudentByIdAsync(studentId);
            if (student == null) return null;
            return ToStudentInformationResponse(student);
        }
    }
}
