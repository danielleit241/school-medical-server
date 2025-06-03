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
        public async Task<MedicalEvent> CreateMedicalEventAsync(MedicalEventRequest request)
        {
            var student = await context.Students.FirstOrDefaultAsync(s => s.StudentCode == request.MedicalEvent.StudentCode);
            if (student == null)
            {
                return null;
            }
            var staffNurse = await context.Users.FindAsync(request.MedicalEvent.StaffNurseId);
            if (staffNurse == null)
            {
                return null;
            }

            foreach (var item in request.MedicalRequests!)
            {
                var inventoryItem = await context.MedicalInventories.Where(i => i.ItemId == item.ItemId && i.Status == true).FirstOrDefaultAsync();
                inventoryItem!.QuantityInStock -= item.RequestQuantity ?? 0;
                if (inventoryItem.QuantityInStock == inventoryItem.MinimumStockLevel)
                {
                    inventoryItem.Status = false;
                }
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
            return medicalEvent;
        }

        public async Task<PaginationResponse<MedicalEventResponse>?> GetAllStudentMedicalEventsAsync(PaginationRequest? paginationRequest)
        {
            var totalCount = await context.MedicalEvents.CountAsync();
            if (totalCount == 0)
            {
                return null!;
            }

            var events = await context.MedicalEvents
                .OrderByDescending(e => e.EventDate)
                .Skip((paginationRequest!.PageIndex - 1) * paginationRequest.PageSize)
                .Take(paginationRequest.PageSize)
                .ToListAsync();

            var result = new List<MedicalEventResponse>();

            foreach (var medicalEvent in events)
            {
                var medicalRequests = await GetMedicalRequestsByEventIdAsync(medicalEvent.EventId);

                var medicalEventDto = MaptoDto(medicalEvent);

                var response = GetResponse(medicalEvent, medicalRequests);

                result.Add(response);
            }

            return new PaginationResponse<MedicalEventResponse>(
                paginationRequest.PageIndex,
                paginationRequest.PageSize,
                totalCount,
                result
            );
        }

        public async Task<MedicalEventResponse?> GetMedicalEventDetailAsync(Guid medicalEventId)
        {
            var medicalEvent = await context.MedicalEvents
                .Where(e => e.EventId == medicalEventId)
                .FirstOrDefaultAsync();

            if (medicalEvent == null)
            {
                return null!;
            }

            var medicalRequests = await GetMedicalRequestsByEventIdAsync(medicalEvent.EventId);

            var medicalEventDto = MaptoDto(medicalEvent);

            var response = GetResponse(medicalEvent, medicalRequests);

            return response;
        }


        public async Task<PaginationResponse<MedicalEventResponse>?> GetMedicalEventsByStudentIdAsync(PaginationRequest? paginationRequest, Guid studentId)
        {
            var totalCount = await context.MedicalEvents
                .Where(e => e.StudentId == studentId)
                .CountAsync();

            if (totalCount == 0)
            {
                return null!;
            }

            var events = await context.MedicalEvents
                .Where(e => e.StudentId == studentId)
                .OrderByDescending(e => e.EventDate)
                .Skip((paginationRequest!.PageIndex - 1) * paginationRequest.PageSize)
                .Take(paginationRequest.PageSize)
                .ToListAsync();

            var result = new List<MedicalEventResponse>();

            foreach (var medicalEvent in events)
            {
                var medicalRequests = await GetMedicalRequestsByEventIdAsync(medicalEvent.EventId);

                var medicalEventDto = MaptoDto(medicalEvent);

                var response = GetResponse(medicalEvent, medicalRequests);

                result.Add(response);
            }

            return new PaginationResponse<MedicalEventResponse>(
                paginationRequest.PageIndex,
                paginationRequest.PageSize,
                totalCount,
                result
            );
        }

        private MedicalEventResponse GetResponse(MedicalEvent medicalEvent, List<MedicalRequestDtoResponse> medicalRequests)
        {
            return new MedicalEventResponse
            {
                MedicalEvent = MaptoDto(medicalEvent),
                MedicalRequests = medicalRequests
            };
        }

        private MedicalEventDtoResponse MaptoDto(MedicalEvent medicalEvent)
        {
            return new MedicalEventDtoResponse
            {
                EventId = medicalEvent.EventId,
                StudentId = medicalEvent.StudentId,
                StaffNurseId = medicalEvent.StaffNurseId,
                EventDate = medicalEvent.EventDate,
                EventType = medicalEvent.EventType,
                EventDescription = medicalEvent.EventDescription,
                Location = medicalEvent.Location,
                SeverityLevel = medicalEvent.SeverityLevel,
                Notes = medicalEvent.Notes
            };
        }

        private async Task<List<MedicalRequestDtoResponse>> GetMedicalRequestsByEventIdAsync(Guid eventId)
        {
            return await context.MedicalRequests
                .Where(r => r.MedicalEventId == eventId)
                .Join(context.MedicalInventories,
                      r => r.ItemId,
                      i => i.ItemId,
                      (r, i) => new MedicalRequestDtoResponse
                      {
                          RequestId = r.RequestId,
                          ItemId = r.ItemId,
                          ItemName = i.ItemName,
                          RequestQuantity = r.RequestQuantity
                      })
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
