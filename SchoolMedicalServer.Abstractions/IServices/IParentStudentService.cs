using SchoolMedicalServer.Abstractions.Dtos.Student;
using SchoolMedicalServer.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IParentStudentService
    {
        Task<IEnumerable<ParentStudentDto>> GetAllStudentsAsync(Guid parentUserId);
        Task<ParentStudentDto?> GetStudentByIdAsync(Guid parentUserId, Guid studentId);

    }
}
