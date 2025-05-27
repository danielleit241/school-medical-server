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
    public class ParentStudentService : IParentStudentService
    {
        private readonly SchoolMedicalManagementContext context;

        public ParentStudentService(SchoolMedicalManagementContext context)
        {
            this.context = context;
        }

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
