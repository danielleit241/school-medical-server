using SchoolMedicalServer.Abstractions.Dtos.Student;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class ParentStudentService(IStudentRepository studentRepository) : IParentStudentService
    {

        public async Task<IEnumerable<StudentInformationResponse>?> GetParentStudentsAsync(Guid parentId)
        {
            var students = await studentRepository.GetByParentIdAsync(parentId);

            List<StudentInformationResponse> response = new();
            foreach (var student in students)
            {
                response.Add(GetStudentInformationResponse(student));
            }

            return response;
        }

        public async Task<StudentInformationResponse?> GetParentStudentAsync(Guid parentId, Guid studentId)
        {
            var student = await studentRepository.GetByParentIdAndStudentIdAsync(parentId, studentId);

            if (student == null) return null;

            var response = GetStudentInformationResponse(student);
            return response;
        }

        private StudentInformationResponse GetStudentInformationResponse(Student student)
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
    }
}
