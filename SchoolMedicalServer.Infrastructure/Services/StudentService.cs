using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Dtos.Student;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class StudentService(SchoolMedicalManagementContext context) : IStudentService
    {
        private readonly SchoolMedicalManagementContext context = context;

        public async Task<IEnumerable<StudentDto>?> GetParentStudentsAsync(Guid parentId)
        {
            var students = await context.Students.Include(s => s.User).Where(s => s.UserId == parentId).ToListAsync();
            if (students == null) return null;

            var response = students.Select(s => new StudentDto
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
            });

            return response;
        }

        public async Task<StudentDto?> GetParentStudentAsync(Guid parentId, Guid studentId)
        {
            var student = await context.Students.Include(s => s.User)
                .Where(s => s.UserId == parentId && s.StudentId == studentId)
                .FirstOrDefaultAsync();

            if (student == null) return null;

            var response = new StudentDto
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
