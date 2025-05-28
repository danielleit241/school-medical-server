using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Dtos.Student;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class ParentStudentService(SchoolMedicalManagementContext context) : IParentStudentService
    {
        public async Task<IEnumerable<ParentStudentDto>> GetAllStudentsAsync(Guid parentUserId)
        {
            return await context.Students
                .Where(s => s.UserId == parentUserId)
                .Select(s => new ParentStudentDto
                {
                    Id = s.StudentId,
                    FullName = s.FullName,
                    DateOfBirth = s.DayOfBirth ?? default,
                    AvatarURL = s.User != null ? s.User.AvatarUrl ?? "" : "",
                    Gender = s.Gender,
                    Grade = s.Grade,
                    Address = s.Address,
                    ParentPhoneNumber = s.ParentPhoneNumber,
                    ParentEmailAddress = s.ParentEmailAddress
                })
                .ToListAsync();
        }

        public async Task<ParentStudentDto?> GetStudentByIdAsync(Guid parentUserId, Guid studentId)
        {
            return await context.Students
                .Where(s => s.UserId == parentUserId && s.StudentId == studentId)
                .Select(s => new ParentStudentDto
                {
                    Id = s.StudentId,
                    FullName = s.FullName,
                    DateOfBirth = s.DayOfBirth ?? default,
                    AvatarURL = s.User != null ? s.User.AvatarUrl ?? "" : "",
                    Gender = s.Gender,
                    Grade = s.Grade,
                    Address = s.Address,
                    ParentPhoneNumber = s.ParentPhoneNumber,
                    ParentEmailAddress = s.ParentEmailAddress
                })
                .FirstOrDefaultAsync();
        }
    }
}
