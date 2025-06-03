using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Dtos.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IStudentService
    {
        Task<PaginationResponse<StudentInformationResponse>> GetAllStudentsAsync(PaginationRequest? paginationRequest);
      
    }
}
