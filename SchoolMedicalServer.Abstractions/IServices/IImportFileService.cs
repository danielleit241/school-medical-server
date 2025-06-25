using Microsoft.AspNetCore.Http;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IImportFileService
    {
        Task UploadMedicalInventoriesExcelFile(IFormFile file);
        Task UploadStudentsExcelFile(IFormFile file);
        Task UploadVaccinationDetailFile(IFormFile file);
    }
}
