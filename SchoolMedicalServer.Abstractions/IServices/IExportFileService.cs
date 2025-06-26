namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IExportFileService
    {
        Task<byte[]> ExportStudentsExcelFileAsync();
        Task<byte[]> ExportMedicalInventoriesExcelFileAsync();
        Task<byte[]> ExportVaccinationDetailsExcelFileAsync();
        Task<byte[]> ExportVaccinationResultsExcelFileAsync();
        Task<byte[]> ExportHealthCheckResultsExcelFileAsync();

    }

}
