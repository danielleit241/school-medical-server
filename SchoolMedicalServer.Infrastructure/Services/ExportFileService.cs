using ClosedXML.Excel;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;
using SchoolMedicalServer.Infrastructure.Repositories;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class ExportFileService(
        IStudentRepository studentRepository,
        IMedicalInventoryRepository medicalInventoryRepository,
        IVaccinationDetailsRepository vaccinationDetailsRepository,
        IVaccinationResultRepository vaccinationResultRepository,
        IHealthCheckResultRepository healthCheckResultRepository,
        IVaccinationObservationRepository vaccinationObservationRepository) : IExportFileService
    {
        public async Task<byte[]> ExportStudentsExcelFileAsync()
        {
            try
            {
                var students = await studentRepository.GetAllAsync();

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Students");

                // Fixed header order - there was an issue in the original code
                worksheet.Cell(1, 1).Value = "StudentCode";
                worksheet.Cell(1, 2).Value = "FullName";
                worksheet.Cell(1, 3).Value = "DayOfBirth";
                worksheet.Cell(1, 4).Value = "Gender";
                worksheet.Cell(1, 5).Value = "Grade";
                worksheet.Cell(1, 6).Value = "Address";
                worksheet.Cell(1, 7).Value = "ParentPhoneNumber";
                worksheet.Cell(1, 8).Value = "ParentEmailAddress";

                var titleRange = worksheet.Range(1, 1, 1, 8);
                titleRange.Style.Font.Bold = true;
                titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRange.Style.Font.FontSize = 12;
                titleRange.Style.Fill.BackgroundColor = XLColor.LightBlue;

                int row = 2;
                foreach (var student in students)
                {
                    worksheet.Cell(row, 1).Value = student.StudentCode;
                    worksheet.Cell(row, 2).Value = student.FullName;
                    worksheet.Cell(row, 3).Value = student.DayOfBirth.HasValue
                        ? student.DayOfBirth.Value.ToString("yyyy-MM-dd")
                        : "";
                    worksheet.Cell(row, 4).Value = student.Gender;
                    worksheet.Cell(row, 5).Value = student.Grade;
                    worksheet.Cell(row, 6).Value = student.Address ?? "";
                    worksheet.Cell(row, 7).Value = student.ParentPhoneNumber;
                    worksheet.Cell(row, 8).Value = student.ParentEmailAddress;
                    row++;
                }
                worksheet.Columns().AdjustToContents(); // Tự động căn lề cột
                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                return stream.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error exporting students: {ex.Message}", ex);
            }
        }

        public async Task<byte[]> ExportMedicalInventoriesExcelFileAsync()
        {
            try
            {
                var inventories = await medicalInventoryRepository.GetAllAsync();

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("MedicalInventories");

                worksheet.Cell(1, 1).Value = "ItemId";
                worksheet.Cell(1, 2).Value = "ItemName";
                worksheet.Cell(1, 3).Value = "Category";
                worksheet.Cell(1, 4).Value = "Description";
                worksheet.Cell(1, 5).Value = "QuantityInStock";
                worksheet.Cell(1, 6).Value = "UnitOfMeasure";
                worksheet.Cell(1, 7).Value = "MinimumStockLevel";
                worksheet.Cell(1, 8).Value = "MaximumStockLevel";
                worksheet.Cell(1, 9).Value = "LastImportDate";
                worksheet.Cell(1, 10).Value = "LastExportDate";
                worksheet.Cell(1, 11).Value = "ExpiryDate";
                worksheet.Cell(1, 12).Value = "Status";

                var titleRange = worksheet.Range(1, 1, 1, 12);
                titleRange.Style.Font.Bold = true;
                titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRange.Style.Font.FontSize = 12;
                titleRange.Style.Fill.BackgroundColor = XLColor.LightBlue;

                int row = 2;
                foreach (var inventory in inventories)
                {
                    worksheet.Cell(row, 1).Value = inventory.ItemId.ToString();
                    worksheet.Cell(row, 2).Value = inventory.ItemName;
                    worksheet.Cell(row, 3).Value = inventory.Category;
                    worksheet.Cell(row, 4).Value = inventory.Description;
                    worksheet.Cell(row, 5).Value = inventory.QuantityInStock;
                    worksheet.Cell(row, 6).Value = inventory.UnitOfMeasure;
                    worksheet.Cell(row, 7).Value = inventory.MinimumStockLevel;
                    worksheet.Cell(row, 8).Value = inventory.MaximumStockLevel;
                    worksheet.Cell(row, 9).Value = inventory.LastImportDate.HasValue
                        ? inventory.LastImportDate.Value.ToString("yyyy-MM-dd HH:mm:ss")
                        : "";
                    worksheet.Cell(row, 10).Value = inventory.LastExportDate.HasValue
                        ? inventory.LastExportDate.Value.ToString("yyyy-MM-dd HH:mm:ss")
                        : "";
                    worksheet.Cell(row, 11).Value = inventory.ExpiryDate.HasValue
                        ? inventory.ExpiryDate.Value.ToString("yyyy-MM-dd")
                        : "";
                    worksheet.Cell(row, 12).Value = inventory.Status ? "Available" : "Unavailable";
                    row++;
                }
                worksheet.Columns().AdjustToContents(); // Tự động căn lề cột
                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                return stream.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error exporting medical inventories: {ex.Message}", ex);
            }
        }

        public async Task<byte[]> ExportVaccinationDetailsExcelFileAsync()
        {
            try
            {
                var vaccinationDetails = await vaccinationDetailsRepository.GetAllAsync();

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("VaccinationDetails");

                worksheet.Cell(1, 1).Value = "VaccineId";
                worksheet.Cell(1, 2).Value = "VaccineCode";
                worksheet.Cell(1, 3).Value = "VaccineName";
                worksheet.Cell(1, 4).Value = "Manufacturer";
                worksheet.Cell(1, 5).Value = "VaccineType";
                worksheet.Cell(1, 6).Value = "AgeRecommendation";
                worksheet.Cell(1, 7).Value = "BatchNumber";
                worksheet.Cell(1, 8).Value = "ExpirationDate";
                worksheet.Cell(1, 9).Value = "ContraindicationNotes";
                worksheet.Cell(1, 10).Value = "Description";
                worksheet.Cell(1, 11).Value = "CreatedAt";
                worksheet.Cell(1, 12).Value = "UpdatedAt";

                var titleRange = worksheet.Range(1, 1, 1, 12);
                titleRange.Style.Font.Bold = true;
                titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRange.Style.Font.FontSize = 12;
                titleRange.Style.Fill.BackgroundColor = XLColor.LightBlue;

                int row = 2;
                foreach (var detail in vaccinationDetails)
                {
                    worksheet.Cell(row, 1).Value = detail.VaccineId.ToString();
                    worksheet.Cell(row, 2).Value = detail.VaccineCode ?? "";
                    worksheet.Cell(row, 3).Value = detail.VaccineName ?? "";
                    worksheet.Cell(row, 4).Value = detail.Manufacturer ?? "";
                    worksheet.Cell(row, 5).Value = detail.VaccineType ?? "";
                    worksheet.Cell(row, 6).Value = detail.AgeRecommendation ?? "";
                    worksheet.Cell(row, 7).Value = detail.BatchNumber ?? "";
                    worksheet.Cell(row, 8).Value = detail.ExpirationDate.HasValue
                        ? detail.ExpirationDate.Value.ToString("yyyy-MM-dd")
                        : "";
                    worksheet.Cell(row, 9).Value = detail.ContraindicationNotes ?? "";
                    worksheet.Cell(row, 10).Value = detail.Description ?? "";
                    worksheet.Cell(row, 11).Value = detail.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss");
                    worksheet.Cell(row, 12).Value = detail.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss");
                    row++;
                }
                worksheet.Columns().AdjustToContents(); // Tự động căn lề cột
                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                return stream.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error exporting vaccination details: {ex.Message}", ex);
            }
        }

            public async Task<byte[]> ExportVaccinationResultsExcelFileAsync(Guid roundId)
            {
            try
            {
                var vaccinationResults = await vaccinationResultRepository.GetByRoundIdAsync(roundId);

                var filteredResults = vaccinationResults
                    .Where(r => r.RoundId == roundId)
                    .ToList();

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("VaccinationResults");

                worksheet.Cell(1, 1).Value = "StudentCode";
                worksheet.Cell(1, 2).Value = "FullName";
                worksheet.Cell(1, 3).Value = "DateOfBirth";
                worksheet.Cell(1, 4).Value = "Gender";
                worksheet.Cell(1, 5).Value = "Grade";
                worksheet.Cell(1, 6).Value = "ParentPhone";
                worksheet.Cell(1, 7).Value = "ParentConfirmed";
                worksheet.Cell(1, 8).Value = "HealthQualified";
                worksheet.Cell(1, 9).Value = "Vaccinated";
                worksheet.Cell(1, 10).Value = "VaccinatedDate";
                worksheet.Cell(1, 11).Value = "VaccinatedTime";
                worksheet.Cell(1, 12).Value = "InjectionSite";
                worksheet.Cell(1, 13).Value = "Notes";
                worksheet.Cell(1, 14).Value = "ObservationNotes";
                worksheet.Cell(1, 15).Value = "ObservationStartTime";
                worksheet.Cell(1, 16).Value = "ObservationEndTime";
                worksheet.Cell(1, 17).Value = "ReactionStartTime";
                worksheet.Cell(1, 18).Value = "ReactionType";
                worksheet.Cell(1, 19).Value = "SeverityLevel";
                worksheet.Cell(1, 20).Value = "ImmediateReaction";
                worksheet.Cell(1, 21).Value = "Intervention";
                worksheet.Cell(1, 22).Value = "ObservedBy";

                var titleRange = worksheet.Range(1, 1, 1, 22);
                titleRange.Style.Font.Bold = true;
                titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRange.Style.Font.FontSize = 12;
                titleRange.Style.Fill.BackgroundColor = XLColor.LightBlue;

                int row = 2;

                foreach (var result in filteredResults)
                {
                    var student = result.HealthProfile?.Student;
                    var observation = result.VaccinationObservation;

                    string healthQualifiedText = (result.HealthQualified.HasValue && result.HealthQualified.Value)
                        ? "Qualified"
                        : "Not Qualified";

                    worksheet.Cell(row, 1).Value = student?.StudentCode ?? "";
                    worksheet.Cell(row, 2).Value = student?.FullName ?? "";
                    worksheet.Cell(row, 3).Value = student?.DayOfBirth.HasValue == true
                        ? student.DayOfBirth.Value.ToString("yyyy-MM-dd")
                        : "";
                    worksheet.Cell(row, 4).Value = student?.Gender ?? "";
                    worksheet.Cell(row, 5).Value = student?.Grade ?? "";
                    worksheet.Cell(row, 6).Value = student?.ParentPhoneNumber ?? "";
                    worksheet.Cell(row, 7).Value = result.ParentConfirmed.HasValue
                        ? (result.ParentConfirmed.Value ? "Confirmed" : "Declined")
                        : "Pending";
                    worksheet.Cell(row, 8).Value = healthQualifiedText;
                    worksheet.Cell(row, 9).Value = result.Vaccinated ? "Yes" : "No";
                    worksheet.Cell(row, 10).Value = result.VaccinatedDate.HasValue
                        ? result.VaccinatedDate.Value.ToString("yyyy-MM-dd")
                        : "Not vaccinated yet";
                    worksheet.Cell(row, 11).Value = result.VaccinatedTime.HasValue
                        ? result.VaccinatedTime.Value.ToString("yyyy-MM-dd HH:mm:ss")
                        : "Not vaccinated yet";
                    worksheet.Cell(row, 12).Value = !string.IsNullOrEmpty(result.InjectionSite)
                        ? result.InjectionSite
                        : "Not specified";
                    worksheet.Cell(row, 13).Value = !string.IsNullOrEmpty(result.Notes)
                        ? result.Notes
                        : "No notes";

     
                    if (result.HealthQualified.HasValue && !result.HealthQualified.Value)
                    {
               
                        worksheet.Cell(row, 14).Value = "Not qualified";
                        worksheet.Cell(row, 15).Value = "Not qualified";
                        worksheet.Cell(row, 16).Value = "Not qualified";
                        worksheet.Cell(row, 17).Value = "Not qualified";
                        worksheet.Cell(row, 18).Value = "Not qualified";
                        worksheet.Cell(row, 19).Value = "Not qualified";
                        worksheet.Cell(row, 20).Value = "Not qualified";
                        worksheet.Cell(row, 21).Value = "Not qualified";
                        worksheet.Cell(row, 22).Value = "Not qualified";
                    }
                    else if (!result.Vaccinated)
                    {
                      
                        worksheet.Cell(row, 14).Value = "Failed";
                        worksheet.Cell(row, 15).Value = "Failed";
                        worksheet.Cell(row, 16).Value = "Failed";
                        worksheet.Cell(row, 17).Value = "Failed";
                        worksheet.Cell(row, 18).Value = "Failed";
                        worksheet.Cell(row, 19).Value = "Failed";
                        worksheet.Cell(row, 20).Value = "Failed";
                        worksheet.Cell(row, 21).Value = "Failed";
                        worksheet.Cell(row, 22).Value = "Failed";
                    }
                    else
                    {
                        
                        worksheet.Cell(row, 14).Value = observation?.Notes ?? "No observation notes";
                        worksheet.Cell(row, 15).Value = observation?.ObservationStartTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "";
                        worksheet.Cell(row, 16).Value = observation?.ObservationEndTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "";
                        worksheet.Cell(row, 17).Value = observation?.ReactionStartTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "";
                        worksheet.Cell(row, 18).Value = observation?.ReactionType ?? "";
                        worksheet.Cell(row, 19).Value = observation?.SeverityLevel ?? "";
                        worksheet.Cell(row, 20).Value = observation?.ImmediateReaction ?? "";
                        worksheet.Cell(row, 21).Value = observation?.Intervention ?? "";
                        worksheet.Cell(row, 22).Value = observation?.ObservedBy ?? "";
                    }

                    row++;
                }

                worksheet.Columns().AdjustToContents();
                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                return stream.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error exporting vaccination results: {ex.Message}", ex);
            }
        }

        public async Task<byte[]> ExportHealthCheckResultsExcelFileAsync(Guid roundId)
        {
            try
            {
                var healthCheckResults = await healthCheckResultRepository.GetAllAsync();
                var filteredResults = healthCheckResults
                    .Where(r => r.RoundId == roundId)
                    .ToList();

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("HealthCheckResults");
           
                worksheet.Cell(1, 1).Value = "Student Code";
                worksheet.Cell(1, 2).Value = "Full Name";
                worksheet.Cell(1, 3).Value = "Date of Birth";
                worksheet.Cell(1, 4).Value = "Gender";
                worksheet.Cell(1, 5).Value = "Grade";
                worksheet.Cell(1, 6).Value = "Parent Phone";
                worksheet.Cell(1, 7).Value = "Parent Confirm";
                // Các trường cũ
                worksheet.Cell(1, 8).Value = "DatePerformed";
                worksheet.Cell(1, 9).Value = "Height";
                worksheet.Cell(1, 10).Value = "Weight";
                worksheet.Cell(1, 11).Value = "VisionLeft";
                worksheet.Cell(1, 12).Value = "VisionRight";
                worksheet.Cell(1, 13).Value = "Hearing";
                worksheet.Cell(1, 14).Value = "Nose";
                worksheet.Cell(1, 15).Value = "BloodPressure";
                worksheet.Cell(1, 16).Value = "Status";
                worksheet.Cell(1, 17).Value = "Notes";
         


                var titleRange = worksheet.Range(1, 1, 1, 17);
                titleRange.Style.Font.Bold = true;
                titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRange.Style.Font.FontSize = 12;
                titleRange.Style.Fill.BackgroundColor = XLColor.LightBlue;

                int row = 2;

                foreach (var result in filteredResults)
                {
                    var student = result.HealthProfile?.Student;
                 
                    worksheet.Cell(row, 1).Value = student?.StudentCode ?? "";
                    worksheet.Cell(row, 2).Value = student?.FullName ?? "";
                    worksheet.Cell(row, 3).Value = student?.DayOfBirth.HasValue == true
                        ? student.DayOfBirth.Value.ToString("dd/MM/yyyy")
                        : "";
                    worksheet.Cell(row, 4).Value = student?.Gender ?? "";
                    worksheet.Cell(row, 5).Value = student?.Grade ?? "";
                    worksheet.Cell(row, 6).Value = student?.ParentPhoneNumber ?? "";
                    worksheet.Cell(row, 7).Value = result.ParentConfirmed.HasValue
                        ? (result.ParentConfirmed.Value ? "Confirmed" : "Declined")
                        : "Pending";
                    worksheet.Cell(row, 8).Value = result.DatePerformed.HasValue
                        ? result.DatePerformed.Value.ToString("yyyy-MM-dd")
                        : "not recorded yet ";
                    worksheet.Cell(row, 9).Value = result.Height?.ToString() ?? "not recorded yet";
                    worksheet.Cell(row, 10).Value = result.Weight?.ToString() ?? "not recorded yet";
                    worksheet.Cell(row, 11).Value = result.VisionLeft?.ToString() ?? "not recorded yet";
                    worksheet.Cell(row, 12).Value = result.VisionRight?.ToString() ?? "not recorded yet";
                    worksheet.Cell(row, 13).Value = result.Hearing ?? "not recorded yet";
                    worksheet.Cell(row, 14).Value = result.Nose ?? "not recorded yet";
                    worksheet.Cell(row, 15).Value = result.BloodPressure ?? "not recorded yet";
                    worksheet.Cell(row, 16).Value = result.Status ?? "";
                    worksheet.Cell(row, 17).Value = result.Notes ?? "not recorded yet";
                  
                    row++;
                }
                worksheet.Columns().AdjustToContents(); // Tự động căn lề cột
                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                return stream.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error exporting health check results: {ex.Message}", ex);
            }
        }
    }
    
}