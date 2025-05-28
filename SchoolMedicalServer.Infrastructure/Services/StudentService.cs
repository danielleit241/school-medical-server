using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Dtos.Student;
using SchoolMedicalServer.Abstractions.Dtos.User;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class StudentService : IStudentService
    {
        private readonly SchoolMedicalManagementContext context;

        public StudentService(SchoolMedicalManagementContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<StudentDTO>?> GetAllStudentsByParentIdAsync(Guid parentId)
        {
            var students = await context.Students.Include(s => s.User).Where(s => s.UserId == parentId).ToListAsync();
            if (students == null) return null;

            var response = students.Select(s => new StudentDTO
            {
                StudentId = s.StudentId,
                StudentCode = s.StudentCode,
                FullName = s.FullName,
                DateOfBirth = s.DayOfBirth ?? default,
                AvatarURL = s.User != null ? s.User.AvatarUrl ?? "" : "",
                Gender = s.Gender,
                Grade = s.Grade,
                Address = s.Address,
                ParentPhoneNumber = s.ParentPhoneNumber,
                ParentEmailAddress = s.ParentEmailAddress
            });

            return response;
        }

        public async Task<StudentDTO?> GetStudentByIdForParentAsync(Guid parentId, Guid studentId)
        {
            var student = await context.Students.Include(s => s.User)
                .Where(s => s.UserId == parentId && s.StudentId == studentId)
                .FirstOrDefaultAsync();

            if (student == null) return null;

            var response = new StudentDTO
            {
                StudentId = student.StudentId,
                StudentCode = student.StudentCode,
                FullName = student.FullName,
                DateOfBirth = student.DayOfBirth ?? default,
                AvatarURL = student.User != null ? student.User.AvatarUrl ?? "" : "",
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
