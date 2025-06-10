
using Microsoft.AspNetCore.Http;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IFileService
    {
        Task UploadMedicalInventoriesExcelFile(IFormFile file);
        Task UploadStudentsExcelFile(IFormFile file);
        Task UploadVaccinationDetailFile(IFormFile file);
        Task<byte[]> ExportStudentsExcelFileAsync();
        Task<byte[]> ExportMedicalInventoriesExcelFileAsync();

    }
}
