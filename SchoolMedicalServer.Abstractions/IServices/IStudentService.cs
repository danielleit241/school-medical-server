using SchoolMedicalServer.Abstractions.Dtos.Student;
using SchoolMedicalServer.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentDTO>?> GetAllStudentsByParentIdAsync(Guid parentId);
        Task<StudentDTO?> GetStudentByIdForParentAsync(Guid parentId, Guid studentId);

    }
}
