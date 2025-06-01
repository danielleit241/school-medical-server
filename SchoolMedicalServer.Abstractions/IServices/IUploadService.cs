
using Microsoft.AspNetCore.Http;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IUploadService
    {
        Task UploadMedicalInventoriesExcelFile(IFormFile file);
        Task UploadStudentsExcelFile(IFormFile file);
    }
}
