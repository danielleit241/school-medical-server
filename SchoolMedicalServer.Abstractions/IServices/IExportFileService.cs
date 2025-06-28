namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IExportFileService
    {
        Task<byte[]> ExportStudentsExcelFileAsync();
        Task<byte[]> ExportMedicalInventoriesExcelFileAsync();
        Task<byte[]> ExportVaccinationDetailsExcelFileAsync();
        Task<byte[]> ExportVaccinationResultsExcelFileAsync(Guid roundId);
        Task<byte[]> ExportHealthCheckResultsExcelFileAsync(Guid roundId);

    }

}
