using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Dtos.Student;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class ParentStudentService(SchoolMedicalManagementContext context) : IParentStudentService
    {
        private readonly SchoolMedicalManagementContext context = context;

        public async Task<IEnumerable<StudentInformationResponse>?> GetParentStudentsAsync(Guid parentId)
        {
            var students = await context.Students
                .Include(s => s.User)
                .Where(s => s.UserId == parentId)
                .ToListAsync();

            List<StudentInformationResponse> response = new();
            foreach (var student in students)
            {
                response.Add(GetStudentInformationResponse(student));
            }

            return response;
        }

        public async Task<StudentInformationResponse?> GetParentStudentAsync(Guid parentId, Guid studentId)
        {
            var student = await context.Students.Include(s => s.User)
                .Where(s => s.UserId == parentId && s.StudentId == studentId)
                .FirstOrDefaultAsync();

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
