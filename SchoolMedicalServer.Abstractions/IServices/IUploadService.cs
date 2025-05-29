
using Microsoft.AspNetCore.Http;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IUploadService
    {
        Task UploadExcelFile(IFormFile file);
    }
}
