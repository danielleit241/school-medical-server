using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class ImportFileService(
        IStudentRepository studentRepository,
        IMedicalInventoryRepository medicalInventoryRepository,
        IHealthProfileRepository healthProfileRepository,
        IBaseRepository baseRepository,
        IVaccinationDetailsRepository vaccinationDetailsRepository) : IImportFileService
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

                        await vaccinationDetailsRepository.AddAsync(vaccineDetail);
                    }
                    await baseRepository.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error uploading vaccination details: {ex.Message}", ex);
            }
        }
    }
}