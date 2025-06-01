using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Dtos.MedicalEvent;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class MedicalEventService(SchoolMedicalManagementContext context) : IMedicalEventService
    {
        public async Task<bool> IsEnoughQuantityAsync(List<MedicalRequestDtoRequest> medicalRequests)
        {
            foreach (var request in medicalRequests)
            {
                var item = await context.MedicalInventories.Where(i => i.ItemId == request.ItemId).FirstOrDefaultAsync();
                if (item == null)
                    return false;
                if (item.QuantityInStock < request.RequestQuantity)
                    return false;
                if ((item.QuantityInStock - request.RequestQuantity) < item.MinimumStockLevel)
                    return false;
            }
            return true;
        }
        public async Task<bool> CreateMedicalEventAsync(MedicalEventRequest request)
        {
            var student = await context.Students.FirstOrDefaultAsync(s => s.StudentCode == request.MedicalEvent.StudentCode);
            if (student == null)
            {
                return false;
            }
            var staffNurse = await context.Users.FindAsync(request.MedicalEvent.StaffNurseId);
            if (staffNurse == null)
            {
                return false;
            }

            foreach (var item in request.MedicalRequests!)
            {
                var inventoryItem = await context.MedicalInventories.FindAsync(item.ItemId);
                inventoryItem!.QuantityInStock -= item.RequestQuantity ?? 0;
                context.MedicalInventories.Update(inventoryItem);
            }

            var medicalEvent = new MedicalEvent
            {
                EventId = Guid.NewGuid(),
                StudentId = student.StudentId,
                StaffNurseId = staffNurse.UserId,
                EventType = request.MedicalEvent.EventType,
                EventDescription = request.MedicalEvent.EventDescription,
                Location = request.MedicalEvent.Location,
                SeverityLevel = request.MedicalEvent.SeverityLevel,
                ParentNotified = request.MedicalEvent.ParentNotified,
                EventDate = DateOnly.FromDateTime(DateTime.Now),
                Notes = request.MedicalEvent.Notes
            };
            var medicalRequests = request.MedicalRequests?.Select(r => new MedicalRequest
            {
                RequestId = Guid.NewGuid(),
                ItemId = r.ItemId,
                RequestQuantity = r.RequestQuantity,
                Purpose = r.Purpose,
                MedicalEventId = medicalEvent.EventId,
                RequestDate = DateOnly.FromDateTime(DateTime.Now)
            }).ToList() ?? new List<MedicalRequest>();

            context.MedicalEvents.Add(medicalEvent);
            context.MedicalRequests.AddRange(medicalRequests);

            await context.SaveChangesAsync();
            return true;
        }

        public Task<PaginationResponse<MedicalEventResponse>?> GetAllStudentMedicalEventsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<MedicalEventResponse?> GetMedicalEventDetailAsync(Guid medicalEventId)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResponse<MedicalEventResponse>?> GetMedicalEventsByStudentIdAsync(Guid studentId)
        {
            throw new NotImplementedException();
        }
    }
}
