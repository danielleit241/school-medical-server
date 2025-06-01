using SchoolMedicalServer.Abstractions.Dtos;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class MedicalInventoryService(SchoolMedicalManagementContext context) : IMedicalInventoryService
    {
        public Task<PaginationResponse<MedicalInventoryDto>?> PaginationMedicalInventoriesAsync(PaginationRequest? pagination)
        {
            throw new NotImplementedException();
        }
    }
}
