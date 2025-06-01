using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class UploadService(SchoolMedicalManagementContext context) : IUploadService
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

                        context.MedicalInventories.Add(medicalInventory);
                    }
                    await context.SaveChangesAsync();
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
                        var studentCode = row.Cell(1).GetString();
                        var fullName = row.Cell(2).GetString();
                        DateOnly? dayOfBirth = null;
                        if (row.Cell(3).DataType == XLDataType.DateTime)
                        {
                            var dateTime = row.Cell(3).GetDateTime();
                            dayOfBirth = DateOnly.FromDateTime(dateTime);
                        }
                        else
                        {
                            if (DateTime.TryParse(row.Cell(3).GetString(), out var parsedDate))
                            {
                                dayOfBirth = DateOnly.FromDateTime(parsedDate);
                            }
                        }

                        var gender = row.Cell(4).GetString();
                        var grade = row.Cell(5).GetString();
                        var address = row.Cell(6).GetString();
                        var parentPhoneNumber = row.Cell(7).GetString();
                        var parentEmailAddress = row.Cell(8).GetString();

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
                            ParentEmailAddress = parentEmailAddress
                        };

                        var studentHealthProfile = new HealthProfile
                        {
                            HealthProfileId = Guid.NewGuid(),
                            StudentId = studentId,
                            CreatedDate = DateTime.UtcNow,
                        };

                        context.HealthProfiles.Add(studentHealthProfile);
                        context.Students.Add(student);
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error uploading students: {ex.Message}", ex);
            }
        }
    }
}
