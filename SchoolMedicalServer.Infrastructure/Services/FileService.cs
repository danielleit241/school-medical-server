using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class FileService(
        IStudentRepository studentRepository,
        IMedicalInventoryRepository medicalInventoryRepository,
        IHealthProfileRepository healthProfileRepository,
        IBaseRepository baseRepository,
        IVaccinationDetailsRepository vacctionDetailsRepository) : IFileService
    {
        public async Task UploadMedicalInventoriesExcelFile(IFormFile file)
        {
            try
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                stream.Position = 0;

                using (var workbook = new XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheet(1);
                    bool isFirstRow = true;

                    foreach (var row in worksheet.RowsUsed())
                    {
                        if (isFirstRow)
                        {
                            isFirstRow = false;
                            continue;
                        }

                        var itemId = Guid.NewGuid();
                        var itemName = row.Cell(1).GetString();
                        var category = row.Cell(2).GetString();
                        var description = row.Cell(3).GetString();
                        var unitOfMeasure = row.Cell(4).GetString();

                        DateTime? expiryDate = null;
                        var expiryCell = row.Cell(5);
                        if (!expiryCell.IsEmpty())
                        {
                            if (DateTime.TryParse(expiryCell.GetString(), out DateTime parsedDate))
                            {
                                expiryDate = parsedDate;
                            }
                            else if (expiryCell.DataType == XLDataType.DateTime)
                            {
                                expiryDate = expiryCell.GetDateTime();
                            }
                        }

                        var maxStockLevel = row.Cell(6).GetValue<int>();
                        var minStockLevel = row.Cell(7).GetValue<int>();
                        var quantityInStock = row.Cell(8).GetValue<int>();
                        var statusCell = row.Cell(9).GetString();
                        bool status = statusCell.Equals("True", StringComparison.OrdinalIgnoreCase)
                                    || statusCell.Equals("Available", StringComparison.OrdinalIgnoreCase)
                                    || statusCell.Equals("1");

                        var medicalInventory = new MedicalInventory
                        {
                            ItemId = itemId,
                            ItemName = itemName,
                            Category = category,
                            Description = description ?? "",
                            UnitOfMeasure = unitOfMeasure ?? "",
                            ExpiryDate = expiryDate,
                            MaximumStockLevel = maxStockLevel,
                            MinimumStockLevel = minStockLevel,
                            LastImportDate = DateTime.UtcNow,
                            QuantityInStock = quantityInStock,
                            Status = status
                        };

                        await medicalInventoryRepository.AddAsync(medicalInventory);
                    }
                    await baseRepository.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error uploading medical inventories: {ex.Message}", ex);
            }
        }

        public async Task UploadStudentsExcelFile(IFormFile file)
        {
            try
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                stream.Position = 0;

                using (var workbook = new XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheet(1);
                    bool isFirstRow = true;

                    foreach (var row in worksheet.RowsUsed())
                    {
                        if (isFirstRow)
                        {
                            isFirstRow = false;
                            continue;
                        }

                        var studentId = Guid.NewGuid();
                        //var studentCode = row.Cell(1).GetString();
                        var studentCode = await studentRepository.GenerateStudentCodeAsync();
                        var fullName = row.Cell(1).GetString();
                        DateOnly? dayOfBirth = null;
                        if (row.Cell(2).DataType == XLDataType.DateTime)
                        {
                            var dateTime = row.Cell(2).GetDateTime();
                            dayOfBirth = DateOnly.FromDateTime(dateTime);
                        }
                        else
                        {
                            if (DateTime.TryParse(row.Cell(2).GetString(), out var parsedDate))
                            {
                                dayOfBirth = DateOnly.FromDateTime(parsedDate);
                            }
                        }

                        var gender = row.Cell(3).GetString();
                        var grade = row.Cell(4).GetString();
                        var address = row.Cell(5).GetString();
                        var parentPhoneNumber = row.Cell(6).GetString();
                        var parentEmailAddress = row.Cell(7).GetString();

                        var student = new Student
                        {
                            StudentId = studentId,
                            StudentCode = studentCode,
                            FullName = fullName,
                            DayOfBirth = dayOfBirth,
                            Gender = gender,
                            Grade = grade,
                            Address = address ?? "",
                            ParentPhoneNumber = parentPhoneNumber,
                            ParentEmailAddress = parentEmailAddress,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };

                        var studentHealthProfile = new HealthProfile
                        {
                            HealthProfileId = Guid.NewGuid(),
                            StudentId = studentId,
                            CreatedDate = DateTime.UtcNow,
                        };

                        await healthProfileRepository.AddAsync(studentHealthProfile);
                        await studentRepository.AddAsync(student);
                        await baseRepository.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error uploading students: {ex.Message}", ex);
            }
        }

        public async Task UploadVaccinationDetailFile(IFormFile file)
        {
            try
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                stream.Position = 0;
                using (var workbook = new XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheet(1);
                    bool isFirstRow = true;
                    foreach (var row in worksheet.RowsUsed())
                    {
                        if (isFirstRow)
                        {
                            isFirstRow = false;
                            continue;
                        }
                        var vaccineId = Guid.NewGuid();
                        var vaccineCode = row.Cell(1).GetString();
                        var vaccineName = row.Cell(2).GetString();
                        var manufacturer = row.Cell(3).GetString();
                        var vaccineType = row.Cell(4).GetString();
                        var ageRecommendation = row.Cell(5).GetString();
                        var batchNumber = row.Cell(6).GetString();
                        DateOnly? expirationDate = null;
                        if (row.Cell(7).DataType == XLDataType.DateTime)
                        {
                            var dateTime = row.Cell(7).GetDateTime();
                            expirationDate = DateOnly.FromDateTime(dateTime);
                        }
                        else
                        {
                            if (DateTime.TryParse(row.Cell(7).GetString(), out var parsedDate))
                            {
                                expirationDate = DateOnly.FromDateTime(parsedDate);
                            }
                        }
                        var contraindicationNotes = row.Cell(8).GetString();
                        var description = row.Cell(9).GetString();

                        var vaccineDetail = new VaccinationDetail
                        {
                            VaccineId = vaccineId,
                            VaccineCode = vaccineCode,
                            VaccineName = vaccineName,
                            Manufacturer = manufacturer,
                            VaccineType = vaccineType,
                            AgeRecommendation = ageRecommendation,
                            BatchNumber = batchNumber,
                            ExpirationDate = expirationDate,
                            ContraindicationNotes = contraindicationNotes ?? "",
                            Description = description ?? "",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };

                        await vacctionDetailsRepository.AddAsync(vaccineDetail);
                    }
                    await baseRepository.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error uploading vaccination details: {ex.Message}", ex);
            }
        }

        public async Task<byte[]> ExportStudentsExcelFileAsync()
        {
            try
            {
                var students = await studentRepository.GetAllAsync();

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Students");

                worksheet.Cell(1, 1).Value = "StudentCode";
                worksheet.Cell(1, 1).Value = "FullName";
                worksheet.Cell(1, 2).Value = "DayOfBirth";
                worksheet.Cell(1, 3).Value = "Gender";
                worksheet.Cell(1, 4).Value = "Grade";
                worksheet.Cell(1, 5).Value = "Address";
                worksheet.Cell(1, 6).Value = "ParentPhoneNumber";
                worksheet.Cell(1, 7).Value = "ParentEmailAddress";

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
                var vaccinationDetails = await vacctionDetailsRepository.GetAllAsync();

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

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                return stream.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error exporting vaccination details: {ex.Message}", ex);
            }
        }
    }
}
