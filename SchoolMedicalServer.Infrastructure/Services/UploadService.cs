using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class UploadService(SchoolMedicalManagementContext context) : IUploadService
    {
        public async Task UploadExcelFile(IFormFile file)
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
                throw new Exception("An error occurred while uploading the Excel file.", ex);
            }
        }
    }
}
