using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Dtos.Student;
using SchoolMedicalServer.Abstractions.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class StudentService(SchoolMedicalManagementContext context) : IStudentService
    {
        public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
        {
            var students = await context.Students
            .Select(s => new StudentDto
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
        })
            .ToListAsync();

            return students;
        }
    }
} 
