using SchoolMedicalServer.Abstractions.Dtos.MedicalEvent;
using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class MedicalEventService(
        IBaseRepository baseRepository,
        IMedicalEventRepository eventRepo,
        IMedicalRequestRepository requestRepo,
        IMedicalInventoryRepository inventoryRepo,
        IStudentRepository studentRepo,
        IUserRepository userRepo
    ) : IMedicalEventService
    {
        public async Task<bool> IsEnoughQuantityAsync(List<MedicalRequestDtoRequest> medicalRequests)
        {
            foreach (var request in medicalRequests)
            {
                var item = await inventoryRepo.GetByIdAsync(request.ItemId);
                if (item == null)
                    return false;
                if (item.QuantityInStock < request.RequestQuantity)
                    return false;
                if ((item.QuantityInStock - request.RequestQuantity) < item.MinimumStockLevel)
                    return false;
            }
            return true;
        }

        public async Task<NotificationMedicalEventResponse> CreateMedicalEventAsync(MedicalEventRequest request)
        {
            var student = await studentRepo.FindByStudentCodeAsync(request.MedicalEvent.StudentCode);
            if (student == null)
                return null!;
            var nurse = await userRepo.GetByIdAsync(request.MedicalEvent.StaffNurseId);
            if (nurse == null)
                return null!;

            foreach (var item in request.MedicalRequests!)
            {
                var inventoryItem = await inventoryRepo.GetByIdAsync(item.ItemId);
                if (inventoryItem == null) return null!;
                inventoryItem.QuantityInStock -= item.RequestQuantity ?? 0;
                inventoryItem.LastExportDate = DateTime.UtcNow;
                if (inventoryItem.QuantityInStock == inventoryItem.MinimumStockLevel)
                {
                    inventoryItem.Status = false;
                }
                inventoryRepo.Update(inventoryItem);
            }

            var medicalEventId = Guid.NewGuid();
            var medicalEvent = new MedicalEvent
            {
                EventId = medicalEventId,
                StudentId = student.StudentId,
                StaffNurseId = request.MedicalEvent.StaffNurseId,
                EventType = request.MedicalEvent.EventType,
                EventDescription = request.MedicalEvent.EventDescription,
                Location = request.MedicalEvent.Location,
                SeverityLevel = request.MedicalEvent.SeverityLevel,
                ParentNotified = request.MedicalEvent.ParentNotified,
                EventDate = DateOnly.FromDateTime(DateTime.Now),
                Notes = request.MedicalEvent.Notes,
                MedicalRequests = request.MedicalRequests?.Select(r => new MedicalRequest
                {
                    RequestId = Guid.NewGuid(),
                    ItemId = r.ItemId,
                    RequestQuantity = r.RequestQuantity,
                    Purpose = r.Purpose,
                    MedicalEventId = medicalEventId,
                    RequestDate = DateOnly.FromDateTime(DateTime.Now)
                }).ToList() ?? []
            };
            await eventRepo.AddAsync(medicalEvent);
            await baseRepository.SaveChangesAsync();

            var manager = await userRepo.GetUserByRoleName("manager") ?? await userRepo.GetUserByRoleName("admin");
            var parentId = await studentRepo.GetParentUserIdAsync(medicalEvent.StudentId);
            var notifications = new NotificationMedicalEventResponse()
            {
                ToManager = new NotificationRequest
                {
                    NotificationTypeId = medicalEvent.EventId,
                    SenderId = medicalEvent.StaffNurseId,
                    ReceiverId = manager?.UserId
                },
                ToParent = new NotificationRequest
                {
                    NotificationTypeId = medicalEvent.EventId,
                    SenderId = medicalEvent.StaffNurseId,
                    ReceiverId = parentId,
                }
            };
            return notifications;
        }

        public async Task<PaginationResponse<MedicalEventResponse>?> GetAllStudentMedicalEventsAsync(PaginationRequest? paginationRequest)
        {
            var totalCount = await eventRepo.CountAsync(paginationRequest?.Search);
            if (totalCount == 0)
                return null!;

            int skip = (paginationRequest!.PageIndex - 1) * paginationRequest.PageSize;
            var events = await eventRepo.GetPagedSearchBySeveriryLevelAsync(skip, paginationRequest.PageSize, paginationRequest.Search);

            var result = new List<MedicalEventResponse>();

            foreach (var medicalEvent in events)
            {
                var medicalRequests = await requestRepo.GetByEventIdAsync(medicalEvent.EventId);
                var student = await studentRepo.GetStudentByIdAsync(medicalEvent.StudentId);
                var studentInfo = MapToStudentInfo(student);
                var response = GetResponse(medicalEvent, medicalRequests, studentInfo);
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
            var medicalEvent = await eventRepo.GetByIdAsync(medicalEventId);
            if (medicalEvent == null)
                return null!;

            var medicalRequests = await requestRepo.GetByEventIdAsync(medicalEvent.EventId);
            var student = await studentRepo.GetStudentByIdAsync(medicalEvent.StudentId);
            var studentInfo = MapToStudentInfo(student);
            var response = GetResponse(medicalEvent, medicalRequests, studentInfo);

            return response;
        }

        public async Task<PaginationResponse<MedicalEventResponse>?> GetMedicalEventsByStudentIdAsync(PaginationRequest? paginationRequest, Guid studentId)
        {
            var totalCount = await eventRepo.CountByStudentIdAsync(studentId);
            if (totalCount == 0)
                return null!;

            int skip = (paginationRequest!.PageIndex - 1) * paginationRequest.PageSize;
            var events = await eventRepo.GetByStudentIdPagedAsync(studentId, skip, paginationRequest.PageSize);

            var result = new List<MedicalEventResponse>();

            foreach (var medicalEvent in events)
            {
                var medicalRequests = await requestRepo.GetByEventIdAsync(medicalEvent.EventId);
                var student = await studentRepo.GetStudentByIdAsync(medicalEvent.StudentId);
                var studentInfo = MapToStudentInfo(student);
                var response = GetResponse(medicalEvent, medicalRequests, studentInfo);
                result.Add(response);
            }

            return new PaginationResponse<MedicalEventResponse>(
                paginationRequest.PageIndex,
                paginationRequest.PageSize,
                totalCount,
                result
            );
        }

        private MedicalEventResponse GetResponse(MedicalEvent medicalEvent, List<MedicalRequestDtoResponse> medicalRequests, StudentInforResponse studentInfor)
        {
            return new MedicalEventResponse
            {
                MedicalEvent = MaptoDto(medicalEvent),
                MedicalRequests = medicalRequests,
                StudentInfo = studentInfor
            };
        }

        private MedicalEventResponseDto MaptoDto(MedicalEvent medicalEvent)
        {
            return new MedicalEventResponseDto
            {
                EventId = medicalEvent.EventId,
                StaffNurseId = medicalEvent.StaffNurseId,
                EventDate = medicalEvent.EventDate,
                EventType = medicalEvent.EventType,
                EventDescription = medicalEvent.EventDescription,
                Location = medicalEvent.Location,
                SeverityLevel = medicalEvent.SeverityLevel,
                Notes = medicalEvent.Notes
            };
        }

        public async Task<IEnumerable<MedicalEventResponse>> GetAllMedicalEvent()
        {
            var medicalEvents = await eventRepo.GetAllMedicalEvent();

            if (medicalEvents == null || !medicalEvents.Any())
            {
                return Enumerable.Empty<MedicalEventResponse>();
            }
            var result = new List<MedicalEventResponse>();
            foreach (var medicalEvent in medicalEvents)
            {
                var medicalRequests = await requestRepo.GetByEventIdAsync(medicalEvent.EventId);
                var student = await studentRepo.GetStudentByIdAsync(medicalEvent.StudentId);
                var studentInfo = new StudentInforResponse
                {
                    StudentId = student?.StudentId,
                    FullName = student?.FullName,
                    StudentCode = student?.StudentCode,
                };
                var response = GetResponse(medicalEvent, medicalRequests, studentInfo);
                result.Add(response);
            }
            return result;
        }

        private StudentInforResponse MapToStudentInfo(Student? student)
        {
            return new StudentInforResponse
            {
                StudentId = student.StudentId,
                FullName = student.FullName,
                StudentCode = student.StudentCode,
                ParentId = student.UserId,
                ParentFullName = student.User?.FullName,
                ParentPhoneNumber = student.User?.PhoneNumber
            };
        }
    }
}