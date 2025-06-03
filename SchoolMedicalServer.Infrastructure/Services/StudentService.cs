using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
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
        public async Task<PaginationResponse<StudentInformationResponse>> GetAllStudentsAsync(PaginationRequest? paginationRequest)
        {
            var totalCount = await context.Students.CountAsync();

            var students = await context.Students
                .Skip((paginationRequest.PageIndex - 1) * paginationRequest.PageSize)
                .Take(paginationRequest.PageSize)
                .Select(s => new StudentInformationResponse
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

            return new PaginationResponse<StudentInformationResponse>(
                paginationRequest.PageIndex,
                paginationRequest.PageSize,
                totalCount,
                students
            );
        }
    }
}
