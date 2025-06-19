using SchoolMedicalServer.Abstractions.Dtos.MedicalEvent;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class MedicalRequestService(IMedicalRequestRepository requestRepository, IUserRepository userRepository) : IMedicalRequestService
    {
        public async Task<PaginationResponse<MedicalRequestResponse>> GetMedicalRequestsAsync(PaginationRequest? pagination)
        {
            var total = await requestRepository.CountAsync();
            var skip = (pagination!.PageIndex - 1) * pagination.PageSize;
            var requests = await requestRepository.GetMedicalRequestsAsync(pagination.PageSize, skip, pagination.Search);

            var nurseInfor = await userRepository.GetByIdAsync(requests.Select(r => r.Event!.StaffNurseId!).FirstOrDefault() ?? Guid.Empty);
            var response = requests.Select(r => new MedicalRequestResponse
            {
                EventInfo = new EventInfo
                {
                    EventId = r.Event!.EventId,
                },
                MedicalInfo = new MedicalInfo
                {
                    ItemId = r.Item!.ItemId,
                    ItemName = r.Item.ItemName,
                    RequestId = r.RequestId,
                    RequestQuantity = r.RequestQuantity,
                    RequestDate = r.RequestDate
                },
                NurseInfo = new NurseInfo
                {
                    NurseId = nurseInfor!.UserId,
                    FullName = nurseInfor!.FullName!,
                },
            }).ToList();
            return new PaginationResponse<MedicalRequestResponse>(
                pagination.PageIndex,
                pagination.PageSize,
                total,
                response
            );
        }
    }
}
